namespace Application.Features.Policies.CreateAccessPolicy;

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

public sealed record CreateAccessPolicyResponse(Guid Id, Guid ApiKeyId, string PathPrefix);
