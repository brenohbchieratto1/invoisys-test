using App.InvoisysTest.Application.UseCases.Order.FindPaginableOrder.Input;
using App.InvoisysTest.Application.UseCases.Order.FindPaginableOrder.Output;
using App.InvoisysTest.WebApi.Endpoints.Base;
using Microsoft.AspNetCore.Mvc;
using Strategyo.Components.Api.Extensions;
using Strategyo.Mediator.Interfaces;
using Strategyo.Results.Contracts.Paginable;

namespace App.InvoisysTest.WebApi.Endpoints.v1.Order;

public class FindPaginableOrder : BaseOrder
{
    protected override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
           .MapGet("", async (
                       [FromHeader] string correlationId,
                       [FromQuery] int pageSize,
                       [FromQuery] int pageNumber,
                       [FromServices] IMediator mediator,
                       CancellationToken ct) =>
                   {
                       var input = new FindPaginableOrderInput(pageSize, pageNumber);
                       input.SetCorrelationId(correlationId);
                        
                       var result = await mediator.SendAsync(input, ct);

                       return Result(result);
                   })
           .WithSwaggerOperation("Busca paginada dos pedidos de compra", "Responsável por buscar com paginação os pedidos de compra")
           .WithResultDefaultStatus200OK<PaginableResult<PaginableOutput>>();
    }
}