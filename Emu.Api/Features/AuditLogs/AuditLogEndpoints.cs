using Application.Features.AuditLogs.ListAuditLogs;

namespace Api.Features.AuditLogs;

public static class AuditLogEndpoints
{
    public static IEndpointRouteBuilder MapAuditLogEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/audit-logs")
            .RequireAuthorization()
            .RequireRateLimiting("api");

        group.MapGet(
            "/",
            async (
                Guid tenantId,
                int? page,
                int? pageSize,
                ListAuditLogsHandler handler,
                CancellationToken cancellationToken
            ) =>
            {
                var response = await handler.HandleAsync(
                    tenantId,
                    page ?? 1,
                    pageSize ?? 50,
                    cancellationToken
                );

                return Results.Ok(response);
            }
        );

        return app;
    }
}
