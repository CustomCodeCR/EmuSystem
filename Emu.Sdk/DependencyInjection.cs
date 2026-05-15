using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sdk;

public static class DependencyInjection
{
    public static IServiceCollection AddVaultSecretClient(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var options =
            configuration.GetSection("VaultSecret").Get<VaultSecretClientOptions>()
            ?? throw new InvalidOperationException("VaultSecret options are missing.");

        services.AddHttpClient<VaultSecretClient>(client =>
        {
            client.BaseAddress = new Uri(options.BaseUrl.TrimEnd('/'));
            client.DefaultRequestHeaders.UserAgent.ParseAdd("VaultSecret.Sdk/1.0");

            if (!string.IsNullOrWhiteSpace(options.ApiKey))
            {
                client.DefaultRequestHeaders.Add("X-Api-Key", options.ApiKey);
            }

            if (!string.IsNullOrWhiteSpace(options.AccessToken))
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue(
                        "Bearer",
                        options.AccessToken
                    );
            }
        });

        return services;
    }
}
