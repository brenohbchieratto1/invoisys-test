/*using Strategyo.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Strategyo.Extensions.RegisterExtensions;

public static class DependencyInjection
{
    public static void AddDynamicScopedImplementation(this IServiceCollection services, Type interfaceType)
    {
        var assembly = TypesHelper.AllTypes;
        
        var implementationType = assembly.FirstOrDefault(t => interfaceType.IsAssignableFrom(t) && t is
        {
            IsClass: true, 
            IsAbstract: false
        });

        if (implementationType != null)
        {
            services.AddScoped(interfaceType, implementationType);
        }
        else
        {
            throw new InvalidOperationException($"Não foi encontrada nenhuma implementação para {interfaceType.Name}");
        }
    }
    
    public static void AddSingletonTypes<T>(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(TypesHelper.Assemblies);
        
        Log.Information($"Registering types for {typeof(T).Name}");
        services.Scan(scan => scan.FromAssemblies(TypesHelper.Assemblies)
                                  .AddClasses(classes => classes.AssignableTo<T>())
                                  .AsSelf()
                                  .AsImplementedInterfaces()
                                  .WithSingletonLifetime());
    }
    
    public static void AddScopedTypes<T>(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(TypesHelper.Assemblies);
        
        Log.Information($"Registering types for {typeof(T).Name}");
        services.Scan(scan => scan.FromAssemblies(TypesHelper.Assemblies)
                                  .AddClasses(classes => classes.AssignableTo<T>())
                                  .AsSelf()
                                  .AsImplementedInterfaces()
                                  .WithScopedLifetime());
    }
    
    public static void AddTransientTypes<T>(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(TypesHelper.Assemblies);
        
        Log.Information($"Registering types for {typeof(T).Name}");
        services.Scan(scan => scan.FromAssemblies(TypesHelper.Assemblies)
                                  .AddClasses(classes => classes.AssignableTo<T>())
                                  .AsSelf()
                                  .AsImplementedInterfaces()
                                  .WithTransientLifetime());
    }
}*/