using System.Net.Http.Headers;
using Cli.Config;
using Sdk;

namespace Cli;

public static class VaultSecretClientFactory
{
    public static async Task<VaultSecretClient> CreateAsync(
        CancellationToken cancellationToken = default
    )
    {
        var store = new VaultSecretCliConfigStore();
        var config = await store.LoadAsync(cancellationToken);

        var httpClient = new HttpClient { BaseAddress = new Uri(config.BaseUrl.TrimEnd('/')) };

        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("VaultSecret.Cli/1.0");

        if (!string.IsNullOrWhiteSpace(config.ApiKey))
        {
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", config.ApiKey);
        }

        if (!string.IsNullOrWhiteSpace(config.AccessToken))
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                config.AccessToken
            );
        }

        return new VaultSecretClient(httpClient);
    }
}
