using Application.Abstractions.Auth;
using Application.Abstractions.Crypto;
using Application.Abstractions.Persistence;
using Application.Abstractions.Time;
using Domain.AuditLogs;

namespace Application.Features.Secrets.GetSecret;

public sealed class GetSecretByPathHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISecretEncryptionService _encryptionService;
    private readonly ICurrentActorService _currentActor;
    private readonly ISystemClock _clock;

    public GetSecretByPathHandler(
        IUnitOfWork unitOfWork,
        ISecretEncryptionService encryptionService,
        ICurrentActorService currentActor,
        ISystemClock clock
    )
    {
        _unitOfWork = unitOfWork;
        _encryptionService = encryptionService;
        _currentActor = currentActor;
        _clock = clock;
    }

    public async Task<GetSecretResponse> HandleAsync(
        GetSecretByPathRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var secret = await _unitOfWork.Secrets.GetByPathAsync(
            request.EnvironmentId,
            request.Path,
            cancellationToken
        );

        if (secret is null)
        {
            throw new InvalidOperationException("Secret not found.");
        }

        var version = await _unitOfWork.SecretVersions.GetCurrentBySecretIdAsync(
            secret.SecretId,
            secret.CurrentVersionNumber,
            cancellationToken
        );

        if (version is null)
        {
            throw new InvalidOperationException("Secret version not found.");
        }

        var value = _encryptionService.Decrypt(
            new EncryptionSecret(
                version.EncryptedValue,
                version.Nonce,
                version.Tag,
                version.Algorithm
            )
        );

        await _unitOfWork.AuditLogs.AddAsync(
            new AuditLog
            {
                AuditLogId = Guid.NewGuid(),
                TenantId = _currentActor.TenantId!.Value,
                ActorType = _currentActor.ActorType,
                ActorId = _currentActor.ActorId,
                Action = "SECRET_READ",
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

        return new GetSecretResponse(
            secret.SecretId,
            secret.Name,
            secret.Path,
            value,
            secret.CurrentVersionNumber
        );
    }
}
