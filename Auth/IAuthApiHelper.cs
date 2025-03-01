using flow_view_database.ApplicationRole;
using flow_view_database.ApplicationUser;
using flow_view_database.RefreshToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace flow_view.Auth;

public interface IAuthApiHelper
{
    Task<Results<Ok, UnauthorizedHttpResult>> LogoutAsync(HttpContext context,
        ClaimsPrincipal userClaim,
        IRefreshTokenRepository refreshTokenRepository,
        UserManager<ApplicationUser> userManager);

    Task<Results<Ok<AuthRefreshDTO>, UnauthorizedHttpResult, NotFound>> LoginAsync(IRefreshTokenRepository refreshTokenRepository,
        IConfiguration configuration,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        AuthLoginDTO model);

    Task<Results<Ok<AuthRefreshDTO>, UnauthorizedHttpResult, NotFound<string>, BadRequest<string>>> RefreshAsync(IConfiguration configuration,
        IRefreshTokenRepository refreshTokenRepository,
        TokenValidationParameters tokenValidationParameters,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        AuthRefreshDTO model);

    Task<Results<Ok, InternalServerError<IEnumerable<IdentityError>>>> RegisterAsync(UserManager<ApplicationUser> userManager,
        IEmailSender emailSender,
        RoleManager<ApplicationRole> roleManager,
        AuthRegisterDTO model);

    Task<Results<Ok, NotFound<string>, BadRequest<string>>> ConfirmEmailAsync(UserManager<ApplicationUser> userManager,
        AuthConfirmEmailDTO confirmEmailDTO);
}
