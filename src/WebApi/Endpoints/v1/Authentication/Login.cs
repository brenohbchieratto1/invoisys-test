using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using App.InvoiSysTest.WebApi.Configurations;
using App.InvoiSysTest.WebApi.Endpoints.Base;
using Microsoft.IdentityModel.Tokens;
using Strategyo.Components.Api.Extensions;

namespace App.InvoiSysTest.WebApi.Endpoints.v1.Authentication;

public class Login : BaseAuthentication
{
    protected override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
           .MapPost("", () =>
            {
                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, "Usuário Teste"),
                    new(ClaimTypes.Role, "invoisys-test-read"),
                    new(ClaimTypes.Role, "invoisys-test-write"),
                };

                var key = new SymmetricSecurityKey(JwtConfigurations.EncodedSecurityKey);
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var expiresIn = DateTime.MaxValue;

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: expiresIn,
                    signingCredentials: creds);

                var tokenString = new JwtSecurityTokenHandler()
                   .WriteToken(token);

                return Results.Ok(new { token = tokenString, expiresIn = expiresIn });
            })
           .WithSwaggerOperation("Cria um token", "Responsável por criar um token");
    }
}