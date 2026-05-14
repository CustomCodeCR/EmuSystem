using Application.Abstractions.Persistence;
using Domain.Environments;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Environments;

public sealed class ProjectEnvironmentRepository : IProjectEnvironmentRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProjectEnvironmentRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<ProjectEnvironment?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return _dbContext.ProjectEnvironments.FirstOrDefaultAsync(
            x => x.EnvironmentId == id,
            cancellationToken
        );
    }

    public Task<ProjectEnvironment?> GetBySlugAsync(
        Guid projectId,
        string slug,
        CancellationToken cancellationToken = default
    )
    {
        return _dbContext.ProjectEnvironments.FirstOrDefaultAsync(
            x => x.ProjectId == projectId && x.Slug == slug,
            cancellationToken
        );
    }

    public async Task<IReadOnlyList<ProjectEnvironment>> ListByProjectAsync(
        Guid projectId,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext
            .ProjectEnvironments.Where(x => x.ProjectId == projectId)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        ProjectEnvironment environment,
        CancellationToken cancellationToken = default
    )
    {
        await _dbContext.ProjectEnvironments.AddAsync(environment, cancellationToken);
    }

    public void Update(ProjectEnvironment environment)
    {
        _dbContext.ProjectEnvironments.Update(environment);
    }
}
