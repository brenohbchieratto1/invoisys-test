using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Strategyo.Components.Api.Interfaces;
using Strategyo.Results.Contracts.Results;
using Swashbuckle.AspNetCore.Annotations;
using Log = Serilog.Log;

namespace Strategyo.Components.Api.Extensions;

public static class MinimalApisExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        var assembly = Assembly.GetEntryAssembly()!;
        
        var endpointServiceDescriptors = assembly
                                        .DefinedTypes
                                        .Where(type => type is { IsInterface: false, IsAbstract: false } && typeof(IEndpoint).IsAssignableFrom(type))
                                        .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
                                        .ToList();
        
        Log.Information("Founded {Founded} endpoints", endpointServiceDescriptors.Count);
        
        services.TryAddEnumerable(endpointServiceDescriptors);
        
        return services;
    }

    public static IEndpointRouteBuilder MapEndpoints(this WebApplication app)
    {
        var services = app.Services;
        var endpoints = services.GetRequiredService<IEnumerable<IEndpoint>>();

        foreach (var endpoint in endpoints)
        {
            endpoint.MapGroup(app);
        }

        return app;
    }

    #region Builder

    public static RouteHandlerBuilder WithSwaggerOperation(this RouteHandlerBuilder builder, string summary, string description)
        => builder.WithMetadata(new SwaggerOperationAttribute(summary, description));

    public static RouteHandlerBuilder WithResultDefaultStatus200OK<T>(this RouteHandlerBuilder builder)
        => builder.Produces<Result<T>>();
    
    public static RouteHandlerBuilder WithStatus200OK<T>(this RouteHandlerBuilder builder)
        => builder.Produces<T>();
    
    public static RouteHandlerBuilder WithStatus400BadRequest<T>(this RouteHandlerBuilder builder)
        => builder.Produces<Result<T>>(StatusCodes.Status400BadRequest);

    #endregion
}