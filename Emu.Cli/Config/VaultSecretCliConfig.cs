namespace Cli.Config;

public sealed class VaultSecretCliConfig
{
    public string BaseUrl { get; set; } = default!;

    public string? ApiKey { get; set; }

    public string? AccessToken { get; set; }
}
