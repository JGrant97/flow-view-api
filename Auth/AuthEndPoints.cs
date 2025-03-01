using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using flow_view_database.RefreshToken;
using Microsoft.AspNetCore.Identity.UI.Services;
using flow_view_database.ApplicationUser;
using flow_view_database.ApplicationRole;

namespace flow_view.Auth;
public static class AuthEndPoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/auth/logout", async (
            HttpContext context,
            ClaimsPrincipal userClaim,
            IRefreshTokenRepository refreshTokenRepository,
            IAuthApiHelper authApiHelper,
            UserManager<ApplicationUser> userManager) =>
        await authApiHelper.LogoutAsync(context, userClaim, refreshTokenRepository, userManager))
       .WithTags("Auth")
       .RequireAuthorization();

        app.MapPost("/auth/login", async (
            IRefreshTokenRepository refreshTokenRepository,
            IConfiguration configuration,
            IAuthApiHelper authApiHelper,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            [FromBody] AuthLoginDTO model) =>
         await authApiHelper.LoginAsync(refreshTokenRepository, configuration, signInManager, userManager, model))
        .WithTags("Auth");

        app.MapPost("/auth/refresh", async (IConfiguration configuration,
            IRefreshTokenRepository refreshTokenRepository,
            IAuthApiHelper authApiHelper,
            TokenValidationParameters tokenValidationParameters,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            [FromBody] AuthRefreshDTO model) =>
        await authApiHelper.RefreshAsync(configuration, refreshTokenRepository, tokenValidationParameters, signInManager, userManager, model))
       .WithTags("Auth");

        app.MapPost("/auth/register", async (UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            IAuthApiHelper authApiHelper,
            RoleManager<ApplicationRole> roleManager,
            [FromBody] AuthRegisterDTO model) =>
        await authApiHelper.RegisterAsync(userManager, emailSender, roleManager, model))
        .WithTags("Auth");

        app.MapPost("/auth/confirmEmail", async (UserManager<ApplicationUser> userManager,
            IAuthApiHelper authApiHelper,
            [FromBody] AuthConfirmEmailDTO confirmEmailDTO) =>
        await authApiHelper.ConfirmEmailAsync(userManager, confirmEmailDTO))
        .WithTags("Auth");
    }
}
