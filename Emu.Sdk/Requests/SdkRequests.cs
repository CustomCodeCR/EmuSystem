namespace Sdk.Requests;

public sealed record LoginRequest(Guid TenantId, string Email, string Password);

public sealed record CreateUserRequest(
    Guid TenantId,
    string Email,
    string FullName,
    string Password
);

public sealed record CreateTenantRequest(string Name, string Slug);

public sealed record CreateProjectRequest(Guid TenantId, string Name, string? Slug);

public sealed record CreateEnvironmentRequest(Guid ProjectId, string Name, string? Slug);

public sealed record CreateApiKeyRequest(
    Guid TenantId,
    string Name,
    string? Description,
    DateTimeOffset? ExpiresAt
);

public sealed record CreateAccessPolicyRequest(
    Guid ApiKeyId,
    Guid TenantId,
    Guid? ProjectId,
    Guid? EnvironmentId,
    string PathPrefix,
    bool CanRead,
    bool CanWrite,
    bool CanDelete
);

public sealed record CreateSecretRequest(
    Guid EnvironmentId,
    string Name,
    string Path,
    string Value
);

public sealed record RotateSecretRequest(string Value);
