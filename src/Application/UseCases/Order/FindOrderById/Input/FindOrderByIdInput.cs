using App.InvoisysTest.Application.UseCases.Order.FindOrderById.Output;
using Strategyo.Mediator.Entities.Base;

namespace App.InvoisysTest.Application.UseCases.Order.FindOrderById.Input;

public class FindOrderByIdInput(string id) : BaseInput<FindOrderByIdInput, FindOrderByIdOutput>
{
    public Ulid Id { get; } = Ulid.Parse(id);
}