using Application.Abstractions.Persistence;

namespace Application.Features.Secrets.ListSecrets;

public sealed class ListSecretsHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public ListSecretsHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<SecretListItemResponse>> HandleAsync(
        Guid environmentId,
        CancellationToken cancellationToken = default
    )
    {
        var secrets = await _unitOfWork.Secrets.ListByEnvironmentAsync(
            environmentId,
            cancellationToken
        );

        return secrets
            .Select(x => new SecretListItemResponse(
                x.SecretId,
                x.EnvironmentId,
                x.Name,
                x.Path,
                x.CurrentVersionNumber,
                x.Status.ToString(),
                x.CreatedAt,
                x.UpdatedAt
            ))
            .ToList();
    }
}
