using App.InvoiSysTest.Application.UseCases.Order.CreateOrder.Input;
using App.InvoiSysTest.Application.UseCases.Order.CreateOrder.Output;
using App.InvoiSysTest.WebApi.Configurations;
using App.InvoiSysTest.WebApi.Contracts.Order.Request;
using App.InvoiSysTest.WebApi.Endpoints.Base;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Strategyo.Components.Api.Extensions;
using Strategyo.Mediator.Interfaces;

namespace App.InvoiSysTest.WebApi.Endpoints.v1.Order;

public class CreateOrder : BaseOrder
{
    protected override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
           .MapPost("", async (
                        [FromHeader] Guid correlationId,
                        [FromBody] CreateOrderRequest req,
                        [FromServices] IMediator mediator,
                        CancellationToken ct) =>
                    {
                        var input = req.Adapt<CreateOrderInput>();
                        input.SetCorrelationId(correlationId);
                        
                        var result = await mediator.SendAsync(input, ct).ConfigureAwait(false);

                        return Result(result);
                    })
           .WithSwaggerOperation("Cria um pedido de compra", "Responsável por criar um pedido de compra")
           .WithResultDefaultStatus200OK<CreateOrderOutput>()
           .WithOpenApi()
           .RequireAuthorization(nameof(JwtConfigurations.InvoiSysTestWrite));
    }
}