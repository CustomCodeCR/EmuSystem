using Application.Abstractions.Persistence;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
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
        var connectionString = configuration.GetConnectionString("postgres");

        services.AddHttpContextAccessor();

        services.AddSingleton<
            Application.Abstractions.Time.ISystemClock,
            Infrastructure.Time.SystemClock
        >();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<AuditableEntityInterceptor>();

        services.AddDbContext<ApplicationDbContext>(
            (serviceProvider, options) =>
            {
                options.UseNpgsql(connectionString);
                options.UseUpperCaseNamingConvention();
                options.AddInterceptors(
                    serviceProvider.GetRequiredService<AuditableEntityInterceptor>()
                );
            }
        );

        return services;
    }
}
