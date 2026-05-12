using Domain.Environments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal sealed class ProjectEnvironmentConfiguration : IEntityTypeConfiguration<ProjectEnvironment>
{
    public void Configure(EntityTypeBuilder<ProjectEnvironment> builder)
    {
        builder.ToTable(nameof(ProjectEnvironment));

        builder.HasKey(x => x.EnvironmentId);

        builder.Property(x => x.EnvironmentId).ValueGeneratedNever();

        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();

        builder.Property(x => x.Slug).HasMaxLength(120).IsRequired();

        builder.Property(x => x.IsActive).HasDefaultValue(true).IsRequired();

        builder.Property(x => x.CreatedAt).IsRequired();

        builder.Property(x => x.UpdatedAt);

        builder
            .HasOne(x => x.Project)
            .WithMany()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => new { x.ProjectId, x.Slug })
            .IsUnique()
            .HasDatabaseName("IX_ProjectEnvironment_ProjectId_Slug");

        builder.HasIndex(x => x.ProjectId).HasDatabaseName("IX_ProjectEnvironment_ProjectId");
    }
}
