using Application.Abstractions.Persistence;

namespace Application.Features.Environments.ListEnvironments;

public sealed class ListEnvironmentsHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public ListEnvironmentsHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<EnvironmentListItemResponse>> HandleAsync(
        Guid projectId,
        CancellationToken cancellationToken = default
    )
    {
        var environments = await _unitOfWork.Environments.ListByProjectAsync(
            projectId,
            cancellationToken
        );

        return environments
            .Select(x => new EnvironmentListItemResponse(
                x.EnvironmentId,
                x.ProjectId,
                x.Name,
                x.Slug,
                x.IsActive,
                x.CreatedAt
            ))
            .ToList();
    }
}
