using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.Scan(scan =>
            scan.FromAssemblyOf<ApplicationAssemblyMarker>()
                .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Handler")))
                .AsSelf()
                .WithScopedLifetime()
        );

        return services;
    }
}

public sealed class ApplicationAssemblyMarker;
