using App.InvoiSysTest.Domain.Interfaces;
using App.InvoiSysTest.Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace App.InvoiSysTest.Infrastructure.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection service)
    {
        service.AddScoped<IOrderRepository, OrderRepository>();
        
        return service;
    }
}