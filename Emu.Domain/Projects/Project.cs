using Domain.Common;

namespace Domain.Projects;

public sealed class Project : IAuditableEntity
{
    public Guid ProjectId { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public Domain.Tenants.Tenant? Tenant { get; set; }
}
