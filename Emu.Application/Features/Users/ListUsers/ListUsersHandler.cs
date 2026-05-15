using Application.Abstractions.Persistence;

namespace Application.Features.Users.ListUsers;

public sealed class ListUsersHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public ListUsersHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<UserListItemResponse>> HandleAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default
    )
    {
        var users = await _unitOfWork.Users.ListByTenantAsync(tenantId, cancellationToken);

        return users
            .Select(x => new UserListItemResponse(
                x.UserId,
                x.TenantId,
                x.Email,
                x.FullName,
                x.IsActive,
                x.CreatedAt,
                x.LastLoginAt
            ))
            .ToList();
    }
}
