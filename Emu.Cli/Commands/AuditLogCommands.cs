using System.CommandLine;

namespace Cli.Commands;

public static class AuditLogCommands
{
    public static Command Create()
    {
        var command = new Command("audit-log", "Manage audit logs.");

        command.Subcommands.Add(ListAuditLogs());

        return command;
    }

    private static Command ListAuditLogs()
    {
        var command = new Command("list", "List audit logs.");

        var tenantId = new Option<Guid>("--tenant-id") { Required = true };
        var page = new Option<int>("--page") { DefaultValueFactory = _ => 1 };
        var pageSize = new Option<int>("--page-size") { DefaultValueFactory = _ => 50 };

        command.Options.Add(tenantId);
        command.Options.Add(page);
        command.Options.Add(pageSize);

        command.SetAction(async result =>
        {
            var client = await VaultSecretClientFactory.CreateAsync();

            var response = await client.ListAuditLogsAsync(
                result.GetValue(tenantId),
                result.GetValue(page),
                result.GetValue(pageSize)
            );

            foreach (var item in response)
            {
                Console.WriteLine(
                    $"{item.CreatedAt:u} | {item.Action} | {item.ResourceType} | {item.Path}"
                );
            }
        });

        return command;
    }
}
