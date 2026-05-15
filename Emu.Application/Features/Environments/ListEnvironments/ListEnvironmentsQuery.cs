namespace Application.Features.Environments.ListEnvironments;

public sealed record EnvironmentListItemResponse(
    Guid Id,
    Guid ProjectId,
    string Name,
    string Slug,
    bool IsActive,
    DateTimeOffset CreatedAt
);
