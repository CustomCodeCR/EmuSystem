namespace Domain.AuditLogs;

public enum ResourseType : short
{
    Tenant = 1,
    Project = 2,
    Environment = 3,
    Secret = 4,
    SecretVersion = 5,
    ApiKey = 6,
    AccessPolicy = 7,
}
