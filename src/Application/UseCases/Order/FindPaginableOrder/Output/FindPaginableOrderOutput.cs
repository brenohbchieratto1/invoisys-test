using Strategyo.Results.Contracts.Paginable;

namespace App.InvoisysTest.Application.UseCases.Order.FindPaginableOrder.Output;

public class FindPaginableOrderOutput : PaginableResult<PaginableOutput>;

public class PaginableOutput
{
    public Ulid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string OrderNumber { get; set; } = null!;
    public DateTime RequestDate { get; set; }
    public DateTime EstimatedDeliveryDate { get; set; }
    public string OrderNote { get; set; } = null!;
}