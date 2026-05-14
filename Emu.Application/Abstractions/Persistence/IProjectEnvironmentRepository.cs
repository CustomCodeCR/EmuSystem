using Domain.Environments;

namespace Application.Abstractions.Persistence;

public interface IProjectEnvironmentRepository
{
    Task<ProjectEnvironment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ProjectEnvironment?> GetBySlugAsync(
        Guid projectId,
        string slug,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyList<ProjectEnvironment>> ListByProjectAsync(
        Guid projectId,
        CancellationToken cancellationToken = default
    );

    Task AddAsync(ProjectEnvironment environment, CancellationToken cancellationToken = default);

    void Update(ProjectEnvironment environment);
}
