namespace App.InvoiSysTest.Application.UseCases.Order.CreateOrder.Output;

public class CreateOrderOutput
{
    public Ulid OrderId { get; set; }
    public Guid CorrelationId { get; set; }
}