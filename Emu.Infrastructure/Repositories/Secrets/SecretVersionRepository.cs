using Application.Abstractions.Persistence;
using Domain.Secrets;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Secrets;

public sealed class SecretVersionRepository : ISecretVersionRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SecretVersionRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<SecretVersion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.SecretVersions.FirstOrDefaultAsync(
            x => x.SecretVersionId == id,
            cancellationToken
        );
    }

    public Task<SecretVersion?> GetCurrentBySecretIdAsync(
        Guid secretId,
        int currentVersionNumber,
        CancellationToken cancellationToken = default
    )
    {
        return _dbContext.SecretVersions.FirstOrDefaultAsync(
            x => x.SecretId == secretId && x.VersionNumber == currentVersionNumber,
            cancellationToken
        );
    }

    public Task<SecretVersion?> GetByVersionNumberAsync(
        Guid secretId,
        int versionNumber,
        CancellationToken cancellationToken = default
    )
    {
        return _dbContext.SecretVersions.FirstOrDefaultAsync(
            x => x.SecretId == secretId && x.VersionNumber == versionNumber,
            cancellationToken
        );
    }

    public async Task<IReadOnlyList<SecretVersion>> ListBySecretIdAsync(
        Guid secretId,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext
            .SecretVersions.Where(x => x.SecretId == secretId)
            .OrderByDescending(x => x.VersionNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        SecretVersion secretVersion,
        CancellationToken cancellationToken = default
    )
    {
        await _dbContext.SecretVersions.AddAsync(secretVersion, cancellationToken);
    }
}
