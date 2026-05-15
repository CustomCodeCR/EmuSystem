using Application.Abstractions.Persistence;
using Application.Abstractions.Time;
using Application.Common;
using Domain.Projects;

namespace Application.Features.Projects.CreateProject;

public sealed class CreateProjectHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemClock _clock;

    public CreateProjectHandler(IUnitOfWork unitOfWork, ISystemClock clock)
    {
        _unitOfWork = unitOfWork;
        _clock = clock;
    }

    public async Task<CreateProjectResponse> HandleAsync(
        CreateProjectRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var tenant = await _unitOfWork.Tenants.GetByIdAsync(request.TenantId, cancellationToken);

        if (tenant is null)
        {
            throw new AppException("Tenant not found.");
        }

        var slug = string.IsNullOrWhiteSpace(request.Slug)
            ? SlugHelper.Generate(request.Name)
            : SlugHelper.Generate(request.Slug);

        var existing = await _unitOfWork.Projects.GetBySlugAsync(
            request.TenantId,
            slug,
            cancellationToken
        );

        if (existing is not null)
        {
            throw new AppException("Project slug already exists.");
        }

        var project = new Project
        {
            ProjectId = Guid.NewGuid(),
            TenantId = request.TenantId,
            Name = request.Name,
            Slug = slug,
            IsActive = true,
            CreatedAt = _clock.UtcNow,
        };

        await _unitOfWork.Projects.AddAsync(project, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateProjectResponse(
            project.ProjectId,
            project.TenantId,
            project.Name,
            project.Slug
        );
    }
}
