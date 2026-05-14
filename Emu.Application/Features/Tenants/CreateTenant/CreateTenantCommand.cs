namespace Application.Features.Tenants.CreateTenant;

public sealed record CreateTenantRequest(string Name, string Slug);

public sealed record CreateTenantResponse(Guid Id, string Name, string Slug);
