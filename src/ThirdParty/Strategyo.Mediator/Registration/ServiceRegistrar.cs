using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Strategyo.Mediator.Configuration;
using Strategyo.Mediator.Interfaces;
using Strategyo.Mediator.Wrappers;

namespace Strategyo.Mediator.Registration;

public static class ServiceRegistrar
{
    public static void AddRequiredServices(IServiceCollection services, StrategyoMediatorConfiguration serviceConfiguration)
    {
        // Use TryAdd, so any existing ServiceFactory/IMediator registration doesn't get overridden
        services.TryAdd(new ServiceDescriptor(typeof(IMediator), serviceConfiguration.MediatorImplementationType, serviceConfiguration.Lifetime));

        foreach (var serviceDescriptor in serviceConfiguration.BehaviorsToRegister)
        {
            services.TryAddEnumerable(serviceDescriptor);
        }
    }

    public static void AddRequiredRequestHandler(IServiceCollection services, StrategyoMediatorConfiguration serviceConfiguration)
    {
        var handlerInterfaceType = typeof(IRequestHandler<,>);

        var handlers = serviceConfiguration
                      .AssembliesToRegister
                      .Select(assembly => assembly
                                         .GetTypes()
                                         .Where(t => t is { IsAbstract: false, IsInterface: false })
                                         .SelectMany(t => t.GetInterfaces(), (t, i) => new { Type = t, Interface = i })
                                         .Where(ti => ti.Interface.IsGenericType && ti.Interface.GetGenericTypeDefinition() == handlerInterfaceType))
                      .SelectMany(handlers => handlers)
                      .ToList();

        handlers
           .Select(handler => new ServiceDescriptor(handler.Interface, handler.Type, serviceConfiguration.Lifetime))
           .ToList()
           .ForEach(services.TryAdd);
    }
    
    public static void AddRequiredRequestHandlerWrapperImpl(IServiceCollection services, StrategyoMediatorConfiguration serviceConfiguration)
    {
        var handlerBaseType = typeof(RequestHandlerWrapperImpl<,>);

        var handlers = serviceConfiguration
                      .AssembliesToRegister
                      .SelectMany(assembly => assembly.GetTypes())
                      .Where(type => !type.IsAbstract && !type.IsInterface)
                      .Select(type => new
                       {
                           ImplementationType = type,
                           BaseType = GetGenericBaseType(type, handlerBaseType)
                       })
                      .Where(x => x.BaseType != null)
                      .ToList();

        foreach (var handler in handlers)
        {
            services.TryAdd(new ServiceDescriptor(handler.BaseType!, handler.ImplementationType, serviceConfiguration.Lifetime));
        }
    }
    
    private static Type? GetGenericBaseType(Type type, Type genericBaseType)
    {
        while (type != null && type != typeof(object))
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == genericBaseType)
                return type;

            type = type.BaseType!;
        }

        return null;
    }

}