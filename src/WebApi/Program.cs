using App.InvoiSysTest.Application.Extensions;
using App.InvoiSysTest.Infrastructure.Extensions;
using App.InvoiSysTest.WebApi.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Strategyo.Components.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
       .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(JwtConfigurations.EncodedSecurityKey)
            };
        });

builder.Services.AddCors();

builder.Services
       .AddAuthorizationBuilder()
       .AddPolicy(nameof(JwtConfigurations.InvoiSysTestRead), policy => policy.RequireRole(JwtConfigurations.InvoiSysTestRead))
       .AddPolicy(nameof(JwtConfigurations.InvoiSysTestWrite), policy => policy.RequireRole(JwtConfigurations.InvoiSysTestWrite));

builder.Services.AddSwaggerStrategyo("App.InvoiSysTest");

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerStrategyo();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(policyBuilder =>
{
    policyBuilder
       .AllowAnyOrigin()
       .AllowAnyMethod()
       .AllowAnyHeader();
});

await app.RunAsync();