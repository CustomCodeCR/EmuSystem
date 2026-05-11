namespace Domain.AuditLogs;

public sealed class AuditLog
{
    public Guid AuditLogId { get; set; }
    public Guid TenantId { get; set; }
    public ActorType AutorType { get; set; }
    public Guid? ActorId { get; set; }
    public string Action { get; set; } = string.Empty;
    public ResourseType ResourseType { get; set; }
    public Guid? ResourseId { get; set; }
    public string? Path { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public Domain.Tenants.Tenant? Tenant { get; set; }
}
