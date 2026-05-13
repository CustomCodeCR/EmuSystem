using Domain.AuditLogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal sealed class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable(nameof(AuditLog));

        builder.HasKey(x => x.AuditLogId);

        builder.Property(x => x.AuditLogId).ValueGeneratedNever();

        builder.Property(x => x.ActorType).HasConversion<short>().HasMaxLength(50).IsRequired();

        builder.Property(x => x.Action).HasMaxLength(200).IsRequired();

        builder.Property(x => x.ResourceType).HasConversion<short>().HasMaxLength(50).IsRequired();

        builder.Property(x => x.Path).HasMaxLength(500);

        builder.Property(x => x.IpAddress).HasMaxLength(100);

        builder.Property(x => x.UserAgent).HasMaxLength(1000);

        builder.Property(x => x.CreatedAt).IsRequired();

        builder
            .HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.TenantId).HasDatabaseName("IX_AuditLog_TenantId");

        builder.HasIndex(x => x.CreatedAt).HasDatabaseName("IX_AuditLog_CreatedAt");

        builder
            .HasIndex(x => new { x.TenantId, x.CreatedAt })
            .HasDatabaseName("IX_AuditLog_TenantId_CreatedAt");

        builder
            .HasIndex(x => new { x.ResourceType, x.ResourceId })
            .HasDatabaseName("IX_AuditLog_ResourseType_ResourseId");
    }
}
