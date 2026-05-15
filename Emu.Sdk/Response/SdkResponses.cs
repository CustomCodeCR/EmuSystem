namespace Sdk.Responses;

public sealed record LoginResponse(string AccessToken, Guid UserId, Guid TenantId, string Email);

public sealed record CreateUserResponse(Guid Id, Guid TenantId, string Email, string FullName);

public sealed record UserListItemResponse(
    Guid Id,
    Guid TenantId,
    string Email,
    string FullName,
    bool IsActive,
    DateTimeOffset CreatedAt,
    DateTimeOffset? LastLoginAt
);

public sealed record CreateTenantResponse(Guid Id, string Name, string Slug);

public sealed record GetTenantResponse(
    Guid Id,
    string Name,
    string Slug,
    bool IsActive,
    DateTimeOffset CreatedAt
);

public sealed record TenantListItemResponse(
    Guid Id,
    string Name,
    string Slug,
    bool IsActive,
    DateTimeOffset CreatedAt
);

public sealed record CreateProjectResponse(Guid Id, Guid TenantId, string Name, string Slug);

public sealed record ProjectListItemResponse(
    Guid Id,
    Guid TenantId,
    string Name,
    string Slug,
    bool IsActive,
    DateTimeOffset CreatedAt
);

public sealed record CreateEnvironmentResponse(Guid Id, Guid ProjectId, string Name, string Slug);

public sealed record EnvironmentListItemResponse(
    Guid Id,
    Guid ProjectId,
    string Name,
    string Slug,
    bool IsActive,
    DateTimeOffset CreatedAt
);

public sealed record CreateApiKeyResponse(Guid Id, string Name, string KeyPrefix, string ApiKey);

public sealed record ApiKeyListItemResponse(
    Guid Id,
    Guid TenantId,
    string Name,
    string KeyPrefix,
    bool IsActive,
    DateTimeOffset CreatedAt,
    DateTimeOffset? ExpiresAt,
    DateTimeOffset? LastUsedAt
);

public sealed record DisableApiKeyResponse(Guid Id, bool IsActive);

public sealed record CreateAccessPolicyResponse(Guid Id, Guid ApiKeyId, string PathPrefix);

public sealed record AccessPolicyListItemResponse(
    Guid Id,
    Guid ApiKeyId,
    Guid TenantId,
    Guid? ProjectId,
    Guid? EnvironmentId,
    string PathPrefix,
    bool CanRead,
    bool CanWrite,
    bool CanDelete,
    DateTimeOffset CreatedAt
);

public sealed record CreateSecretResponse(Guid Id, string Path, int VersionNumber);

public sealed record GetSecretResponse(
    Guid Id,
    string Name,
    string Path,
    string Value,
    int VersionNumber
);

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

public sealed record RotateSecretResponse(Guid Id, string Path, int VersionNumber);

public sealed record DeleteSecretResponse(Guid Id, string Status);

public sealed record AuditLogListItemResponse(
    Guid Id,
    Guid TenantId,
    string ActorType,
    Guid? ActorId,
    string Action,
    string ResourceType,
    Guid? ResourceId,
    string? Path,
    string? IpAddress,
    string? UserAgent,
    DateTimeOffset CreatedAt
);
