namespace Application.Features.Projects.ListProjects;

public sealed record ProjectListItemResponse(
    Guid Id,
    Guid TenantId,
    string Name,
    string Slug,
    bool IsActive,
    DateTimeOffset CreatedAt
);
