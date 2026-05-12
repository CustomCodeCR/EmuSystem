using Domain.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal sealed class SecretConfiguration : IEntityTypeConfiguration<Secret>
{
    public void Configure(EntityTypeBuilder<Secret> builder)
    {
        builder.ToTable(nameof(Secret));

        builder.HasKey(x => x.SecretId);

        builder.Property(x => x.SecretId).ValueGeneratedNever();

        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();

        builder.Property(x => x.Path).HasMaxLength(500).IsRequired();

        builder.Property(x => x.CurrentVersionNumber).IsRequired();

        builder.Property(x => x.Status).HasConversion<short>().HasMaxLength(50).IsRequired();

        builder.Property(x => x.CreatedAt).IsRequired();

        builder.Property(x => x.UpdatedAt);

        builder
            .HasOne(x => x.Environment)
            .WithMany()
            .HasForeignKey(x => x.EnvironmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => new { x.EnvironmentId, x.Path })
            .IsUnique()
            .HasDatabaseName("IX_Secret_EnvironmentId_Path");

        builder.HasIndex(x => x.EnvironmentId).HasDatabaseName("IX_Secret_EnvironmentId");

        builder.HasIndex(x => x.Status).HasDatabaseName("IX_Secret_Status");
    }
}
