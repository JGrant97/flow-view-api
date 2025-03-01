using flow_view_database.ApplicationUser;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace flow_view.Auth;

public static class AuthJWTHandler
{
    public static async Task<string> GenerateToken(IConfiguration configuration, UserManager<ApplicationUser> userManager, ApplicationUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>([
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.EmailVerified, user.EmailConfirmed.ToString()),
            new Claim("UserName", user.DisplayName),
        ]);

        var roles = await userManager.GetRolesAsync(user);

        claims.AddRange(roles.Select(role => new Claim("roles", role)));

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }
        return Convert.ToBase64String(randomNumber);
    }

    public static ClaimsPrincipal GetPrincipalFromExpiredToken(
         string token,
         TokenValidationParameters tokenValidationParameters)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        tokenValidationParameters.ValidateLifetime = false;

        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }

    public static void AddJWTAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var tokenValidationParameters = new TokenValidationParameters()
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
        };

        services.AddAuthorization();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = tokenValidationParameters;
        });

        services.AddSingleton(tokenValidationParameters);
    }
}