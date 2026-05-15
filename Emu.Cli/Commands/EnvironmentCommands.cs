using System.CommandLine;
using Sdk.Requests;

namespace Cli.Commands;

public static class EnvironmentCommands
{
    public static Command Create()
    {
        var command = new Command("env", "Manage environments.");

        command.Subcommands.Add(CreateEnvironment());
        command.Subcommands.Add(ListEnvironments());

        return command;
    }

    private static Command CreateEnvironment()
    {
        var command = new Command("create", "Create environment.");

        var projectId = new Option<Guid>("--project-id") { Required = true };
        var name = new Option<string>("--name") { Required = true };
        var slug = new Option<string?>("--slug");

        command.Options.Add(projectId);
        command.Options.Add(name);
        command.Options.Add(slug);

        command.SetAction(async result =>
        {
            var client = await VaultSecretClientFactory.CreateAsync();

            var response = await client.CreateEnvironmentAsync(
                new CreateEnvironmentRequest(
                    result.GetValue(projectId),
                    result.GetValue(name)!,
                    result.GetValue(slug)
                )
            );

            Console.WriteLine($"{response.Id} | {response.Name} | {response.Slug}");
        });

        return command;
    }

    private static Command ListEnvironments()
    {
        var command = new Command("list", "List environments.");

        var projectId = new Option<Guid>("--project-id") { Required = true };
        command.Options.Add(projectId);

        command.SetAction(async result =>
        {
            var client = await VaultSecretClientFactory.CreateAsync();
            var response = await client.ListEnvironmentsAsync(result.GetValue(projectId));

            foreach (var item in response)
            {
                Console.WriteLine(
                    $"{item.Id} | {item.Name} | {item.Slug} | Active: {item.IsActive}"
                );
            }
        });

        return command;
    }
}
