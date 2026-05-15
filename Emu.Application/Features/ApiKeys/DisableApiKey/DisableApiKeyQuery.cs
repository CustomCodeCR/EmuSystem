namespace Application.Features.ApiKeys.DisableApiKey;

public sealed record DisableApiKeyResponse(Guid Id, bool IsActive);
