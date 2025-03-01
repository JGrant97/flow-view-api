using flow_view_database.ApplicationRole;
using flow_view_database.ApplicationUser;
using flow_view_database.RefreshToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace flow_view.Auth;
public class AuthApiHelper : IAuthApiHelper
{
    public async Task<Results<Ok<AuthRefreshDTO>, UnauthorizedHttpResult, NotFound>> LoginAsync(IRefreshTokenRepository refreshTokenRepository,
        IConfiguration configuration,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        AuthLoginDTO model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);

        if (user is null)
            return TypedResults.NotFound();

        var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);

        if (!result.Succeeded)
            return TypedResults.Unauthorized();

        var token = await AuthJWTHandler.GenerateToken(configuration, userManager, user);
        var refreshToken = await refreshTokenRepository.GenerateRefreshTokenAsync(user, "FlowViewAPI");

        return TypedResults.Ok(new AuthRefreshDTO() { Token = token, RefreshToken = refreshToken });
    }

    public async Task<Results<Ok, UnauthorizedHttpResult>> LogoutAsync(HttpContext content,
        ClaimsPrincipal userClaim,
        IRefreshTokenRepository refreshTokenRepository,
        UserManager<ApplicationUser> userManager)
    {
        var userId = userClaim.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return TypedResults.Unauthorized();

        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return TypedResults.Unauthorized();

        await refreshTokenRepository.RevokeRefreshTokenAsync(user, "FlowViewAPI");

        return TypedResults.Ok();
    }

    public async Task<Results<Ok<AuthRefreshDTO>, UnauthorizedHttpResult, NotFound<string>, BadRequest<string>>> RefreshAsync(IConfiguration configuration,
        IRefreshTokenRepository refreshTokenRepository,
        TokenValidationParameters tokenValidationParameters,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        AuthRefreshDTO model)
    {
        var principal = AuthJWTHandler.GetPrincipalFromExpiredToken(model.Token, tokenValidationParameters);

        if (principal == null)
            return TypedResults.BadRequest("Invalid token");

        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
            return TypedResults.NotFound("User not found");

        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return TypedResults.NotFound("User not found");

        var isValidRefreshToken = await refreshTokenRepository.ValidateRefreshTokenAsync(model.RefreshToken, user, "FlowViewAPI");

        if (user is null || !isValidRefreshToken)
            return TypedResults.Unauthorized();

        var newJwtToken = await AuthJWTHandler.GenerateToken(configuration, userManager, user);
        var newRefreshToken = await refreshTokenRepository.GenerateRefreshTokenAsync(user, "FlowViewAPI");

        return TypedResults.Ok(new AuthRefreshDTO() { Token = newJwtToken, RefreshToken = newRefreshToken });
    }

    public async Task<Results<Ok, InternalServerError<IEnumerable<IdentityError>>>> RegisterAsync(UserManager<ApplicationUser> userManager, IEmailSender emailSender, RoleManager<ApplicationRole> roleManager, AuthRegisterDTO model)
    {
        var user = new ApplicationUser()
        {
            DisplayName = model.DisplayName,
            UserName = model.Email,
            Email = model.Email,
        };
        var roleName = "user";
        var result = await userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            bool roleExists = await roleManager.RoleExistsAsync(roleName);
            if (roleExists)
            {
                await userManager.AddToRoleAsync(user, roleName);
            }
            else
            {
                var role = new ApplicationRole();
                role.Name = roleName;
                await roleManager.CreateAsync(role);
                await userManager.AddToRoleAsync(user, roleName);
            }

            //var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            //var confirmationLink = $"{model.emailCallbackUrl}/{Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Id))}/{Convert.ToBase64String(Encoding.UTF8.GetBytes(token))}";

            //await emailSender.SendEmailAsync(user.Email, "Confirm your email", $"Please confirm your email by <a href='{confirmationLink}'>clicking here</a>");

            return TypedResults.Ok();
        }

        return TypedResults.InternalServerError(result.Errors);
    }

    public async Task<Results<Ok, NotFound<string>, BadRequest<string>>> ConfirmEmailAsync(UserManager<ApplicationUser> userManager, AuthConfirmEmailDTO confirmEmailDTO)
    {
        var user = await userManager.FindByIdAsync(confirmEmailDTO.UserId);

        if (user is null)
            return TypedResults.NotFound("User does not exists.");

        var result = await userManager.ConfirmEmailAsync(user, confirmEmailDTO.Token);

        if (!result.Succeeded)
            return TypedResults.BadRequest("Invalid token.");

        return TypedResults.Ok();
    }

}
