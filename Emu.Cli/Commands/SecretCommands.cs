using System.CommandLine;
using Sdk.Requests;

namespace Cli.Commands;

public static class SecretCommands
{
    public static Command Create()
    {
        var command = new Command("secret", "Manage secrets.");

        command.Subcommands.Add(SetSecret());
        command.Subcommands.Add(GetSecret());
        command.Subcommands.Add(ListSecrets());
        command.Subcommands.Add(RotateSecret());
        command.Subcommands.Add(DeleteSecret());

        return command;
    }

    private static Command SetSecret()
    {
        var command = new Command("set", "Create secret.");

        var environmentId = new Option<Guid>("--environment-id") { Required = true };
        var name = new Option<string>("--name") { Required = true };
        var path = new Option<string>("--path") { Required = true };
        var value = new Option<string>("--value") { Required = true };

        command.Options.Add(environmentId);
        command.Options.Add(name);
        command.Options.Add(path);
        command.Options.Add(value);

        command.SetAction(async result =>
        {
            var client = await VaultSecretClientFactory.CreateAsync();

            var response = await client.CreateSecretAsync(
                new CreateSecretRequest(
                    result.GetValue(environmentId),
                    result.GetValue(name)!,
                    result.GetValue(path)!,
                    result.GetValue(value)!
                )
            );

            Console.WriteLine(
                $"{response.Id} | {response.Path} | Version: {response.VersionNumber}"
            );
        });

        return command;
    }

    private static Command GetSecret()
    {
        var command = new Command("get", "Get secret value.");

        var environmentId = new Option<Guid>("--environment-id") { Required = true };
        var path = new Option<string>("--path") { Required = true };

        command.Options.Add(environmentId);
        command.Options.Add(path);

        command.SetAction(async result =>
        {
            var client = await VaultSecretClientFactory.CreateAsync();

            var response = await client.GetSecretByPathAsync(
                result.GetValue(environmentId),
                result.GetValue(path)!
            );

            Console.WriteLine(response.Value);
        });

        return command;
    }

    private static Command ListSecrets()
    {
        var command = new Command("list", "List secrets.");

        var environmentId = new Option<Guid>("--environment-id") { Required = true };
        command.Options.Add(environmentId);

        command.SetAction(async result =>
        {
            var client = await VaultSecretClientFactory.CreateAsync();
            var response = await client.ListSecretsAsync(result.GetValue(environmentId));

            foreach (var item in response)
            {
                Console.WriteLine(
                    $"{item.Id} | {item.Path} | Version: {item.CurrentVersionNumber} | {item.Status}"
                );
            }
        });

        return command;
    }

    private static Command RotateSecret()
    {
        var command = new Command("rotate", "Rotate secret.");

        var id = new Option<Guid>("--id") { Required = true };
        var value = new Option<string>("--value") { Required = true };

        command.Options.Add(id);
        command.Options.Add(value);

        command.SetAction(async result =>
        {
            var client = await VaultSecretClientFactory.CreateAsync();

            var response = await client.RotateSecretAsync(
                result.GetValue(id),
                new RotateSecretRequest(result.GetValue(value)!)
            );

            Console.WriteLine(
                $"{response.Id} | {response.Path} | Version: {response.VersionNumber}"
            );
        });

        return command;
    }

    private static Command DeleteSecret()
    {
        var command = new Command("delete", "Delete secret.");

        var id = new Option<Guid>("--id") { Required = true };
        command.Options.Add(id);

        command.SetAction(async result =>
        {
            var client = await VaultSecretClientFactory.CreateAsync();
            var response = await client.DeleteSecretAsync(result.GetValue(id));

            Console.WriteLine($"{response.Id} | Status: {response.Status}");
        });

        return command;
    }
}
