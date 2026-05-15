namespace Application.Features.ApiKeys.ListApiKeys;

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
