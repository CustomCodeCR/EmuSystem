using Application.Abstractions.Auth;
using Application.Abstractions.Crypto;
using Application.Abstractions.Persistence;
using Infrastructure.Audit;
using Infrastructure.Auth;
using Infrastructure.Crypto;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Infrastructure.Persistence.Seed;
using Infrastructure.Repositories.AccessPolicies;
using Infrastructure.Repositories.ApiKeys;
using Infrastructure.Repositories.AuditLogs;
using Infrastructure.Repositories.Environments;
using Infrastructure.Repositories.Projects;
using Infrastructure.Repositories.Secrets;
using Infrastructure.Repositories.Tenants;
using Infrastructure.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<EncryptionOptions>(configuration.GetSection("Encryption"));

        services.Configure<ApiKeyOptions>(configuration.GetSection("ApiKeys"));

        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

        services.AddHttpContextAccessor();

        services.AddSingleton<
            Application.Abstractions.Time.ISystemClock,
            Infrastructure.Time.SystemClock
        >();

        services.AddSingleton<IMasterKeyProvider, MasterKeyProvider>();
        services.AddScoped<ISecretEncryptionService, SecretEncryptionService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddScoped<ICurrentActorService, CurrentActorService>();
        services.AddScoped<IApiKeyHasher, ApiKeyHasher>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IApiKeyGenerator, ApiKeyGenerator>();

        services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();

        services.AddScoped<IAuditWriter, AuditWriter>();

        services.AddScoped<ISecretRepository, SecretRepository>();
        services.AddScoped<IApiKeyRepository, ApiKeyRepository>();
        services.AddScoped<IAccessPolicyRepository, AccessPolicyRepository>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        services.AddScoped<IProjectEnvironmentRepository, ProjectEnvironmentRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<ISecretVersionRepository, SecretVersionRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<AuditableEntityInterceptor>();

        services.AddDbContext<ApplicationDbContext>(
            (serviceProvider, options) =>
            {
                var connectionString = configuration.GetConnectionString("Postgres");

                options.UseNpgsql(connectionString);
                options.UseSnakeCaseNamingConvention();
                options.AddInterceptors(
                    serviceProvider.GetRequiredService<AuditableEntityInterceptor>()
                );
            }
        );

        return services;
    }
}
