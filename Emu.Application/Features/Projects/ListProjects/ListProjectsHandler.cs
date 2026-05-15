using Application.Abstractions.Persistence;

namespace Application.Features.Projects.ListProjects;

public sealed class ListProjectsHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public ListProjectsHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<ProjectListItemResponse>> HandleAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default
    )
    {
        var projects = await _unitOfWork.Projects.ListByTenantAsync(tenantId, cancellationToken);

        return projects
            .Select(x => new ProjectListItemResponse(
                x.ProjectId,
                x.TenantId,
                x.Name,
                x.Slug,
                x.IsActive,
                x.CreatedAt
            ))
            .ToList();
    }
}
