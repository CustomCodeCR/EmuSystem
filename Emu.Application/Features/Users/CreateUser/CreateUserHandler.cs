using Application.Abstractions.Auth;
using Application.Abstractions.Persistence;
using Application.Abstractions.Time;
using Application.Common;
using Domain.Users;

namespace Application.Features.Users.CreateUser;

public sealed class CreateUserHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ISystemClock _clock;

    public CreateUserHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        ISystemClock clock
    )
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _clock = clock;
    }

    public async Task<CreateUserResponse> HandleAsync(
        CreateUserRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var tenant = await _unitOfWork.Tenants.GetByIdAsync(request.TenantId, cancellationToken);

        if (tenant is null)
        {
            throw new AppException("Tenant not found.");
        }

        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var existing = await _unitOfWork.Users.GetByEmailAsync(
            request.TenantId,
            normalizedEmail,
            cancellationToken
        );

        if (existing is not null)
        {
            throw new AppException("User email already exists.");
        }

        var user = new User
        {
            UserId = Guid.NewGuid(),
            TenantId = request.TenantId,
            Email = normalizedEmail,
            FullName = request.FullName.Trim(),
            PasswordHash = _passwordHasher.Hash(request.Password),
            IsActive = true,
            CreatedAt = _clock.UtcNow,
        };

        await _unitOfWork.Users.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateUserResponse(user.UserId, user.TenantId, user.Email, user.FullName);
    }
}
