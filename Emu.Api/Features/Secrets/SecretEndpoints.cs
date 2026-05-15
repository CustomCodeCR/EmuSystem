using Application.Features.Secrets.CreateSecret;
using Application.Features.Secrets.DeleteSecret;
using Application.Features.Secrets.GetSecret;
using Application.Features.Secrets.ListSecrets;
using Application.Features.Secrets.RotateSecret;

namespace Api.Features.Secrets;

public static class SecretEndpoints
{
    public static IEndpointRouteBuilder MapSecretEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/secrets").RequireAuthorization().RequireRateLimiting("api");

        group.MapPost(
            "/",
            async (
                CreateSecretRequest request,
                CreateSecretHandler handler,
                CancellationToken cancellationToken
            ) =>
            {
                var response = await handler.HandleAsync(request, cancellationToken);
                return Results.Created($"/api/secrets/{response.Id}", response);
            }
        );

        group.MapGet(
            "/",
            async (
                Guid environmentId,
                ListSecretsHandler handler,
                CancellationToken cancellationToken
            ) =>
            {
                var response = await handler.HandleAsync(environmentId, cancellationToken);
                return Results.Ok(response);
            }
        );

        group
            .MapGet(
                "/by-path",
                async (
                    Guid environmentId,
                    string path,
                    GetSecretByPathHandler handler,
                    CancellationToken cancellationToken
                ) =>
                {
                    var response = await handler.HandleAsync(
                        new GetSecretByPathRequest(environmentId, path),
                        cancellationToken
                    );

                    return Results.Ok(response);
                }
            )
            .RequireRateLimiting("secrets-read");

        group.MapPost(
            "/{id:guid}/rotate",
            async (
                Guid id,
                RotateSecretRequest request,
                RotateSecretHandler handler,
                CancellationToken cancellationToken
            ) =>
            {
                var response = await handler.HandleAsync(id, request, cancellationToken);
                return Results.Ok(response);
            }
        );

        group.MapDelete(
            "/{id:guid}",
            async (Guid id, DeleteSecretHandler handler, CancellationToken cancellationToken) =>
            {
                var response = await handler.HandleAsync(id, cancellationToken);
                return Results.Ok(response);
            }
        );

        return app;
    }
}
