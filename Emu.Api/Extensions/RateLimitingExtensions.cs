using System.Threading.RateLimiting;

namespace Api.Extensions;

public static class RateLimitingExtensions
{
    public static IServiceCollection AddVaultSecretRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.AddPolicy(
                "api",
                context =>
                {
                    var actorId = context.User.FindFirst("actor_id")?.Value;

                    var partitionKey =
                        actorId ?? context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey,
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 120,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0,
                            AutoReplenishment = true,
                        }
                    );
                }
            );

            options.AddPolicy(
                "auth",
                context =>
                {
                    var partitionKey =
                        context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey,
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 10,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0,
                            AutoReplenishment = true,
                        }
                    );
                }
            );

            options.AddPolicy(
                "secrets-read",
                context =>
                {
                    var actorId =
                        context.User.FindFirst("actor_id")?.Value
                        ?? context.Connection.RemoteIpAddress?.ToString()
                        ?? "anonymous";

                    return RateLimitPartition.GetFixedWindowLimiter(
                        actorId,
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 300,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0,
                            AutoReplenishment = true,
                        }
                    );
                }
            );
        });

        return services;
    }
}
