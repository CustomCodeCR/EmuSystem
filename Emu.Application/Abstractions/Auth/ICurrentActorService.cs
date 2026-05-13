using Domain.AuditLogs;

namespace Application.Abstractions.Auth;

public interface ICurrentActorService
{
    ActorType ActorType { get; }

    Guid? ActorId { get; }

    Guid? TenantId { get; }

    string? IpAddress { get; }

    string? UserAgent { get; }

    bool IsAuthenticated { get; }
}
