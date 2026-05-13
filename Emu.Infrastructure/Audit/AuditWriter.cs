using Application.Abstractions.Persistence;
using Application.Abstractions.Time;
using Domain.AuditLogs;
using Infrastructure.Persistence;

namespace Infrastructure.Audit;

public sealed class AuditWriter : IAuditWriter
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISystemClock _clock;

    public AuditWriter(ApplicationDbContext dbContext, ISystemClock clock)
    {
        _dbContext = dbContext;
        _clock = clock;
    }

    public async Task WriteAsync(
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
    )
    {
        var auditLog = new AuditLog
        {
            AuditLogId = Guid.NewGuid(),
            TenantId = tenantId,
            ActorType = actorType,
            ActorId = actorId,
            Action = action,
            ResourceType = resourceType,
            ResourceId = resourceId,
            Path = path,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            CreatedAt = _clock.UtcNow,
        };

        await _dbContext.AuditLogs.AddAsync(auditLog, cancellationToken);
    }
}
