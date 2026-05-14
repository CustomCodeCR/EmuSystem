namespace Application.Features.Secrets.GetSecret;

public sealed record GetSecretByPathRequest(Guid EnvironmentId, string Path);

public sealed record GetSecretResponse(
    Guid Id,
    string Name,
    string Path,
    string Value,
    int VersionNumber
);
