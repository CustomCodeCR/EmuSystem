using Application.Abstractions.Auth;
using Application.Abstractions.Persistence;
using Application.Abstractions.Time;
using Application.Common;
using Domain.AuditLogs;
using Domain.Secrets;

namespace Application.Features.Secrets.DeleteSecret;

public sealed class DeleteSecretHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentActorService _currentActor;
    private readonly ISystemClock _clock;

    public DeleteSecretHandler(
        IUnitOfWork unitOfWork,
        ICurrentActorService currentActor,
        ISystemClock clock
    )
    {
        _unitOfWork = unitOfWork;
        _currentActor = currentActor;
        _clock = clock;
    }

    public async Task<DeleteSecretResponse> HandleAsync(
        Guid secretId,
        CancellationToken cancellationToken = default
    )
    {
        var secret = await _unitOfWork.Secrets.GetByIdAsync(secretId, cancellationToken);

        if (secret is null)
        {
            throw new AppException("Secret not found.");
        }

        secret.Status = SecretStatus.Deleted;
        secret.UpdatedAt = _clock.UtcNow;

        _unitOfWork.Secrets.Update(secret);

        await _unitOfWork.AuditLogs.AddAsync(
            new AuditLog
            {
                AuditLogId = Guid.NewGuid(),
                TenantId = _currentActor.TenantId!.Value,
                ActorType = _currentActor.ActorType,
                ActorId = _currentActor.ActorId,
                Action = "SECRET_DELETED",
                ResourceType = ResourceType.Secret,
                ResourceId = secret.SecretId,
                Path = secret.Path,
                IpAddress = _currentActor.IpAddress,
                UserAgent = _currentActor.UserAgent,
                CreatedAt = _clock.UtcNow,
            },
            cancellationToken
        );

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new DeleteSecretResponse(secret.SecretId, secret.Status.ToString());
    }
}
