using App.InvoisysTest.Application.UseCases.Order.CreateOrder.Output;
using Strategyo.Mediator.Entities.Base;

namespace App.InvoisysTest.Application.UseCases.Order.CreateOrder.Input;

public class CreateOrderInput : BaseInput<CreateOrderInput, CreateOrderOutput>
{
    public string OrderNumber { get; set; } = null!;
    public DateTime RequestDate { get; set; }
    public DateTime EstimatedDeliveryDate { get; set; }
    public string OrderNote { get; set; } = null!;
    public List<ProductInput>? Products { get; set; }
}

public class ProductInput
{
    public string ProductCode { get; set; } = null!;
    public int Quantity { get; set; }
    public string ProductDescription { get; set; } = null!;
    public decimal ProductPrice { get; set; }
}