using Application.Abstractions.Persistence;
using Application.Common;

namespace Application.Features.Tenants.GetTenant;

public sealed class GetTenantHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTenantHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<GetTenantResponse> HandleAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var tenant = await _unitOfWork.Tenants.GetByIdAsync(id, cancellationToken);

        if (tenant is null)
        {
            throw new AppException("Tenant not found.");
        }

        return new GetTenantResponse(
            tenant.TenantId,
            tenant.Name,
            tenant.Slug,
            tenant.IsActive,
            tenant.CreatedAt
        );
    }
}
