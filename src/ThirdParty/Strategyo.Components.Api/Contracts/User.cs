namespace Strategyo.Components.Api.Contracts;

public class User
{
    public Ulid Id { get; set; }
    public Ulid TenantId { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? AvatarUrl { get; set; }
    public bool Enabled { get; set; }
    public List<string>? Roles { get; set; }
    public List<string>? Permissions { get; set; }
}