using System.CommandLine;
using Cli.Config;

namespace Cli.Commands;

public static class LoginCommand
{
    public static Command Create()
    {
        var command = new Command("login", "Configure CLI credentials.");

        var baseUrl = new Option<string>("--base-url")
        {
            Required = true,
            Description = "VaultSecret API base URL.",
        };

        var apiKey = new Option<string?>("--api-key") { Description = "VaultSecret API key." };

        var accessToken = new Option<string?>("--access-token")
        {
            Description = "JWT access token.",
        };

        command.Options.Add(baseUrl);
        command.Options.Add(apiKey);
        command.Options.Add(accessToken);

        command.SetAction(async result =>
        {
            var config = new VaultSecretCliConfig
            {
                BaseUrl = result.GetValue(baseUrl)!.TrimEnd('/'),
                ApiKey = result.GetValue(apiKey),
                AccessToken = result.GetValue(accessToken),
            };

            await new VaultSecretCliConfigStore().SaveAsync(config);

            Console.WriteLine("VaultSecret CLI configured.");
        });

        return command;
    }
}
