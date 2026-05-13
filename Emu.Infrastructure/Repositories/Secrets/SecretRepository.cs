using Application.Abstractions.Persistence;
using Domain.Secrets;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Secrets;

public sealed class SecretRepository : ISecretRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SecretRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Secret?> GetByIdAsync(Guid secretId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Secrets.FirstOrDefaultAsync(
            x => x.SecretId == secretId,
            cancellationToken
        );
    }

    public Task<Secret?> GetByPathAsync(
        Guid environmentId,
        string path,
        CancellationToken cancellationToken = default
    )
    {
        return _dbContext.Secrets.FirstOrDefaultAsync(
            x => x.EnvironmentId == environmentId && x.Path == path,
            cancellationToken
        );
    }

    public async Task<IReadOnlyList<Secret>> ListByEnvironmentAsync(
        Guid environmentId,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext
            .Secrets.Where(x => x.EnvironmentId == environmentId)
            .OrderBy(x => x.Path)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Secret secret, CancellationToken cancellationToken = default)
    {
        await _dbContext.Secrets.AddAsync(secret, cancellationToken);
    }

    public void Update(Secret secret)
    {
        _dbContext.Secrets.Update(secret);
    }
}
