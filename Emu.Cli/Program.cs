using System.CommandLine;
using Cli.Commands;

var root = new RootCommand("CustomCodeCR Emu System CLI");

root.Subcommands.Add(LoginCommand.Create());
root.Subcommands.Add(TenantCommands.Create());
root.Subcommands.Add(ProjectCommands.Create());
root.Subcommands.Add(EnvironmentCommands.Create());
root.Subcommands.Add(ApiKeyCommands.Create());
root.Subcommands.Add(PolicyCommands.Create());
root.Subcommands.Add(SecretCommands.Create());
root.Subcommands.Add(UserCommands.Create());
root.Subcommands.Add(AuditLogCommands.Create());

return await root.Parse(args).InvokeAsync();
