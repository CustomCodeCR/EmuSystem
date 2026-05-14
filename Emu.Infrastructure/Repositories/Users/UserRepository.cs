using Application.Abstractions.Persistence;
using Domain.Users;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Users;

public sealed class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Users.FirstOrDefaultAsync(x => x.UserId == id, cancellationToken);
    }

    public Task<User?> GetByEmailAsync(
        Guid tenantId,
        string email,
        CancellationToken cancellationToken = default
    )
    {
        return _dbContext.Users.FirstOrDefaultAsync(
            x => x.TenantId == tenantId && x.Email == email,
            cancellationToken
        );
    }

    public async Task<IReadOnlyList<User>> ListByTenantAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext
            .Users.Where(x => x.TenantId == tenantId)
            .OrderBy(x => x.FullName)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
    }

    public void Update(User user)
    {
        _dbContext.Users.Update(user);
    }
}
