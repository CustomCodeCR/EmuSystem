using Application.Abstractions.Persistence;

namespace Application.Features.Tenants.ListTenants;

public sealed class ListTenantsHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public ListTenantsHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<TenantListItemResponse>> HandleAsync(
        CancellationToken cancellationToken = default
    )
    {
        var tenants = await _unitOfWork.Tenants.ListAsync(cancellationToken);

        return tenants
            .Select(x => new TenantListItemResponse(
                x.TenantId,
                x.Name,
                x.Slug,
                x.IsActive,
                x.CreatedAt
            ))
            .ToList();
    }
}
