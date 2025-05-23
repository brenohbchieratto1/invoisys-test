using App.InvoiSysTest.Application.UseCases.Order.FindOrderById.Input;
using App.InvoiSysTest.Application.UseCases.Order.FindOrderById.Output;
using App.InvoiSysTest.Domain.Interfaces;
using Mapster;
using Microsoft.Extensions.Logging;
using Strategyo.Mediator.Interfaces;
using Strategyo.Mediator.Wrappers;
using Strategyo.Results.Contracts.Results;

namespace App.InvoiSysTest.Application.UseCases.Order.FindOrderById;

public class FindOrderByIdUseCase(
    IOrderRepository  orderRepository,
    ILogger<FindOrderByIdUseCase> logger) : 
    IRequestHandler<FindOrderByIdInput, FindOrderByIdOutput>
{
    public async Task<Result<FindOrderByIdOutput>> HandleAsync(FindOrderByIdInput request, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await orderRepository.FindOneAsync(request.Id, cancellationToken).ConfigureAwait(false);

            if (entity.TryGetErrorsAndMessages(out var errors, out var messages, out var order))
            {
                logger.LogWarning("Não foi possível realizar a busca do pedido do compra, Erros: {@Errors}, Mensagens: {@Messages}, CorrelationId: {CorrelationId}",
                    errors, 
                    messages, 
                    request.CorrelationId);
                return (errors, messages);
            }

            var response = order.Adapt<FindOrderByIdOutput>();
            
            return response;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Não foi possível realizar a busca do pedido do compra, CorrelationId: {CorrelationId}", request.CorrelationId);
            return Errors.BadRequest("Não foi possível realizar a busca pedido do compra");
        }
    }
}