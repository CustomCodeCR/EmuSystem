using Domain.Secrets;

namespace Application.Abstractions.Persistence;

public interface ISecretRepository
{
    Task<Secret?> GetByIdAsync(Guid secretId, CancellationToken cancellationToken = default);
    Task<Secret?> GetByPathAsync(
        Guid environmentId,
        string path,
        CancellationToken cancellationToken = default
    );
    Task<IReadOnlyList<Secret>> ListByEnvironmentAsync(
        Guid environmentId,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(Secret secret, CancellationToken cancellationToken = default);
    void Update(Secret secret);
}
