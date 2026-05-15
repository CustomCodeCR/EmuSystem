using System.CommandLine;
using Sdk.Requests;

namespace Cli.Commands;

public static class ApiKeyCommands
{
    public static Command Create()
    {
        var command = new Command("api-key", "Manage API keys.");

        command.Subcommands.Add(CreateApiKey());
        command.Subcommands.Add(ListApiKeys());
        command.Subcommands.Add(DisableApiKey());

        return command;
    }

    private static Command CreateApiKey()
    {
        var command = new Command("create", "Create API key.");

        var tenantId = new Option<Guid>("--tenant-id") { Required = true };
        var name = new Option<string>("--name") { Required = true };
        var description = new Option<string?>("--description");
        var expiresAt = new Option<DateTimeOffset?>("--expires-at");

        command.Options.Add(tenantId);
        command.Options.Add(name);
        command.Options.Add(description);
        command.Options.Add(expiresAt);

        command.SetAction(async result =>
        {
            var client = await VaultSecretClientFactory.CreateAsync();

            var response = await client.CreateApiKeyAsync(
                new CreateApiKeyRequest(
                    result.GetValue(tenantId),
                    result.GetValue(name)!,
                    result.GetValue(description),
                    result.GetValue(expiresAt)
                )
            );

            Console.WriteLine($"Id: {response.Id}");
            Console.WriteLine($"Prefix: {response.KeyPrefix}");
            Console.WriteLine($"API Key: {response.ApiKey}");
            Console.WriteLine("Save this API key now. It will not be shown again.");
        });

        return command;
    }

    private static Command ListApiKeys()
    {
        var command = new Command("list", "List API keys.");

        var tenantId = new Option<Guid>("--tenant-id") { Required = true };
        command.Options.Add(tenantId);

        command.SetAction(async result =>
        {
            var client = await VaultSecretClientFactory.CreateAsync();
            var response = await client.ListApiKeysAsync(result.GetValue(tenantId));

            foreach (var item in response)
            {
                Console.WriteLine(
                    $"{item.Id} | {item.Name} | {item.KeyPrefix} | Active: {item.IsActive}"
                );
            }
        });

        return command;
    }

    private static Command DisableApiKey()
    {
        var command = new Command("disable", "Disable API key.");

        var id = new Option<Guid>("--id") { Required = true };
        command.Options.Add(id);

        command.SetAction(async result =>
        {
            var client = await VaultSecretClientFactory.CreateAsync();
            var response = await client.DisableApiKeyAsync(result.GetValue(id));

            Console.WriteLine($"{response.Id} | Active: {response.IsActive}");
        });

        return command;
    }
}
