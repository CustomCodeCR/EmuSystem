using Application.Abstractions.Persistence;

namespace Application.Features.Policies.ListAccessPolicies;

public sealed class ListAccessPoliciesHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public ListAccessPoliciesHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<AccessPolicyListItemResponse>> HandleByApiKeyAsync(
        Guid apiKeyId,
        CancellationToken cancellationToken = default
    )
    {
        var policies = await _unitOfWork.AccessPolicies.ListByApiKeyAsync(
            apiKeyId,
            cancellationToken
        );

        return policies
            .Select(x => new AccessPolicyListItemResponse(
                x.AccessPolicyId,
                x.ApiKeyId,
                x.TenantId,
                x.ProjectId,
                x.EnvironmentId,
                x.PathPrefix,
                x.CanRead,
                x.CanWrite,
                x.CanDelete,
                x.CreatedAt
            ))
            .ToList();
    }
}
