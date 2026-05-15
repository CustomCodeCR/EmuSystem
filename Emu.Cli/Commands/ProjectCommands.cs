using System.CommandLine;
using Sdk.Requests;

namespace Cli.Commands;

public static class ProjectCommands
{
    public static Command Create()
    {
        var command = new Command("project", "Manage projects.");

        command.Subcommands.Add(CreateProject());
        command.Subcommands.Add(ListProjects());

        return command;
    }

    private static Command CreateProject()
    {
        var command = new Command("create", "Create project.");

        var tenantId = new Option<Guid>("--tenant-id") { Required = true };
        var name = new Option<string>("--name") { Required = true };
        var slug = new Option<string?>("--slug");

        command.Options.Add(tenantId);
        command.Options.Add(name);
        command.Options.Add(slug);

        command.SetAction(async result =>
        {
            var client = await VaultSecretClientFactory.CreateAsync();

            var response = await client.CreateProjectAsync(
                new CreateProjectRequest(
                    result.GetValue(tenantId),
                    result.GetValue(name)!,
                    result.GetValue(slug)
                )
            );

            Console.WriteLine($"{response.Id} | {response.Name} | {response.Slug}");
        });

        return command;
    }

    private static Command ListProjects()
    {
        var command = new Command("list", "List projects.");

        var tenantId = new Option<Guid>("--tenant-id") { Required = true };
        command.Options.Add(tenantId);

        command.SetAction(async result =>
        {
            var client = await VaultSecretClientFactory.CreateAsync();
            var response = await client.ListProjectsAsync(result.GetValue(tenantId));

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
