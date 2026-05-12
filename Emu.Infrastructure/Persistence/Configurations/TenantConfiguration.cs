using Domain.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal sealed class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable(nameof(Tenant));

        builder.HasKey(x => x.TenantId);

        builder.Property(x => x.TenantId).ValueGeneratedNever();

        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();

        builder.Property(x => x.Slug).HasMaxLength(120).IsRequired();

        builder.Property(x => x.IsActive).HasDefaultValue(true).IsRequired();

        builder.Property(x => x.CreatedAt).IsRequired();

        builder.Property(x => x.UpdatedAt);

        builder.HasIndex(x => x.Slug).IsUnique().HasDatabaseName("IX_Tenant_Slug");

        builder.HasIndex(x => x.IsActive).HasDatabaseName("IX_Tenant_IsActive");
    }
}
