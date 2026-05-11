using Domain.Common;

namespace Domain.Secrets;

public sealed class SecretVersion : IAuditableEntity
{
    public Guid SecretVersionId { get; set; }
    public Guid SecretId { get; set; }
    public int VersionNumber { get; set; }
    public string EncryptedValue { get; set; } = string.Empty;
    public string Nonce { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public string Algorithm { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public Domain.Secrets.Secret? Secret { get; set; }
}
