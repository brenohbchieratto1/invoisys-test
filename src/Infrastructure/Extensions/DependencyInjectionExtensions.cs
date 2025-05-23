using App.InvoisysTest.Domain.Interfaces;
using App.InvoisysTest.Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace App.InvoisysTest.Infrastructure.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection service)
    {
        service.AddScoped<IOrderRepository, OrderRepository>();
        
        return service;
    }
}