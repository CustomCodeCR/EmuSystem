using Application.Abstractions.Persistence;
using Domain.ApiKeys;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ApiKeys;

public sealed class ApiKeyRepository : IApiKeyRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ApiKeyRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<ApiKey?> GetByIdAsync(Guid apiKeyId, CancellationToken cancellationToken = default)
    {
        return _dbContext.ApiKeys.FirstOrDefaultAsync(
            x => x.ApiKeyId == apiKeyId,
            cancellationToken
        );
    }

    public Task<ApiKey?> GetByPrefixAsync(
        string keyPrefix,
        CancellationToken cancellationToken = default
    )
    {
        return _dbContext.ApiKeys.FirstOrDefaultAsync(
            x => x.KeyPrefix == keyPrefix,
            cancellationToken
        );
    }

    public async Task<IReadOnlyList<ApiKey>> ListByTenantAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext
            .ApiKeys.Where(x => x.TenantId == tenantId)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ApiKey apiKey, CancellationToken cancellationToken = default)
    {
        await _dbContext.ApiKeys.AddAsync(apiKey, cancellationToken);
    }

    public void Update(ApiKey apiKey)
    {
        _dbContext.ApiKeys.Update(apiKey);
    }

    public async Task UpdateLastUsedAtAsync(
        Guid apiKeyId,
        DateTimeOffset usedAt,
        CancellationToken cancellationToken = default
    )
    {
        var apiKey = await _dbContext.ApiKeys.FirstOrDefaultAsync(
            x => x.ApiKeyId == apiKeyId,
            cancellationToken
        );

        if (apiKey is null)
            return;

        apiKey.LastUsedAt = usedAt;
    }
}
