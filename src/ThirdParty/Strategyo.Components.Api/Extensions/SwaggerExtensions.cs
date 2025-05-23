using System.Text.Json;
using System.Text.Json.Serialization;
using Cysharp.Serialization.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Strategyo.Components.Api.Extensions;

public static class SwaggerExtensions
{
    public static void AddSwaggerStrategyo(this IServiceCollection services, string name)
    {
        services.AddEndpointsApiExplorer();
        
        services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();
            options.DescribeAllParametersInCamelCase();
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = name,
            });
            options.CustomSchemaIds(x => x.FullName);
            
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Digite 'Bearer' + espaço + seu token JWT.\n\nExemplo: Bearer abc123"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
            });
        });
        
        services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.SerializerOptions.Converters.Add(new UlidJsonConverter());
            options.SerializerOptions.WriteIndented = true;
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

        services.AddEndpoints();
    }

    public static void UseSwaggerStrategyo(this WebApplication app)
    {
        app.UseSwagger();

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("./v1/swagger.json", "v1 Docs");
            c.DocumentTitle = "Documentation";
            c.DocExpansion(DocExpansion.None);
        });

        app.MapEndpoints();
    }
}