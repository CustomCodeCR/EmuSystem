namespace Application.Features.Users.ListUsers;

public sealed record UserListItemResponse(
    Guid Id,
    Guid TenantId,
    string Email,
    string FullName,
    bool IsActive,
    DateTimeOffset CreatedAt,
    DateTimeOffset? LastLoginAt
);
