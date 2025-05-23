using App.InvoiSysTest.Application.UseCases.Order.FindPaginableOrder.Output;
using Strategyo.Mediator.Entities.Base;

namespace App.InvoiSysTest.Application.UseCases.Order.FindPaginableOrder.Input;

public class FindPaginableOrderInput(int pageNumber, int pageSize) : BaseInput<FindPaginableOrderInput, FindPaginableOrderOutput>
{
    public int PageNumber { get; } = pageNumber;
    public int PageSize { get; } = pageSize;
}