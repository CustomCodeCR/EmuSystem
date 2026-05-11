using Domain.Common;

namespace Domain.Secrets;

public sealed class Secret : IAuditableEntity
{
    public Guid SecretId { get; set; }
    public Guid EnvironmentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public int CurrentVersionNumber { get; set; }
    public SecretStatus Status { get; set; } = SecretStatus.Active;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public Domain.Environments.ProjectEnvironment? Environment { get; set; }
}
