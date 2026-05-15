using Application.Features.ApiKeys.CreateApiKey;
using Application.Features.ApiKeys.DisableApiKey;
using Application.Features.ApiKeys.ListApiKeys;

namespace Api.Features.ApiKeys;

public static class ApiKeyEndpoints
{
    public static IEndpointRouteBuilder MapApiKeyEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/api-keys").RequireAuthorization().RequireRateLimiting("api");

        group.MapPost(
            "/",
            async (
                CreateApiKeyRequest request,
                CreateApiKeyHandler handler,
                CancellationToken cancellationToken
            ) =>
            {
                var response = await handler.HandleAsync(request, cancellationToken);
                return Results.Created($"/api/api-keys/{response.Id}", response);
            }
        );

        group.MapGet(
            "/",
            async (
                Guid tenantId,
                ListApiKeysHandler handler,
                CancellationToken cancellationToken
            ) =>
            {
                var response = await handler.HandleAsync(tenantId, cancellationToken);
                return Results.Ok(response);
            }
        );

        group.MapPost(
            "/{id:guid}/disable",
            async (Guid id, DisableApiKeyHandler handler, CancellationToken cancellationToken) =>
            {
                var response = await handler.HandleAsync(id, cancellationToken);
                return Results.Ok(response);
            }
        );

        return app;
    }
}
