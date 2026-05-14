using System.Security.Cryptography;
using Application.Abstractions.Auth;

namespace Infrastructure.Auth;

public sealed class ApiKeyGenerator : IApiKeyGenerator
{
    public GeneratedApiKey Generate()
    {
        var prefixRandom = Convert
            .ToHexString(RandomNumberGenerator.GetBytes(4))
            .ToLowerInvariant();
        var secretRandom = Convert
            .ToBase64String(RandomNumberGenerator.GetBytes(48))
            .Replace("+", "")
            .Replace("/", "")
            .Replace("=", "");

        var prefix = $"ccv_live_{prefixRandom}";
        var plainTextKey = $"{prefix}.{secretRandom}";

        return new GeneratedApiKey(prefix, plainTextKey);
    }
}
