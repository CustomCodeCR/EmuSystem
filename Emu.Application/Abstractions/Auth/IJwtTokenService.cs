namespace Application.Abstractions.Auth;

public interface IJwtTokenService
{
    string CreateToken(Guid userId, Guid tenantId, string email, IReadOnlyList<string> roles);
}
