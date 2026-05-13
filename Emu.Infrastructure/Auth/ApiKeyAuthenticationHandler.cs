using System.Security.Claims;
using System.Text.Encodings.Web;
using Application.Abstractions.Auth;
using Application.Abstractions.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Auth;

public sealed class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "ApiKey";

    private const string HeaderName = "X-Api-Key";

    private readonly IApiKeyRepository _apiKeyRepository;
    private readonly IApiKeyHasher _apiKeyHasher;
    private readonly Application.Abstractions.Time.ISystemClock _clock;

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock systemClock,
        IApiKeyRepository apiKeyRepository,
        IApiKeyHasher apiKeyHasher,
        Application.Abstractions.Time.ISystemClock clock
    )
        : base(options, logger, encoder, systemClock)
    {
        _apiKeyRepository = apiKeyRepository;
        _apiKeyHasher = apiKeyHasher;
        _clock = clock;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(HeaderName, out var apiKeyHeader))
        {
            return AuthenticateResult.NoResult();
        }

        var rawApiKey = apiKeyHeader.ToString();

        if (string.IsNullOrWhiteSpace(rawApiKey))
        {
            return AuthenticateResult.Fail("Missing API key.");
        }

        var prefix = ExtractPrefix(rawApiKey);

        if (prefix is null)
        {
            return AuthenticateResult.Fail("Invalid API key format.");
        }

        var apiKey = await _apiKeyRepository.GetByPrefixAsync(prefix, Context.RequestAborted);

        if (apiKey is null || !apiKey.IsActive)
        {
            return AuthenticateResult.Fail("Invalid API key.");
        }

        if (apiKey.ExpiresAt is not null && apiKey.ExpiresAt <= _clock.UtcNow)
        {
            return AuthenticateResult.Fail("API key expired.");
        }

        if (!_apiKeyHasher.Verify(rawApiKey, apiKey.KeyHash))
        {
            return AuthenticateResult.Fail("Invalid API key.");
        }

        await _apiKeyRepository.UpdateLastUsedAtAsync(
            apiKey.ApiKeyId,
            _clock.UtcNow,
            Context.RequestAborted
        );

        var claims = new List<Claim>
        {
            new("actor_type", "api_key"),
            new("actor_id", apiKey.ApiKeyId.ToString()),
            new("tenant_id", apiKey.TenantId.ToString()),
            new(ClaimTypes.NameIdentifier, apiKey.ApiKeyId.ToString()),
            new(ClaimTypes.Name, apiKey.Name),
        };

        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return AuthenticateResult.Success(ticket);
    }

    private static string? ExtractPrefix(string rawApiKey)
    {
        var dotIndex = rawApiKey.IndexOf('.');

        if (dotIndex <= 0)
        {
            return null;
        }

        return rawApiKey[..dotIndex];
    }
}
