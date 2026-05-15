namespace Application.Features.Tenants.GetTenant;

public sealed record GetTenantResponse(
    Guid Id,
    string Name,
    string Slug,
    bool IsActive,
    DateTimeOffset CreatedAt
);
