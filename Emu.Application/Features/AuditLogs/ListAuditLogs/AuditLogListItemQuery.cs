namespace Application.Features.AuditLogs.ListAuditLogs;

public sealed record AuditLogListItemResponse(
    Guid Id,
    Guid TenantId,
    string ActorType,
    Guid? ActorId,
    string Action,
    string ResourceType,
    Guid? ResourceId,
    string? Path,
    string? IpAddress,
    string? UserAgent,
    DateTimeOffset CreatedAt
);
