using System.Text.Json;

namespace Cli.Config;

public sealed class VaultSecretCliConfigStore
{
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

    public string ConfigPath { get; }

    public VaultSecretCliConfigStore()
    {
        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        ConfigPath = Path.Combine(home, ".config", "vaultsecret", "config.json");
    }

    public async Task SaveAsync(
        VaultSecretCliConfig config,
        CancellationToken cancellationToken = default
    )
    {
        var directory = Path.GetDirectoryName(ConfigPath);

        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var json = JsonSerializer.Serialize(config, Options);

        await File.WriteAllTextAsync(ConfigPath, json, cancellationToken);
    }

    public async Task<VaultSecretCliConfig> LoadAsync(CancellationToken cancellationToken = default)
    {
        if (!File.Exists(ConfigPath))
        {
            throw new InvalidOperationException("CLI is not configured. Run: vaultsecret login");
        }

        var json = await File.ReadAllTextAsync(ConfigPath, cancellationToken);

        return JsonSerializer.Deserialize<VaultSecretCliConfig>(json)
            ?? throw new InvalidOperationException("Invalid CLI config file.");
    }
}
