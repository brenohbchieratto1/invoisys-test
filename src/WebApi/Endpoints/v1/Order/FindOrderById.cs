using App.InvoiSysTest.Application.UseCases.Order.FindOrderById.Input;
using App.InvoiSysTest.Application.UseCases.Order.FindOrderById.Output;
using App.InvoiSysTest.WebApi.Configurations;
using App.InvoiSysTest.WebApi.Endpoints.Base;
using Microsoft.AspNetCore.Mvc;
using Strategyo.Components.Api.Extensions;
using Strategyo.Mediator.Interfaces;

namespace App.InvoiSysTest.WebApi.Endpoints.v1.Order;

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
           .WithResultDefaultStatus200OK<FindOrderByIdOutput>()
           .WithOpenApi()
           .RequireAuthorization(nameof(JwtConfigurations.InvoiSysTestRead));
    }
}