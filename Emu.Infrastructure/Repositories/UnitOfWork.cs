using Application.Abstractions.Persistence;

namespace Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    public UnitOfWork(
        ApplicationDbContext dbContext,
        ITenantRepository tenants,
        IProjectRepository projects,
        IProjectEnvironmentRepository environments,
        ISecretRepository secrets,
        ISecretVersionRepository secretVersions,
        IApiKeyRepository apiKeys,
        IAccessPolicyRepository accessPolicies,
        IAuditLogRepository auditLogs,
        IUserRepository users
    )
    {
        _dbContext = dbContext;
        Tenants = tenants;
        Projects = projects;
        Environments = environments;
        Secrets = secrets;
        SecretVersions = secretVersions;
        ApiKeys = apiKeys;
        AccessPolicies = accessPolicies;
        AuditLogs = auditLogs;
        Users = users;
    }

    public ITenantRepository Tenants { get; }

    public IProjectRepository Projects { get; }

    public IProjectEnvironmentRepository Environments { get; }

    public ISecretRepository Secrets { get; }

    public ISecretVersionRepository SecretVersions { get; }

    public IApiKeyRepository ApiKeys { get; }

    public IAccessPolicyRepository AccessPolicies { get; }

    public IAuditLogRepository AuditLogs { get; }

    public IUserRepository Users { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
