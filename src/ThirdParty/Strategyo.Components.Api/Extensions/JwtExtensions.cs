using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using Strategyo.Components.Api.Contracts;

namespace Strategyo.Components.Api.Extensions;

public static class JwtExtensions
{
    public static string GenerateJwt(this User user, string jwtKey)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var bytes = Encoding.ASCII.GetBytes(jwtKey);
        var key = new SymmetricSecurityKey(bytes);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTimeOffset.UtcNow.AddDays(1).DateTime,
            TokenType = CookieAuthenticationDefaults.AuthenticationScheme,
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
            Claims = new Dictionary<string, object>
            {
                [$"{nameof(User.Id)}"] = user.Id.ToString(),
                [$"{nameof(User.TenantId)}"] = user.TenantId.ToString(),
                [$"{nameof(User.Email)}"] = user.Email         ?? string.Empty,
                [$"{nameof(User.UserName)}"] = user.UserName   ?? string.Empty,
                [$"{nameof(User.AvatarUrl)}"] = user.AvatarUrl ?? string.Empty,
                [$"{nameof(User.Enabled)}"] = user.Enabled,
                [$"{nameof(User.Roles)}"] = user.Roles             ?? [],
                [$"{nameof(User.Permissions)}"] = user.Permissions ?? []
            }
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }

    public static (bool isValid, JwtSecurityToken token) ValidateJwt(this string jwt)
    {
        if (string.IsNullOrWhiteSpace(jwt))
        {
            return (false, new JwtSecurityToken());
        }

        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var jwtTokenDecoded = jwtTokenHandler.ReadJwtToken(jwt);

        if (jwtTokenDecoded.ValidTo.ToUniversalTime() <= DateTimeOffset.UtcNow)
        {
            return (false, jwtTokenDecoded);
        }

        return (true, jwtTokenDecoded);
    }
}