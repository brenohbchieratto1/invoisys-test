using App.InvoisysTest.Application.UseCases.Order.CreateOrder.Input;
using App.InvoisysTest.Application.UseCases.Order.CreateOrder.Output;
using Strategyo.Mediator.Interfaces;
using Strategyo.Results.Contracts.Results;

namespace App.InvoisysTest.Application.UseCases.Order.CreateOrder;

public class CreateOrderUseCase() : 
    IRequestHandler<CreateOrderInput, CreateOrderOutput>
{
    public Task<Result<CreateOrderOutput>> HandleAsync(CreateOrderInput request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}