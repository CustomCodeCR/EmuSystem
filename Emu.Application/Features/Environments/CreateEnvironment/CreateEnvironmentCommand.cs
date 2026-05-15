namespace Application.Features.Environments.CreateEnvironment;

public sealed record CreateEnvironmentRequest(Guid ProjectId, string Name, string? Slug);

public sealed record CreateEnvironmentResponse(Guid Id, Guid ProjectId, string Name, string Slug);
