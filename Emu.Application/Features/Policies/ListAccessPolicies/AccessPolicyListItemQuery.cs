namespace Application.Features.Policies.ListAccessPolicies;

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
