using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions;

public static class DatabaseExtensions
{
    public static async Task ApplyDatabaseMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var logger = scope
            .ServiceProvider.GetRequiredService<ILoggerFactory>()
            .CreateLogger("DatabaseMigration");

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        logger.LogInformation("Applying database migrations...");

        await dbContext.Database.MigrateAsync();

        logger.LogInformation("Database migrations applied successfully.");
    }

    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var logger = scope
            .ServiceProvider.GetRequiredService<ILoggerFactory>()
            .CreateLogger("DatabaseSeeder");

        var seeder = scope.ServiceProvider.GetRequiredService<IDatabaseSeeder>();

        logger.LogInformation("Seeding database...");

        await seeder.SeedAsync();

        logger.LogInformation("Database seeded successfully.");
    }

    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        await app.ApplyDatabaseMigrationsAsync();
        await app.SeedDatabaseAsync();
    }
}
