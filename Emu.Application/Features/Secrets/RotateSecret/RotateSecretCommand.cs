namespace Application.Features.Secrets.RotateSecret;

public sealed record RotateSecretRequest(string Value);

public sealed record RotateSecretResponse(Guid Id, string Path, int VersionNumber);
