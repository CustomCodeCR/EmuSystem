using Api.Features.ApiKeys;
using Api.Features.AuditLogs;
using Api.Features.Environments;
using Api.Features.Policies;
using Api.Features.Projects;
using Api.Features.Secrets;
using Api.Features.Tenants;
using Api.Features.Users;

namespace Api.Extensions;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapTenantEndpoints();
        app.MapProjectEndpoints();
        app.MapEnvironmentEndpoints();
        app.MapApiKeyEndpoints();
        app.MapPolicyEndpoints();
        app.MapSecretEndpoints();
        app.MapAuditLogEndpoints();
        app.MapUserEndpoints();

        return app;
    }
}
