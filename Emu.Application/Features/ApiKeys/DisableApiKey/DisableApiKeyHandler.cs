using Application.Abstractions.Persistence;
using Application.Abstractions.Time;
using Application.Common;

namespace Application.Features.ApiKeys.DisableApiKey;

public sealed class DisableApiKeyHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemClock _clock;

    public DisableApiKeyHandler(IUnitOfWork unitOfWork, ISystemClock clock)
    {
        _unitOfWork = unitOfWork;
        _clock = clock;
    }

    public async Task<DisableApiKeyResponse> HandleAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var apiKey = await _unitOfWork.ApiKeys.GetByIdAsync(id, cancellationToken);

        if (apiKey is null)
        {
            throw new AppException("API key not found.");
        }

        apiKey.IsActive = false;
        apiKey.UpdatedAt = _clock.UtcNow;

        _unitOfWork.ApiKeys.Update(apiKey);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new DisableApiKeyResponse(apiKey.ApiKeyId, apiKey.IsActive);
    }
}
