using Domain.Secrets;

namespace Application.Abstractions.Persistence;

public interface ISecretVersionRepository
{
    Task<SecretVersion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<SecretVersion?> GetCurrentBySecretIdAsync(
        Guid secretId,
        int currentVersionNumber,
        CancellationToken cancellationToken = default
    );

    Task<SecretVersion?> GetByVersionNumberAsync(
        Guid secretId,
        int versionNumber,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyList<SecretVersion>> ListBySecretIdAsync(
        Guid secretId,
        CancellationToken cancellationToken = default
    );

    Task AddAsync(SecretVersion secretVersion, CancellationToken cancellationToken = default);
}
