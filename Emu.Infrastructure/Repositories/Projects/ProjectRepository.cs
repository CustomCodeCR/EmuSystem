using Application.Abstractions.Persistence;
using Domain.Projects;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Projects;

public sealed class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProjectRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Projects.FirstOrDefaultAsync(x => x.ProjectId == id, cancellationToken);
    }

    public Task<Project?> GetBySlugAsync(
        Guid tenantId,
        string slug,
        CancellationToken cancellationToken = default
    )
    {
        return _dbContext.Projects.FirstOrDefaultAsync(
            x => x.TenantId == tenantId && x.Slug == slug,
            cancellationToken
        );
    }

    public async Task<IReadOnlyList<Project>> ListByTenantAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext
            .Projects.Where(x => x.TenantId == tenantId)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Project project, CancellationToken cancellationToken = default)
    {
        await _dbContext.Projects.AddAsync(project, cancellationToken);
    }

    public void Update(Project project)
    {
        _dbContext.Projects.Update(project);
    }
}
