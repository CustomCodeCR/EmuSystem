using Domain.Common;

namespace Domain.Environments;

public sealed class ProjectEnvironment : IAuditableEntity
{
    public Guid EnvironmentId { get; set; }
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public Domain.Projects.Project? Project { get; set; }
}
