namespace Application.Features.Users.Login;

public sealed record LoginRequest(Guid TenantId, string Email, string Password);

public sealed record LoginResponse(string AccessToken, Guid UserId, Guid TenantId, string Email);
