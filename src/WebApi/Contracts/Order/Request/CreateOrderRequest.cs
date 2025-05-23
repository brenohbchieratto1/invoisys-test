namespace App.InvoiSysTest.WebApi.Contracts.Order.Request;

public class CreateOrderRequest
{
    public string OrderNumber { get; set; } = null!;
    public DateTime RequestDate { get; set; }
    public DateTime EstimatedDeliveryDate { get; set; }
    public string OrderNote { get; set; } = null!;
    public List<ProductRequest>? Products { get; set; }
}

public class ProductRequest
{
    public string ProductCode { get; set; } = null!;
    public int Quantity { get; set; }
    public string ProductDescription { get; set; } = null!;
    public decimal ProductPrice { get; set; }
}