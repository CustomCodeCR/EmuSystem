using Application.Features.Users.CreateUser;
using Application.Features.Users.ListUsers;

namespace Api.Features.Users;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users").RequireAuthorization().RequireRateLimiting("api");

        group.MapPost(
            "/",
            async (
                CreateUserRequest request,
                CreateUserHandler handler,
                CancellationToken cancellationToken
            ) =>
            {
                var response = await handler.HandleAsync(request, cancellationToken);
                return Results.Created($"/api/users/{response.Id}", response);
            }
        );

        group.MapGet(
            "/",
            async (Guid tenantId, ListUsersHandler handler, CancellationToken cancellationToken) =>
            {
                var response = await handler.HandleAsync(tenantId, cancellationToken);
                return Results.Ok(response);
            }
        );

        return app;
    }
}
