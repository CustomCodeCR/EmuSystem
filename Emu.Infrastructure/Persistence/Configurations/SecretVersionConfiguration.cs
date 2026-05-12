using Domain.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal sealed class SecretVersionConfiguration : IEntityTypeConfiguration<SecretVersion>
{
    public void Configure(EntityTypeBuilder<SecretVersion> builder)
    {
        builder.ToTable(nameof(SecretVersion));

        builder.HasKey(x => x.SecretVersionId);

        builder.Property(x => x.SecretVersionId).ValueGeneratedNever();

        builder.Property(x => x.VersionNumber).IsRequired();

        builder.Property(x => x.EncryptedValue).IsRequired();

        builder.Property(x => x.Nonce).HasMaxLength(500).IsRequired();

        builder.Property(x => x.Tag).HasMaxLength(500).IsRequired();

        builder.Property(x => x.Algorithm).HasMaxLength(100).IsRequired();

        builder.Property(x => x.IsActive).HasDefaultValue(true).IsRequired();

        builder.Property(x => x.CreatedAt).IsRequired();

        builder.Property(x => x.UpdatedAt);

        builder
            .HasOne(x => x.Secret)
            .WithMany()
            .HasForeignKey(x => x.SecretId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(x => new { x.SecretId, x.VersionNumber })
            .IsUnique()
            .HasDatabaseName("IX_SecretVersion_SecretId_VersionNumber");

        builder.HasIndex(x => x.SecretId).HasDatabaseName("IX_SecretVersion_SecretId");
    }
}
