using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VaultSecret.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User));

        builder.HasKey(x => x.UserId);

        builder.Property(x => x.TenantId).IsRequired();

        builder.Property(x => x.Email).HasMaxLength(255).IsRequired();

        builder.Property(x => x.FullName).HasMaxLength(200).IsRequired();

        builder.Property(x => x.PasswordHash).HasMaxLength(500).IsRequired();

        builder.Property(x => x.IsActive).IsRequired();

        builder.Property(x => x.CreatedAt).IsRequired();

        builder.Property(x => x.UpdatedAt);

        builder.Property(x => x.LastLoginAt);

        builder.HasIndex(x => x.TenantId);

        builder.HasIndex(x => new { x.TenantId, x.Email }).IsUnique();
    }
}
