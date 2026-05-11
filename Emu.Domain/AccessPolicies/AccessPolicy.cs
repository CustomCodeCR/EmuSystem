using Domain.Common;

namespace Domain.AccessPolicies;

public sealed class AccessPolicy : IAuditableEntity
{
    public Guid AccessPolicyId { get; set; }
    public Guid ApiKeyId { get; set; }
    public Guid TenantId { get; set; }
    public Guid? ProjectId { get; set; }
    public Guid? EnvironmentId { get; set; }
    public string PathPrefix { get; set; } = string.Empty;
    public bool CanRead { get; set; }
    public bool CanWrite { get; set; }
    public bool CanDelete { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public Domain.ApiKeys.ApiKey? ApiKey { get; set; }
    public Domain.Tenants.Tenant? Tenant { get; set; }
    public Domain.Projects.Project? Project { get; set; }
    public Domain.Environments.ProjectEnvironment? Environment { get; set; }
}
