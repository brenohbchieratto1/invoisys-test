using App.InvoiSysTest.Application.Extensions;
using App.InvoiSysTest.Application.UseCases.Order.CreateOrder.Input;
using App.InvoiSysTest.Application.UseCases.Order.CreateOrder.Output;
using App.InvoiSysTest.Domain.Interfaces;
using FluentValidation;
using Mapster;
using Microsoft.Extensions.Logging;
using Strategyo.Mediator.Interfaces;
using Strategyo.Mediator.Wrappers;
using Strategyo.Results.Contracts.Results;

namespace App.InvoiSysTest.Application.UseCases.Order.CreateOrder;

public class CreateOrderUseCase(
    IOrderRepository orderRepository,
    IValidator<CreateOrderInput> inputValidator,
    ILogger<CreateOrderUseCase> logger) :
    IRequestHandler<CreateOrderInput, CreateOrderOutput>
{
    public async Task<Result<CreateOrderOutput>> HandleAsync(CreateOrderInput request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validator = await inputValidator.ValidateAsync(request, cancellationToken).ConfigureAwait(false);

            if (!validator.IsValid)
            {
                return validator.Errors.HandleErrors();
            }

            var entity = request.Adapt<Domain.Entities.Order>();
            entity.SetCreatedAt(request.LogUser);

            await orderRepository.AddAsync(entity, cancellationToken).ConfigureAwait(false);

            var response = new CreateOrderOutput
            {
                CorrelationId = request.CorrelationId,
                OrderId = entity.Id,
            };

            return response;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Não foi possível criar um pedido de compra, CorrelationId: {CorrelationId}", request.CorrelationId);
            return Errors.BadRequest("Não foi possível criar um pedido de compra");
        }
    }
}