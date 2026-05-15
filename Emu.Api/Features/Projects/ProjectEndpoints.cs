using Application.Features.Projects.CreateProject;
using Application.Features.Projects.ListProjects;

namespace Api.Features.Projects;

public static class ProjectEndpoints
{
    public static IEndpointRouteBuilder MapProjectEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/projects").RequireAuthorization().RequireRateLimiting("api");

        group.MapPost(
            "/",
            async (
                CreateProjectRequest request,
                CreateProjectHandler handler,
                CancellationToken cancellationToken
            ) =>
            {
                var response = await handler.HandleAsync(request, cancellationToken);
                return Results.Created($"/api/projects/{response.Id}", response);
            }
        );

        group.MapGet(
            "/",
            async (
                Guid tenantId,
                ListProjectsHandler handler,
                CancellationToken cancellationToken
            ) =>
            {
                var response = await handler.HandleAsync(tenantId, cancellationToken);
                return Results.Ok(response);
            }
        );

        return app;
    }
}
