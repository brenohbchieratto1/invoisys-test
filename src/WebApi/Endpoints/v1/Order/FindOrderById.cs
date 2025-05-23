using App.InvoiSysTest.Application.UseCases.Order.FindOrderById.Input;
using App.InvoiSysTest.Application.UseCases.Order.FindOrderById.Output;
using App.InvoiSysTest.WebApi.Configurations;
using App.InvoiSysTest.WebApi.Endpoints.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Strategyo.Components.Api.Extensions;
using Strategyo.Mediator.Interfaces;
using Strategyo.Results.Contracts.Results;

namespace App.InvoiSysTest.WebApi.Endpoints.v1.Order;

public class FindOrderById : BaseOrder
{
    protected override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
           .MapGet("{id}", async (
                        [FromHeader] Guid correlationId,
                        [FromRoute] string id,
                        [FromServices] IMediator mediator,
                        [FromServices] IMemoryCache cache,
                        CancellationToken ct) =>
                    {
                        var cacheKey = $"Order:{id}";

                        if (cache.TryGetValue(cacheKey, out Result<FindOrderByIdOutput>? cachedResult))
                        {
                            return Result(cachedResult!);
                        }

                        var input = new FindOrderByIdInput(id);
                        input.SetCorrelationId(correlationId);
                        
                        var result = await mediator.SendAsync(input, ct);

                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                           .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                        cache.Set(cacheKey, result, cacheEntryOptions);

                        return Result(result);
                    })
           .WithSwaggerOperation("Busca um pedido de compra", "Responsável por buscar um pedido de compra")
           .WithResultDefaultStatus200OK<FindOrderByIdOutput>()
           .WithOpenApi()
           .RequireAuthorization(nameof(JwtConfigurations.InvoiSysTestRead));
    }
}