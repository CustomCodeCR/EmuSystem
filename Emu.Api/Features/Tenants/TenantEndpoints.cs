using Application.Features.Tenants.CreateTenant;
using Application.Features.Tenants.GetTenant;
using Application.Features.Tenants.ListTenants;

namespace Api.Features.Tenants;

public static class TenantEndpoints
{
    public static IEndpointRouteBuilder MapTenantEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tenants").RequireAuthorization().RequireRateLimiting("api");

        group.MapPost(
            "/",
            async (
                CreateTenantRequest request,
                CreateTenantHandler handler,
                CancellationToken cancellationToken
            ) =>
            {
                var response = await handler.HandleAsync(request, cancellationToken);
                return Results.Created($"/api/tenants/{response.Id}", response);
            }
        );

        group.MapGet(
            "/{id:guid}",
            async (Guid id, GetTenantHandler handler, CancellationToken cancellationToken) =>
            {
                var response = await handler.HandleAsync(id, cancellationToken);
                return Results.Ok(response);
            }
        );

        group.MapGet(
            "/",
            async (ListTenantsHandler handler, CancellationToken cancellationToken) =>
            {
                var response = await handler.HandleAsync(cancellationToken);
                return Results.Ok(response);
            }
        );

        return app;
    }
}
