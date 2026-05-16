using System.Text;
using Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Api.Extensions;

public static class AuthExtensions
{
    public const string JwtScheme = JwtBearerDefaults.AuthenticationScheme;
    public const string ApiKeyScheme = ApiKeyAuthenticationHandler.SchemeName;
    public const string SmartScheme = "Smart";

    public static IServiceCollection AddVaultSecretAuth(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var jwtSection = configuration.GetSection("Jwt");

        var signingKey =
            jwtSection["SigningKey"]
            ?? throw new InvalidOperationException("JWT SigningKey is missing.");

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = SmartScheme;
                options.DefaultChallengeScheme = SmartScheme;
            })
            .AddPolicyScheme(
                SmartScheme,
                SmartScheme,
                options =>
                {
                    options.ForwardDefaultSelector = context =>
                    {
                        if (context.Request.Headers.ContainsKey("X-Api-Key"))
                        {
                            return ApiKeyScheme;
                        }

                        return JwtScheme;
                    };
                }
            )
            .AddJwtBearer(
                JwtScheme,
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtSection["Issuer"],
                        ValidAudience = jwtSection["Audience"],

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(signingKey)
                        ),
                    };
                }
            )
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(
                ApiKeyScheme,
                _ => { }
            );

        services.AddAuthorization();

        return services;
    }

    public static IServiceCollection AddVaultSecretSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(
                "v1",
                new OpenApiInfo { Title = "Emu VaultSecret API", Version = "v1" }
            );

            options.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Use: Bearer {token}",
                }
            );

            options.AddSecurityDefinition(
                "ApiKey",
                new OpenApiSecurityScheme
                {
                    Name = "X-Api-Key",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Description = "Use your VaultSecret API key",
                }
            );

            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        },
                        Array.Empty<string>()
                    },
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "ApiKey",
                            },
                        },
                        Array.Empty<string>()
                    },
                }
            );
        });

        return services;
    }
}
