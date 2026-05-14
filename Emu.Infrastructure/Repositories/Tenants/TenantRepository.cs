using Application.Abstractions.Persistence;
using Domain.Tenants;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Tenants;

public sealed class TenantRepository : ITenantRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TenantRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Tenants.FirstOrDefaultAsync(x => x.TenantId == id, cancellationToken);
    }

    public Task<Tenant?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return _dbContext.Tenants.FirstOrDefaultAsync(x => x.Slug == slug, cancellationToken);
    }

    public async Task<IReadOnlyList<Tenant>> ListAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext.Tenants.OrderBy(x => x.Name).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        await _dbContext.Tenants.AddAsync(tenant, cancellationToken);
    }

    public void Update(Tenant tenant)
    {
        _dbContext.Tenants.Update(tenant);
    }
}
