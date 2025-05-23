using App.InvoiSysTest.Application.UseCases.Order.FindPaginableOrder.Input;
using App.InvoiSysTest.Application.UseCases.Order.FindPaginableOrder.Output;
using App.InvoiSysTest.Domain.Interfaces;
using Mapster;
using Microsoft.Extensions.Logging;
using Strategyo.Mediator.Wrappers;
using Strategyo.Results.Contracts.Results;

namespace App.InvoiSysTest.Application.UseCases.Order.FindPaginableOrder;

public class FindPaginableOrderUseCaseCreateOrderUseCase(
    IOrderRepository orderRepository,
    ILogger<FindPaginableOrderUseCaseCreateOrderUseCase> logger) :
    RequestHandlerWrapperImpl<FindPaginableOrderInput, FindPaginableOrderOutput>
{
    public async Task<Result<FindPaginableOrderOutput>> HandleAsync(FindPaginableOrderInput request, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await orderRepository
                              .PaginableAsync<PaginableOutput>(request.PageNumber, request.PageSize, cancellationToken)
                              .ConfigureAwait(false);

            Response = result.Adapt<FindPaginableOrderOutput>();

            return Response;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Não foi possível realizar a busca paginada do pedido de compra, CorrelationId: {CorrelationId}", request.CorrelationId);
            return Errors.BadRequest("Não foi possível realizar a busca paginada do pedido de compra");
        }
    }
}