using App.InvoisysTest.Application.UseCases.Order.FindOrderById.Input;
using App.InvoisysTest.Application.UseCases.Order.FindOrderById.Output;
using App.InvoisysTest.WebApi.Endpoints.Base;
using Microsoft.AspNetCore.Mvc;
using Strategyo.Components.Api.Extensions;
using Strategyo.Mediator.Interfaces;

namespace App.InvoisysTest.WebApi.Endpoints.v1.Order;

public class FindOrderById : BaseOrder
{
    protected override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
           .MapGet("{id}", async (
                        [FromHeader] string correlationId,
                        [FromRoute] string id,
                        [FromServices] IMediator mediator,
                        CancellationToken ct) =>
                    {
                        var input = new FindOrderByIdInput(id);
                        input.SetCorrelationId(correlationId);
                        
                        var result = await mediator.SendAsync(input, ct);

                        return Result(result);
                    })
           .WithSwaggerOperation("Busca um pedido de compra", "Responsável por buscar um pedido de compra")
           .WithResultDefaultStatus200OK<FindOrderByIdOutput>();
    }
}