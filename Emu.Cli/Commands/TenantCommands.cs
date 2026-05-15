using System.CommandLine;
using Sdk.Requests;

namespace Cli.Commands;

public static class TenantCommands
{
    public static Command Create()
    {
        var command = new Command("tenant", "Manage tenants.");

        command.Subcommands.Add(CreateTenant());
        command.Subcommands.Add(GetTenant());
        command.Subcommands.Add(ListTenants());

        return command;
    }

    private static Command CreateTenant()
    {
        var command = new Command("create", "Create tenant.");

        var name = new Option<string>("--name") { Required = true };
        var slug = new Option<string>("--slug") { Required = true };

        command.Options.Add(name);
        command.Options.Add(slug);

        command.SetAction(async result =>
        {
            var client = await VaultSecretClientFactory.CreateAsync();

            var response = await client.CreateTenantAsync(
                new CreateTenantRequest(result.GetValue(name)!, result.GetValue(slug)!)
            );

            Console.WriteLine($"{response.Id} | {response.Name} | {response.Slug}");
        });

        return command;
    }

    private static Command GetTenant()
    {
        var command = new Command("get", "Get tenant.");

        var id = new Option<Guid>("--id") { Required = true };
        command.Options.Add(id);

        command.SetAction(async result =>
        {
            var client = await VaultSecretClientFactory.CreateAsync();
            var response = await client.GetTenantAsync(result.GetValue(id));

            Console.WriteLine(
                $"{response.Id} | {response.Name} | {response.Slug} | Active: {response.IsActive}"
            );
        });

        return command;
    }

    private static Command ListTenants()
    {
        var command = new Command("list", "List tenants.");

        command.SetAction(async _ =>
        {
            var client = await VaultSecretClientFactory.CreateAsync();
            var response = await client.ListTenantsAsync();

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
