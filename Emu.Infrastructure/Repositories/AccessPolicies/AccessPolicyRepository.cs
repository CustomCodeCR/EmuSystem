using Application.Abstractions.Persistence;
using Domain.AccessPolicies;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.AccessPolicies;

public sealed class AccessPolicyRepository : IAccessPolicyRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AccessPolicyRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<AccessPolicy?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.AccessPolicies.FirstOrDefaultAsync(
            x => x.AccessPolicyId == id,
            cancellationToken
        );
    }

    public async Task<IReadOnlyList<AccessPolicy>> ListByApiKeyAsync(
        Guid apiKeyId,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext
            .AccessPolicies.Where(x => x.ApiKeyId == apiKeyId)
            .OrderBy(x => x.PathPrefix)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AccessPolicy>> ListByTenantAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext
            .AccessPolicies.Where(x => x.TenantId == tenantId)
            .OrderBy(x => x.PathPrefix)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(AccessPolicy policy, CancellationToken cancellationToken = default)
    {
        await _dbContext.AccessPolicies.AddAsync(policy, cancellationToken);
    }

    public void Update(AccessPolicy policy)
    {
        _dbContext.AccessPolicies.Update(policy);
    }
}
