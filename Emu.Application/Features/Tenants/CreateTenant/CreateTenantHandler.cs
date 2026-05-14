using Application.Abstractions.Persistence;
using Application.Abstractions.Time;
using Domain.Tenants;

namespace Application.Features.Tenants.CreateTenant;

public sealed class CreateTenantHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemClock _clock;

    public CreateTenantHandler(IUnitOfWork unitOfWork, ISystemClock clock)
    {
        _unitOfWork = unitOfWork;
        _clock = clock;
    }

    public async Task<CreateTenantResponse> HandleAsync(
        CreateTenantRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var existing = await _unitOfWork.Tenants.GetBySlugAsync(request.Slug, cancellationToken);

        if (existing is not null)
        {
            throw new InvalidOperationException("Tenant slug already exists.");
        }

        var tenant = new Tenant
        {
            TenantId = Guid.NewGuid(),
            Name = request.Name,
            Slug = request.Slug,
            IsActive = true,
            CreatedAt = _clock.UtcNow,
        };

        await _unitOfWork.Tenants.AddAsync(tenant, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateTenantResponse(tenant.TenantId, tenant.Name, tenant.Slug);
    }
}
