using Domain.ApiKeys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal sealed class ApiKeyConfiguration : IEntityTypeConfiguration<ApiKey>
{
    public void Configure(EntityTypeBuilder<ApiKey> builder)
    {
        builder.ToTable(nameof(ApiKey));

        builder.HasKey(x => x.ApiKeyId);

        builder.Property(x => x.ApiKeyId).ValueGeneratedNever();

        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();

        builder.Property(x => x.Description).HasMaxLength(500);

        builder.Property(x => x.KeyHash).HasMaxLength(500).IsRequired();

        builder.Property(x => x.KeyPrefix).HasMaxLength(50).IsRequired();

        builder.Property(x => x.IsActive).HasDefaultValue(true).IsRequired();

        builder.Property(x => x.CreatedAt).IsRequired();

        builder.Property(x => x.UpdatedAt);
        builder.Property(x => x.ExpiresAt);
        builder.Property(x => x.LastUsedAt);

        builder
            .HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.TenantId).HasDatabaseName("IX_ApiKey_TenantId");

        builder.HasIndex(x => x.KeyHash).IsUnique().HasDatabaseName("IX_ApiKey_KeyHash");

        builder.HasIndex(x => x.KeyPrefix).HasDatabaseName("IX_ApiKey_KeyPrefix");
    }
}
