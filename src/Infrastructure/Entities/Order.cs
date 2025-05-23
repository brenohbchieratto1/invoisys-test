using App.InvoiSysTest.Infrastructure.Attributes;
using App.InvoiSysTest.Infrastructure.Entities.Base;

namespace App.InvoiSysTest.Infrastructure.Entities;

[DatabasePath("Order")]
public class Order : BaseCollection
{
    public string OrderNumber { get; set; } = null!;
    public DateTime RequestDate { get; set; }
    public DateTime EstimatedDeliveryDate { get; set; }
    public string OrderNote { get; set; } = null!;
    public List<Product>? Products { get; set; }
}

public class Product
{
    public string ProductCode { get; set; } = null!;
    public int Quantity { get; set; }
    public string ProductDescription { get; set; } = null!;
    public decimal ProductPrice { get; set; }
}