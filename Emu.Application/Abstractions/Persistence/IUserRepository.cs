using Domain.Users;

namespace Application.Abstractions.Persistence;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<User?> GetByEmailAsync(
        Guid tenantId,
        string email,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyList<User>> ListByTenantAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default
    );

    Task AddAsync(User user, CancellationToken cancellationToken = default);

    void Update(User user);
}
