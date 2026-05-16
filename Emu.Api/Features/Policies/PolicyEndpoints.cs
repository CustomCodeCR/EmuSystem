using Application.Features.Policies.CreateAccessPolicy;
using Application.Features.Policies.ListAccessPolicies;

namespace Api.Features.Policies;

public static class PolicyEndpoints
{
    public static IEndpointRouteBuilder MapPolicyEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/policies").RequireAuthorization().RequireRateLimiting("api");

        group.MapPost(
            "/",
            async (
                CreateAccessPolicyRequest request,
                CreateAccessPolicyHandler handler,
                CancellationToken cancellationToken
            ) =>
            {
                var response = await handler.HandleAsync(request, cancellationToken);
                return Results.Created($"/api/policies/{response.Id}", response);
            }
        );

        group.MapGet(
            "/by-api-key/{apiKeyId:guid}",
            async (
                Guid apiKeyId,
                ListAccessPoliciesHandler handler,
                CancellationToken cancellationToken
            ) =>
            {
                var response = await handler.HandleByApiKeyAsync(apiKeyId, cancellationToken);
                return Results.Ok(response);
            }
        );

        return app;
    }
}
