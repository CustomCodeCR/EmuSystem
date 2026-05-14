using Application.Abstractions.Persistence;
using Domain.AuditLogs;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.AuditLogs;

public sealed class AuditLogRepository : IAuditLogRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AuditLogRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<AuditLog?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.AuditLogs.FirstOrDefaultAsync(x => x.AuditLogId == id, cancellationToken);
    }

    public async Task<IReadOnlyList<AuditLog>> ListByTenantAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbContext
            .AuditLogs.Where(x => x.TenantId == tenantId)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(AuditLog auditLog, CancellationToken cancellationToken = default)
    {
        await _dbContext.AuditLogs.AddAsync(auditLog, cancellationToken);
    }
}
