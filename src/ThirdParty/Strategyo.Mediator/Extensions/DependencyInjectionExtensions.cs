using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Strategyo.Mediator.Configuration;
using Strategyo.Mediator.Registration;

namespace Strategyo.Mediator.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
        => AddMediator(services, Assembly.GetExecutingAssembly());
    
    public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] assemblies)
    {
        var serviceConfig = new StrategyoMediatorConfiguration();
        
        serviceConfig.AssembliesToRegister.AddRange(assemblies);
        
        return services.AddMediator(serviceConfig);
    }
    
    public static IServiceCollection AddMediator(this IServiceCollection services, 
                                                 Action<StrategyoMediatorConfiguration> configuration)
    {
        var serviceConfig = new StrategyoMediatorConfiguration();

        configuration.Invoke(serviceConfig);

        return services.AddMediator(serviceConfig);
    }
    
    public static IServiceCollection AddMediator(this IServiceCollection services, 
                                                 StrategyoMediatorConfiguration configuration)
    {
        if (configuration.AssembliesToRegister.Count == 0)
        {
            throw new ArgumentException("No assemblies found to scan. Supply at least one assembly to scan for handlers.");
        }

        ServiceRegistrar.AddRequiredServices(services, configuration);
        ServiceRegistrar.AddRequiredRequestHandler(services, configuration);
        ServiceRegistrar.AddRequiredRequestHandlerWrapperImpl(services, configuration);

        return services;
    }
}