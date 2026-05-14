using Application.Abstractions.Auth;
using Application.Abstractions.Crypto;
using Application.Abstractions.Persistence;
using Application.Abstractions.Time;
using Domain.AuditLogs;
using Domain.Secrets;

namespace Application.Features.Secrets.CreateSecret;

public sealed class CreateSecretHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISecretEncryptionService _encryptionService;
    private readonly ICurrentActorService _currentActor;
    private readonly ISystemClock _clock;

    public CreateSecretHandler(
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

    public async Task<CreateSecretResponse> HandleAsync(
        CreateSecretRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var existing = await _unitOfWork.Secrets.GetByPathAsync(
            request.EnvironmentId,
            request.Path,
            cancellationToken
        );

        if (existing is not null)
        {
            throw new InvalidOperationException("Secret already exists.");
        }

        var encrypted = _encryptionService.Encrypt(request.Value);

        var secret = new Secret
        {
            SecretId = Guid.NewGuid(),
            EnvironmentId = request.EnvironmentId,
            Name = request.Name,
            Path = request.Path,
            CurrentVersionNumber = 1,
            Status = SecretStatus.Active,
            CreatedAt = _clock.UtcNow,
        };

        var version = new SecretVersion
        {
            SecretVersionId = Guid.NewGuid(),
            SecretId = secret.SecretId,
            VersionNumber = 1,
            EncryptedValue = encrypted.EncryptionValue,
            Nonce = encrypted.Nonce,
            Tag = encrypted.Tag,
            Algorithm = encrypted.Algorithm,
            CreatedBy = _currentActor.ActorId,
            CreatedAt = _clock.UtcNow,
        };

        await _unitOfWork.Secrets.AddAsync(secret, cancellationToken);
        await _unitOfWork.SecretVersions.AddAsync(version, cancellationToken);

        await _unitOfWork.AuditLogs.AddAsync(
            new AuditLog
            {
                AuditLogId = Guid.NewGuid(),
                TenantId = _currentActor.TenantId!.Value,
                ActorType = _currentActor.ActorType,
                ActorId = _currentActor.ActorId,
                Action = "SECRET_CREATED",
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

        return new CreateSecretResponse(secret.SecretId, secret.Path, secret.CurrentVersionNumber);
    }
}
