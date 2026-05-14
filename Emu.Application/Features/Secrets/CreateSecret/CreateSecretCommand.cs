namespace Application.Features.Secrets.CreateSecret;

public sealed record CreateSecretRequest(
    Guid EnvironmentId,
    string Name,
    string Path,
    string Value
);

public sealed record CreateSecretResponse(Guid Id, string Path, int VersionNumber);
