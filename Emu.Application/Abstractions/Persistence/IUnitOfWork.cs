namespace Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    ITenantRepository Tenants { get; }

    IProjectRepository Projects { get; }

    IProjectEnvironmentRepository Environments { get; }

    ISecretRepository Secrets { get; }

    ISecretVersionRepository SecretVersions { get; }

    IApiKeyRepository ApiKeys { get; }

    IAccessPolicyRepository AccessPolicies { get; }

    IAuditLogRepository AuditLogs { get; }

    IUserRepository Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
