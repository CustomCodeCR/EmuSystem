using Domain.Projects;

namespace Application.Abstractions.Persistence;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Project?> GetBySlugAsync(
        Guid tenantId,
        string slug,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyList<Project>> ListByTenantAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default
    );

    Task AddAsync(Project project, CancellationToken cancellationToken = default);

    void Update(Project project);
}
