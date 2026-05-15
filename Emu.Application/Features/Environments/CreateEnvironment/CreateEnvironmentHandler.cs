using Application.Abstractions.Persistence;
using Application.Abstractions.Time;
using Application.Common;
using Domain.Environments;

namespace Application.Features.Environments.CreateEnvironment;

public sealed class CreateEnvironmentHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemClock _clock;

    public CreateEnvironmentHandler(IUnitOfWork unitOfWork, ISystemClock clock)
    {
        _unitOfWork = unitOfWork;
        _clock = clock;
    }

    public async Task<CreateEnvironmentResponse> HandleAsync(
        CreateEnvironmentRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(request.ProjectId, cancellationToken);

        if (project is null)
        {
            throw new AppException("Project not found.");
        }

        var slug = string.IsNullOrWhiteSpace(request.Slug)
            ? SlugHelper.Generate(request.Name)
            : SlugHelper.Generate(request.Slug);

        var existing = await _unitOfWork.Environments.GetBySlugAsync(
            request.ProjectId,
            slug,
            cancellationToken
        );

        if (existing is not null)
        {
            throw new AppException("Environment slug already exists.");
        }

        var environment = new ProjectEnvironment
        {
            EnvironmentId = Guid.NewGuid(),
            ProjectId = request.ProjectId,
            Name = request.Name,
            Slug = slug,
            IsActive = true,
            CreatedAt = _clock.UtcNow,
        };

        await _unitOfWork.Environments.AddAsync(environment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateEnvironmentResponse(
            environment.EnvironmentId,
            environment.ProjectId,
            environment.Name,
            environment.Slug
        );
    }
}
