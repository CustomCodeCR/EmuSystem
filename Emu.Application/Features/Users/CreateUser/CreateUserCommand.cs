namespace Application.Features.Users.CreateUser;

public sealed record CreateUserRequest(
    Guid TenantId,
    string Email,
    string FullName,
    string Password
);

public sealed record CreateUserResponse(Guid Id, Guid TenantId, string Email, string FullName);
