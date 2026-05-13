using Application.Abstractions.Auth;
using Domain.AuditLogs;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Auth;

public sealed class CurrentActorService : ICurrentActorService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentActorService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public ActorType ActorType
    {
        get
        {
            var value = GetClaim("actor_type");

            return value switch
            {
                "api_key" => ActorType.ApiKey,
                "user" => ActorType.User,
                _ => ActorType.System,
            };
        }
    }

    public Guid? ActorId => GetGuidClaim("actor_id");

    public Guid? TenantId => GetGuidClaim("tenant_id");

    public string? IpAddress =>
        _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

    public string? UserAgent =>
        _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString();

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;

    private string? GetClaim(string type)
    {
        return _httpContextAccessor
            .HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == type)
            ?.Value;
    }

    private Guid? GetGuidClaim(string type)
    {
        var value = GetClaim(type);

        return Guid.TryParse(value, out var id) ? id : null;
    }
}
