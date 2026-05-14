using Domain.AccessPolicies;

namespace Application.Abstractions.Persistence;

public interface IAccessPolicyRepository
{
    Task<AccessPolicy?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AccessPolicy>> ListByApiKeyAsync(
        Guid apiKeyId,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyList<AccessPolicy>> ListByTenantAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default
    );

    Task AddAsync(AccessPolicy policy, CancellationToken cancellationToken = default);

    void Update(AccessPolicy policy);
}
