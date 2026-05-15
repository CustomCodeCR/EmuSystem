using System.CommandLine;
using Sdk.Requests;

namespace Cli.Commands;

public static class PolicyCommands
{
    public static Command Create()
    {
        var command = new Command("policy", "Manage access policies.");

        command.Subcommands.Add(CreatePolicy());
        command.Subcommands.Add(ListByApiKey());

        return command;
    }

    private static Command CreatePolicy()
    {
        var command = new Command("create", "Create access policy.");

        var apiKeyId = new Option<Guid>("--api-key-id") { Required = true };
        var tenantId = new Option<Guid>("--tenant-id") { Required = true };
        var projectId = new Option<Guid?>("--project-id");
        var environmentId = new Option<Guid?>("--environment-id");
        var pathPrefix = new Option<string>("--path-prefix") { Required = true };
        var canRead = new Option<bool>("--read");
        var canWrite = new Option<bool>("--write");
        var canDelete = new Option<bool>("--delete");

        command.Options.Add(apiKeyId);
        command.Options.Add(tenantId);
        command.Options.Add(projectId);
        command.Options.Add(environmentId);
        command.Options.Add(pathPrefix);
        command.Options.Add(canRead);
        command.Options.Add(canWrite);
        command.Options.Add(canDelete);

        command.SetAction(async result =>
        {
            var client = await VaultSecretClientFactory.CreateAsync();

            var response = await client.CreateAccessPolicyAsync(
                new CreateAccessPolicyRequest(
                    result.GetValue(apiKeyId),
                    result.GetValue(tenantId),
                    result.GetValue(projectId),
                    result.GetValue(environmentId),
                    result.GetValue(pathPrefix)!,
                    result.GetValue(canRead),
                    result.GetValue(canWrite),
                    result.GetValue(canDelete)
                )
            );

            Console.WriteLine($"{response.Id} | {response.PathPrefix}");
        });

        return command;
    }

    private static Command ListByApiKey()
    {
        var command = new Command("list", "List policies by API key.");

        var apiKeyId = new Option<Guid>("--api-key-id") { Required = true };
        command.Options.Add(apiKeyId);

        command.SetAction(async result =>
        {
            var client = await VaultSecretClientFactory.CreateAsync();
            var response = await client.ListAccessPoliciesByApiKeyAsync(result.GetValue(apiKeyId));

            foreach (var item in response)
            {
                Console.WriteLine(
                    $"{item.Id} | {item.PathPrefix} | R:{item.CanRead} W:{item.CanWrite} D:{item.CanDelete}"
                );
            }
        });

        return command;
    }
}
