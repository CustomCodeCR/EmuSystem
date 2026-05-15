namespace Sdk;

public sealed class VaultSecretClientOptions
{
    public string BaseUrl { get; set; } = default!;

    public string? ApiKey { get; set; }

    public string? AccessToken { get; set; }
}
