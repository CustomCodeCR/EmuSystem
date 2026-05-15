namespace Application.Features.Secrets.ListSecrets;

public sealed record SecretListItemResponse(
    Guid Id,
    Guid EnvironmentId,
    string Name,
    string Path,
    int CurrentVersionNumber,
    string Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
);
