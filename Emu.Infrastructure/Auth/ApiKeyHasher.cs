using System.Security.Cryptography;
using System.Text;
using Application.Abstractions.Auth;
using Microsoft.Extensions.Options;

namespace Infrastructure.Auth;

public sealed class ApiKeyHasher : IApiKeyHasher
{
    private readonly byte[] _pepper;

    public ApiKeyHasher(IOptions<ApiKeyOptions> options)
    {
        if (string.IsNullOrWhiteSpace(options.Value.Pepper))
            throw new InvalidOperationException("API key pepper is not configured.");

        _pepper = Convert.FromBase64String(options.Value.Pepper);
    }

    public string Hash(string apiKey)
    {
        using var hmac = new HMACSHA256(_pepper);
        var bytes = Encoding.UTF8.GetBytes(apiKey);
        var hash = hmac.ComputeHash(bytes);

        return Convert.ToBase64String(hash);
    }

    public bool Verify(string apiKey, string hash)
    {
        var computeHash = Hash(apiKey);

        return CryptographicOperations.FixedTimeEquals(
            Convert.FromBase64String(computeHash),
            Convert.FromBase64String(hash)
        );
    }
}
