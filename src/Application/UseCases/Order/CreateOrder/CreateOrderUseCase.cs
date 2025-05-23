using App.InvoisysTest.Application.Extensions;
using App.InvoisysTest.Application.UseCases.Order.CreateOrder.Input;
using App.InvoisysTest.Application.UseCases.Order.CreateOrder.Output;
using App.InvoisysTest.Domain.Interfaces;
using FluentValidation;
using Mapster;
using Microsoft.Extensions.Logging;
using Strategyo.Mediator.Wrappers;
using Strategyo.Results.Contracts.Results;

namespace App.InvoisysTest.Application.UseCases.Order.CreateOrder;

public class CreateOrderUseCase(
    IOrderRepository orderRepository,
    IValidator<CreateOrderInput> inputValidator,
    ILogger<CreateOrderUseCase> logger) :
    RequestHandlerWrapperImpl<CreateOrderInput, CreateOrderOutput>
{
    public async Task<Result<CreateOrderOutput>> HandleAsync(CreateOrderInput request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validator = await inputValidator.ValidateAsync(request, cancellationToken);

            if (!validator.IsValid)
            {
                return validator.Errors.HandleErrors();
            }

            var entity = request.Adapt<Domain.Entities.Order>();
            entity.SetCreatedAt(request.LogUser);

            await orderRepository.AddAsync(entity, cancellationToken);

            Response.CorrelationId = request.CorrelationId;
            Response.OrderId = entity.Id;

            return Response;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Não foi possível criar um pedido de compra, CorrelationId: {CorrelationId}", request.CorrelationId);
            return Errors.BadRequest("Não foi possível criar um pedido de compra");
        }
    }
}