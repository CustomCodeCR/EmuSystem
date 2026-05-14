namespace Application.Features.ApiKeys.CreateApiKey;

public sealed record CreateApiKeyRequest(
    Guid TenantId,
    string Name,
    string? Description,
    DateTimeOffset? ExpiresAt
);

public sealed record CreateApiKeyResponse(Guid Id, string Name, string KeyPrefix, string ApiKey);
