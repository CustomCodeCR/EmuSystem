namespace Domain.AuditLogs;

public enum AutorType : short
{
    User = 1,
    ApiKey = 2,
    System = 3,
    Agent = 4,
    Scheduler = 5,
}
