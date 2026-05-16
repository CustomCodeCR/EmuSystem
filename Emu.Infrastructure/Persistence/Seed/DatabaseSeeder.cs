using Application.Abstractions.Auth;
using Application.Abstractions.Time;
using Domain.Tenants;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Seed;

public sealed class DatabaseSeeder : IDatabaseSeeder
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ISystemClock _clock;

    public DatabaseSeeder(
        ApplicationDbContext dbContext,
        IPasswordHasher passwordHasher,
        ISystemClock clock
    )
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _clock = clock;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        const string tenantSlug = "customcodecr";
        const string adminEmail = "admin@customcodecr.com";

        var tenant = await _dbContext.Tenants.FirstOrDefaultAsync(
            x => x.Slug == tenantSlug,
            cancellationToken
        );

        if (tenant is null)
        {
            tenant = new Tenant
            {
                TenantId = Guid.NewGuid(),
                Name = "CustomCodeCR",
                Slug = tenantSlug,
                IsActive = true,
                CreatedAt = _clock.UtcNow,
            };

            await _dbContext.Tenants.AddAsync(tenant, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        var adminExists = await _dbContext.Users.AnyAsync(
            x => x.TenantId == tenant.TenantId && x.Email == adminEmail,
            cancellationToken
        );

        if (!adminExists)
        {
            var admin = new User
            {
                UserId = Guid.NewGuid(),
                TenantId = tenant.TenantId,
                Email = adminEmail,
                FullName = "CustomCodeCR Admin",
                PasswordHash = _passwordHasher.Hash("Admin123!"),
                IsActive = true,
                CreatedAt = _clock.UtcNow,
            };

            await _dbContext.Users.AddAsync(admin, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
