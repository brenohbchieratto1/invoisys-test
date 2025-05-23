using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Strategyo.Mediator.Extensions;

namespace App.InvoiSysTest.Application.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
        => services
          .AddValidators()
          .AddMediator();

    private static IServiceCollection AddMediator(this IServiceCollection services)
        => services
           .AddMediator(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjectionExtensions).Assembly);
            });
    
    private static IServiceCollection AddValidators(this IServiceCollection services)
        => services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
}