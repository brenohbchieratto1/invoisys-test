namespace App.InvoisysTest.Application.UseCases.Order.FindOrderById.Output;

public class FindOrderByIdOutput
{
    public Ulid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public string? LogUser { get; set; }
    
    public string OrderNumber { get; set; } = null!;
    public DateTime RequestDate { get; set; }
    public DateTime EstimatedDeliveryDate { get; set; }
    public string OrderNote { get; set; } = null!;
    public List<ProductOutput>? Products { get; set; }
}

public class ProductOutput
{
    public string ProductCode { get; set; } = null!;
    public int Quantity { get; set; }
    public string ProductDescription { get; set; } = null!;
    public decimal ProductPrice { get; set; }
}