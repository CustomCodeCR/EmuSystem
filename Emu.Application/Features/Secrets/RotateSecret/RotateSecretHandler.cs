using Application.Abstractions.Auth;
using Application.Abstractions.Crypto;
using Application.Abstractions.Persistence;
using Application.Abstractions.Time;
using Application.Common;
using Domain.AuditLogs;
using Domain.Secrets;

namespace Application.Features.Secrets.RotateSecret;

public sealed class RotateSecretHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISecretEncryptionService _encryptionService;
    private readonly ICurrentActorService _currentActor;
    private readonly ISystemClock _clock;

    public RotateSecretHandler(
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

    public async Task<RotateSecretResponse> HandleAsync(
        Guid secretId,
        RotateSecretRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var secret = await _unitOfWork.Secrets.GetByIdAsync(secretId, cancellationToken);

        if (secret is null)
        {
            throw new AppException("Secret not found.");
        }

        var newVersionNumber = secret.CurrentVersionNumber + 1;
        var encrypted = _encryptionService.Encrypt(request.Value);

        var version = new SecretVersion
        {
            SecretVersionId = Guid.NewGuid(),
            SecretId = secret.SecretId,
            VersionNumber = newVersionNumber,
            EncryptedValue = encrypted.EncryptionValue,
            Nonce = encrypted.Nonce,
            Tag = encrypted.Tag,
            Algorithm = encrypted.Algorithm,
            CreatedBy = _currentActor.ActorId,
            CreatedAt = _clock.UtcNow,
        };

        secret.CurrentVersionNumber = newVersionNumber;
        secret.UpdatedAt = _clock.UtcNow;

        await _unitOfWork.SecretVersions.AddAsync(version, cancellationToken);
        _unitOfWork.Secrets.Update(secret);

        await _unitOfWork.AuditLogs.AddAsync(
            new AuditLog
            {
                AuditLogId = Guid.NewGuid(),
                TenantId = _currentActor.TenantId!.Value,
                ActorType = _currentActor.ActorType,
                ActorId = _currentActor.ActorId,
                Action = "SECRET_ROTATED",
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

        return new RotateSecretResponse(secret.SecretId, secret.Path, secret.CurrentVersionNumber);
    }
}
