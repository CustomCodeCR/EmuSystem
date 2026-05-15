using Application.Abstractions.Persistence;
using Application.Abstractions.Time;
using Application.Common;
using Domain.AccessPolicies;

namespace Application.Features.Policies.CreateAccessPolicy;

public sealed class CreateAccessPolicyHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemClock _clock;

    public CreateAccessPolicyHandler(IUnitOfWork unitOfWork, ISystemClock clock)
    {
        _unitOfWork = unitOfWork;
        _clock = clock;
    }

    public async Task<CreateAccessPolicyResponse> HandleAsync(
        CreateAccessPolicyRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var apiKey = await _unitOfWork.ApiKeys.GetByIdAsync(request.ApiKeyId, cancellationToken);

        if (apiKey is null)
        {
            throw new AppException("API key not found.");
        }

        var policy = new AccessPolicy
        {
            AccessPolicyId = Guid.NewGuid(),
            ApiKeyId = request.ApiKeyId,
            TenantId = request.TenantId,
            ProjectId = request.ProjectId,
            EnvironmentId = request.EnvironmentId,
            PathPrefix = request.PathPrefix.Trim(),
            CanRead = request.CanRead,
            CanWrite = request.CanWrite,
            CanDelete = request.CanDelete,
            CreatedAt = _clock.UtcNow,
        };

        await _unitOfWork.AccessPolicies.AddAsync(policy, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateAccessPolicyResponse(
            policy.AccessPolicyId,
            policy.ApiKeyId,
            policy.PathPrefix
        );
    }
}
