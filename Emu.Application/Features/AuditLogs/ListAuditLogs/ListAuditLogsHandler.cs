using Application.Abstractions.Persistence;

namespace Application.Features.AuditLogs.ListAuditLogs;

public sealed class ListAuditLogsHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public ListAuditLogsHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<AuditLogListItemResponse>> HandleAsync(
        Guid tenantId,
        int page = 1,
        int pageSize = 50,
        CancellationToken cancellationToken = default
    )
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 200);

        var logs = await _unitOfWork.AuditLogs.ListByTenantAsync(
            tenantId,
            page,
            pageSize,
            cancellationToken
        );

        return logs.Select(x => new AuditLogListItemResponse(
                x.AuditLogId,
                x.TenantId,
                x.ActorType.ToString(),
                x.ActorId,
                x.Action,
                x.ResourceType.ToString(),
                x.ResourceId,
                x.Path,
                x.IpAddress,
                x.UserAgent,
                x.CreatedAt
            ))
            .ToList();
    }
}
