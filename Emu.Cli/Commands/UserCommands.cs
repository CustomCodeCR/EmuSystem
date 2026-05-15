using System.CommandLine;
using Cli.Config;
using Sdk.Requests;

namespace Cli.Commands;

public static class UserCommands
{
    public static Command Create()
    {
        var command = new Command("user", "Manage users.");

        command.Subcommands.Add(CreateUser());
        command.Subcommands.Add(ListUsers());
        command.Subcommands.Add(LoginUser());

        return command;
    }

    private static Command CreateUser()
    {
        var command = new Command("create", "Create user.");

        var tenantId = new Option<Guid>("--tenant-id") { Required = true };
        var email = new Option<string>("--email") { Required = true };
        var fullName = new Option<string>("--full-name") { Required = true };
        var password = new Option<string>("--password") { Required = true };

        command.Options.Add(tenantId);
        command.Options.Add(email);
        command.Options.Add(fullName);
        command.Options.Add(password);

        command.SetAction(async result =>
        {
            var client = await VaultSecretClientFactory.CreateAsync();

            var response = await client.CreateUserAsync(
                new CreateUserRequest(
                    result.GetValue(tenantId),
                    result.GetValue(email)!,
                    result.GetValue(fullName)!,
                    result.GetValue(password)!
                )
            );

            Console.WriteLine($"{response.Id} | {response.Email} | {response.FullName}");
        });

        return command;
    }

    private static Command ListUsers()
    {
        var command = new Command("list", "List users.");

        var tenantId = new Option<Guid>("--tenant-id") { Required = true };
        command.Options.Add(tenantId);

        command.SetAction(async result =>
        {
            var client = await VaultSecretClientFactory.CreateAsync();
            var response = await client.ListUsersAsync(result.GetValue(tenantId));

            foreach (var item in response)
            {
                Console.WriteLine(
                    $"{item.Id} | {item.Email} | {item.FullName} | Active: {item.IsActive}"
                );
            }
        });

        return command;
    }

    private static Command LoginUser()
    {
        var command = new Command("login", "Login user and save JWT token.");

        var baseUrl = new Option<string>("--base-url") { Required = true };
        var tenantId = new Option<Guid>("--tenant-id") { Required = true };
        var email = new Option<string>("--email") { Required = true };
        var password = new Option<string>("--password") { Required = true };

        command.Options.Add(baseUrl);
        command.Options.Add(tenantId);
        command.Options.Add(email);
        command.Options.Add(password);

        command.SetAction(async result =>
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(result.GetValue(baseUrl)!.TrimEnd('/')),
            };

            var client = new Sdk.VaultSecretClient(httpClient);

            var response = await client.LoginAsync(
                new LoginRequest(
                    result.GetValue(tenantId),
                    result.GetValue(email)!,
                    result.GetValue(password)!
                )
            );

            await new VaultSecretCliConfigStore().SaveAsync(
                new VaultSecretCliConfig
                {
                    BaseUrl = result.GetValue(baseUrl)!.TrimEnd('/'),
                    AccessToken = response.AccessToken,
                }
            );

            Console.WriteLine("Logged in successfully.");
        });

        return command;
    }
}
