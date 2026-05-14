using Domain.AuditLogs;

namespace Application.Abstractions.Persistence;

public interface IAuditLogRepository
{
    Task<AuditLog?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AuditLog>> ListByTenantAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default
    );

    Task AddAsync(AuditLog auditLog, CancellationToken cancellationToken = default);
}
