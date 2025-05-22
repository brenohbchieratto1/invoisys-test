using Microsoft.Extensions.DependencyInjection;
using Strategyo.Mediator.Interfaces;

namespace Strategyo.Mediator.Entities;

public class OpenBehavior
{
    public OpenBehavior(Type openBehaviorType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        ValidatePipelineBehaviorType(openBehaviorType);
        OpenBehaviorType = openBehaviorType;
        ServiceLifetime = serviceLifetime;
    }

    public Type OpenBehaviorType { get; }

    public ServiceLifetime ServiceLifetime { get; }

    private static void ValidatePipelineBehaviorType(Type? openBehaviorType)
    {
        ArgumentNullException.ThrowIfNull(openBehaviorType);

        var isPipelineBehavior = openBehaviorType
                                .GetInterfaces()
                                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPipelineBehavior<,>));

        if (!isPipelineBehavior)
        {
            throw new InvalidOperationException($"The type \"{openBehaviorType.Name}\" must implement IPipelineBehavior<,> interface.");
        }
    }
}