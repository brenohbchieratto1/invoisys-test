using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Strategyo.Extensions;
using Strategyo.Mediator.Entities;
using Strategyo.Mediator.Interfaces;

namespace Strategyo.Mediator.Configuration;

public class StrategyoMediatorConfiguration
{
    public Func<Type, bool> TypeEvaluator { get; set; } = t => true;
    
    public Type MediatorImplementationType { get; set; } = typeof(Mediator);
    
    public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Transient;

    public List<ServiceDescriptor> BehaviorsToRegister { get; } = [];

    internal List<Assembly> AssembliesToRegister { get; } = [];
    
    public int MaxGenericTypeParameters { get; set; } = 10;
    
    public int MaxTypesClosing { get; set; } = 100;
    
    public int MaxGenericTypeRegistrations { get; set; } = 125000;
    
    public bool RegisterGenericHandlers { get; set; }

    public bool RegisterGenericBehaviors { get; set; } = true;

    #region Builder

    public StrategyoMediatorConfiguration RegisterServicesFromAssemblyContaining<T>()
        => RegisterServicesFromAssemblyContaining(typeof(T));

    public StrategyoMediatorConfiguration RegisterServicesFromAssemblyContaining(Type type)
        => RegisterServicesFromAssembly(type.Assembly);

    public StrategyoMediatorConfiguration RegisterServicesFromAssembly(Assembly assembly)
        => RegisterServicesFromAssemblies(assembly);

    public StrategyoMediatorConfiguration RegisterServicesFromAssemblies(
        params Assembly[] assemblies)
    {
        AssembliesToRegister.AddRange(assemblies);

        return this;
    }

    public StrategyoMediatorConfiguration AddBehavior<TServiceType, TImplementationType>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        => AddBehavior(typeof(TServiceType), typeof(TImplementationType), serviceLifetime);

    public StrategyoMediatorConfiguration AddBehavior<TImplementationType>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        => AddBehavior(typeof(TImplementationType), serviceLifetime);

    public StrategyoMediatorConfiguration AddBehavior(Type implementationType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        var implementedGenericInterfaces = implementationType.FindInterfacesThatClose(typeof(IPipelineBehavior<,>)).ToList();

        if (implementedGenericInterfaces.Count == 0)
        {
            throw new InvalidOperationException($"{implementationType.Name} must implement {typeof(IPipelineBehavior<,>).FullName}");
        }

        implementedGenericInterfaces
           .ForEach(x => AddBehavior(x, implementationType, serviceLifetime));

        return this;
    }

    public StrategyoMediatorConfiguration AddBehavior(Type serviceType, Type implementationType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        BehaviorsToRegister.Add(new ServiceDescriptor(serviceType, implementationType, serviceLifetime));

        return this;
    }

    public StrategyoMediatorConfiguration AddOpenBehaviors(IEnumerable<Type> openBehaviorTypes, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        openBehaviorTypes
           .ToList()
           .ForEach(x => AddOpenBehavior(x, serviceLifetime));
        
        return this;
    }

    public StrategyoMediatorConfiguration AddOpenBehaviors(IEnumerable<OpenBehavior> openBehaviors)
    {
        openBehaviors
           .ToList()
           .ForEach(x => AddOpenBehavior(x.OpenBehaviorType, x.ServiceLifetime));

        return this;
    }

    public StrategyoMediatorConfiguration AddOpenBehavior(Type openBehaviorType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        if (!openBehaviorType.IsGenericType)
        {
            throw new InvalidOperationException($"{openBehaviorType.Name} must be generic");
        }

        var implementedGenericInterfaces = openBehaviorType
                                          .GetInterfaces()
                                          .Where(i => i.IsGenericType)
                                          .Select(i => i.GetGenericTypeDefinition());

        var implementedOpenBehaviorInterfaces = new HashSet<Type>(implementedGenericInterfaces.Where(i => i == typeof(IPipelineBehavior<,>)));

        if (implementedOpenBehaviorInterfaces.Count == 0)
        {
            throw new InvalidOperationException($"{openBehaviorType.Name} must implement {typeof(IPipelineBehavior<,>).FullName}");
        }
        
        implementedOpenBehaviorInterfaces
           .ToList()
           .ForEach(x => AddBehavior(x, openBehaviorType, serviceLifetime));
        
        return this;
    }

    #endregion
}