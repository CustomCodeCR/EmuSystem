using Application.Abstractions.Persistence;

namespace Application.Features.ApiKeys.ListApiKeys;

public sealed class ListApiKeysHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public ListApiKeysHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<ApiKeyListItemResponse>> HandleAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default
    )
    {
        var apiKeys = await _unitOfWork.ApiKeys.ListByTenantAsync(tenantId, cancellationToken);

        return apiKeys
            .Select(x => new ApiKeyListItemResponse(
                x.ApiKeyId,
                x.TenantId,
                x.Name,
                x.KeyPrefix,
                x.IsActive,
                x.CreatedAt,
                x.ExpiresAt,
                x.LastUsedAt
            ))
            .ToList();
    }
}
