using Application.Features.Environments.CreateEnvironment;
using Application.Features.Environments.ListEnvironments;

namespace Api.Features.Environments;

public static class EnvironmentEndpoints
{
    public static IEndpointRouteBuilder MapEnvironmentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/environments")
            .RequireAuthorization()
            .RequireRateLimiting("api");

        group.MapPost(
            "/",
            async (
                CreateEnvironmentRequest request,
                CreateEnvironmentHandler handler,
                CancellationToken cancellationToken
            ) =>
            {
                var response = await handler.HandleAsync(request, cancellationToken);
                return Results.Created($"/api/environments/{response.Id}", response);
            }
        );

        group.MapGet(
            "/",
            async (
                Guid projectId,
                ListEnvironmentsHandler handler,
                CancellationToken cancellationToken
            ) =>
            {
                var response = await handler.HandleAsync(projectId, cancellationToken);
                return Results.Ok(response);
            }
        );

        return app;
    }
}
