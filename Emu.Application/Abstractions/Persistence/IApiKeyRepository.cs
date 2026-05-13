using Domain.ApiKeys;

namespace Application.Abstractions.Persistence;

public interface IApiKeyRepository
{
    Task<ApiKey?> GetByIdAsync(Guid apiKeyId, CancellationToken cancellationToken = default);

    Task<ApiKey?> GetByPrefixAsync(string keyPrefix, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ApiKey>> ListByTenantAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default
    );

    Task AddAsync(ApiKey apiKey, CancellationToken cancellationToken = default);

    void Update(ApiKey apiKey);

    Task UpdateLastUsedAtAsync(
        Guid apiKeyId,
        DateTimeOffset usedAt,
        CancellationToken cancellationToken = default
    );
}
