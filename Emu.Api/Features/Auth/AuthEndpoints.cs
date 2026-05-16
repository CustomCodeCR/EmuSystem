using Application.Features.Users.Login;

namespace Api.Features.Auth;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").RequireRateLimiting("auth");

        group.MapPost(
            "/login",
            async (
                LoginRequest request,
                LoginHandler handler,
                CancellationToken cancellationToken
            ) =>
            {
                var response = await handler.HandleAsync(request, cancellationToken);
                return Results.Ok(response);
            }
        );

        return app;
    }
}
