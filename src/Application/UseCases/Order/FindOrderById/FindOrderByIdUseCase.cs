using App.InvoisysTest.Application.UseCases.Order.FindOrderById.Input;
using App.InvoisysTest.Application.UseCases.Order.FindOrderById.Output;
using App.InvoisysTest.Domain.Interfaces;
using Mapster;
using Microsoft.Extensions.Logging;
using Strategyo.Mediator.Wrappers;
using Strategyo.Results.Contracts.Results;

namespace App.InvoisysTest.Application.UseCases.Order.FindOrderById;

public class FindOrderByIdUseCase(
    IOrderRepository  orderRepository,
    ILogger<FindOrderByIdUseCase> logger) : 
    RequestHandlerWrapperImpl<FindOrderByIdInput, FindOrderByIdOutput>
{
    public async Task<Result<FindOrderByIdOutput>> HandleAsync(FindOrderByIdInput request, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await orderRepository.FindOneAsync(request.Id, cancellationToken);

            if (entity.TryGetErrorsAndMessages(out var errors, out var messages, out var order))
            {
                logger.LogWarning("Não foi possível realizar a busca do pedido do compra, Erros: {@Errors}, Mensagens: {@Messages}, CorrelationId: {CorrelationId}",
                    errors, 
                    messages, 
                    request.CorrelationId);
                return (errors, messages);
            }

            Response = order.Adapt<FindOrderByIdOutput>();
            
            return Response;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Não foi possível realizar a busca do pedido do compra, CorrelationId: {CorrelationId}", request.CorrelationId);
            return Errors.BadRequest("Não foi possível realizar a busca pedido do compra");
        }
    }
}