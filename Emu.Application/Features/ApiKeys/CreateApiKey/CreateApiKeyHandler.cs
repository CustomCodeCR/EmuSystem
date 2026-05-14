using Application.Abstractions.Auth;
using Application.Abstractions.Persistence;
using Application.Abstractions.Time;
using Domain.ApiKeys;

namespace Application.Features.ApiKeys.CreateApiKey;

public sealed class CreateApiKeyHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IApiKeyGenerator _apiKeyGenerator;
    private readonly IApiKeyHasher _apiKeyHasher;
    private readonly ISystemClock _clock;

    public CreateApiKeyHandler(
        IUnitOfWork unitOfWork,
        IApiKeyGenerator apiKeyGenerator,
        IApiKeyHasher apiKeyHasher,
        ISystemClock clock
    )
    {
        _unitOfWork = unitOfWork;
        _apiKeyGenerator = apiKeyGenerator;
        _apiKeyHasher = apiKeyHasher;
        _clock = clock;
    }

    public async Task<CreateApiKeyResponse> HandleAsync(
        CreateApiKeyRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var tenant = await _unitOfWork.Tenants.GetByIdAsync(request.TenantId, cancellationToken);

        if (tenant is null)
        {
            throw new InvalidOperationException("Tenant not found.");
        }

        var generated = _apiKeyGenerator.Generate();

        var apiKey = new ApiKey
        {
            ApiKeyId = Guid.NewGuid(),
            TenantId = request.TenantId,
            Name = request.Name,
            Description = request.Description,
            KeyPrefix = generated.Prefix,
            KeyHash = _apiKeyHasher.Hash(generated.PlainTextKey),
            IsActive = true,
            ExpiresAt = request.ExpiresAt,
            CreatedAt = _clock.UtcNow,
        };

        await _unitOfWork.ApiKeys.AddAsync(apiKey, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateApiKeyResponse(
            apiKey.ApiKeyId,
            apiKey.Name,
            apiKey.KeyPrefix,
            generated.PlainTextKey
        );
    }
}
