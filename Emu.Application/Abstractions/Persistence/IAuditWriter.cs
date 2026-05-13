using Domain.AuditLogs;

namespace Application.Abstractions.Persistence;

public interface IAuditWriter
{
    Task WriteAsync(
        Guid tenantId,
        ActorType actorType,
        Guid? actorId,
        string action,
        ResourceType resourceType,
        Guid? resourceId,
        string? path,
        string? ipAddress,
        string? userAgent,
        CancellationToken cancellationToken = default
    );
}
