namespace App.InvoisysTest.Application.UseCases.Order.CreateOrder.Output;

public class CreateOrderOutput
{
    public Ulid OrderId { get; set; }
    public Ulid CorrelationId { get; set; }
}