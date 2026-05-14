namespace Domain.Users;

public sealed class User
{
    public Guid UserId { get; set; }

    public Guid TenantId { get; set; }

    public string Email { get; set; } = default!;

    public string FullName { get; set; } = default!;

    public string PasswordHash { get; set; } = default!;

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? LastLoginAt { get; set; }

    public Domain.Tenants.Tenant? Tenant { get; set; }
}
