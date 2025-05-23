using App.InvoiSysTest.Application.UseCases.Order.FindPaginableOrder.Input;
using App.InvoiSysTest.Application.UseCases.Order.FindPaginableOrder.Output;
using App.InvoiSysTest.WebApi.Configurations;
using App.InvoiSysTest.WebApi.Endpoints.Base;
using Microsoft.AspNetCore.Mvc;
using Strategyo.Components.Api.Extensions;
using Strategyo.Mediator.Interfaces;
using Strategyo.Results.Contracts.Paginable;

namespace App.InvoiSysTest.WebApi.Endpoints.v1.Order;

public class FindPaginableOrder : BaseOrder
{
    protected override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app
           .MapGet("", async (
                       [FromHeader] Guid correlationId,
                       [FromQuery] int pageSize,
                       [FromQuery] int pageNumber,
                       [FromServices] IMediator mediator,
                       CancellationToken ct) =>
                   {
                       var input = new FindPaginableOrderInput(pageNumber, pageSize);
                       input.SetCorrelationId(correlationId);
                        
                       var result = await mediator.SendAsync(input, ct);

                       return Result(result);
                   })
           .WithSwaggerOperation("Busca paginada dos pedidos de compra", "Responsável por buscar com paginação os pedidos de compra")
           .WithResultDefaultStatus200OK<PaginableResult<PaginableOutput>>()
           .WithOpenApi()
           .RequireAuthorization(nameof(JwtConfigurations.InvoiSysTestRead));
    }
}