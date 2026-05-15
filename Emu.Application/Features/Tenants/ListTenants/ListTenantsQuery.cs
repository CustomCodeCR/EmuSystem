namespace Application.Features.Tenants.ListTenants;

public sealed record TenantListItemResponse(
    Guid Id,
    string Name,
    string Slug,
    bool IsActive,
    DateTimeOffset CreatedAt
);
