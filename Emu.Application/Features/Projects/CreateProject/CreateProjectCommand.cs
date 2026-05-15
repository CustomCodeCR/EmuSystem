namespace Application.Features.Projects.CreateProject;

public sealed record CreateProjectRequest(Guid TenantId, string Name, string? Slug);

public sealed record CreateProjectResponse(Guid Id, Guid TenantId, string Name, string Slug);
