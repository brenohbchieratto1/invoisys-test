namespace App.InvoiSysTest.Infrastructure.Entities.Base;

public class BaseCollection
{
    public Ulid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public string? LogUser { get; set; }
}