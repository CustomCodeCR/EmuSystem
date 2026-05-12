using Domain.AccessPolicies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal sealed class AccessPolicyConfiguration : IEntityTypeConfiguration<AccessPolicy>
{
    public void Configure(EntityTypeBuilder<AccessPolicy> builder)
    {
        builder.ToTable(nameof(AccessPolicy));

        builder.HasKey(x => x.AccessPolicyId);

        builder.Property(x => x.AccessPolicyId).ValueGeneratedNever();

        builder.Property(x => x.PathPrefix).HasMaxLength(500).IsRequired();

        builder.Property(x => x.CanRead).IsRequired();
        builder.Property(x => x.CanWrite).IsRequired();
        builder.Property(x => x.CanDelete).IsRequired();

        builder.Property(x => x.IsActive).HasDefaultValue(true).IsRequired();

        builder.Property(x => x.CreatedAt).IsRequired();

        builder.Property(x => x.UpdatedAt);

        builder
            .HasOne(x => x.ApiKey)
            .WithMany()
            .HasForeignKey(x => x.ApiKeyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Project)
            .WithMany()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Environment)
            .WithMany()
            .HasForeignKey(x => x.EnvironmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.ApiKeyId).HasDatabaseName("IX_AccessPolicy_ApiKeyId");

        builder.HasIndex(x => x.TenantId).HasDatabaseName("IX_AccessPolicy_TenantId");

        builder
            .HasIndex(x => new { x.ApiKeyId, x.PathPrefix })
            .HasDatabaseName("IX_AccessPolicy_ApiKeyId_PathPrefix");

        builder.HasIndex(x => x.IsActive).HasDatabaseName("IX_AccessPolicy_IsActive");
    }
}
