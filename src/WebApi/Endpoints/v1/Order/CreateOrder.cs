using App.InvoisysTest.WebApi.Contracts.Order.Request;
using App.InvoisysTest.WebApi.Contracts.Order.Response;
using App.InvoisysTest.WebApi.Endpoints.Base;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Strategyo.Components.Api.Extensions;
using Strategyo.Mediator.Interfaces;

namespace App.InvoisysTest.WebApi.Endpoints.v1.Order;

public class CreateOrder : BaseOrder
{
    protected override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
           .MapPost("", async (
                        [FromHeader] Ulid correlationId,
                        [FromBody] CreateOrderRequest req,
                        [FromServices] IMediator mediator,
                        CancellationToken ct) =>
                    {
                        var input = req.Adapt<object>();
                        input.SetCorrelationId(correlationId);
                        
                        var result = await mediator.SendAsync(input, ct);

                        return Result(result);
                    })
           .WithSwaggerOperation("Cria um pedido de compra", "Responsável por criar um pedido de compra")
           .WithStatus200OK<CreateOrderResponse>();
    }
}