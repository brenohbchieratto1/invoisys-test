namespace App.InvoiSysTest.Domain.Entities.Base;

public class BaseEntity
{
    public Ulid Id { get; set; } = Ulid.NewUlid();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public string? LogUser { get; set; }

    public void SetCreatedAt(string? logUserCreatedAt)
    {
        CreatedAt = DateTimeOffset.UtcNow;
        SetUpdatedAt(CreatedAt, logUserCreatedAt);
    }

    public void SetUpdatedAt(string logUpdatedAt)
        => SetUpdatedAt(DateTimeOffset.UtcNow, logUpdatedAt);

    public void SetUpdatedAt(DateTimeOffset updatedAt, string? logUpdatedAt)
    {
        UpdatedAt = updatedAt;
        LogUser =  logUpdatedAt;
    }
}