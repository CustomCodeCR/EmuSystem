using Application.Abstractions.Auth;
using Application.Abstractions.Persistence;
using Application.Abstractions.Time;
using Application.Common;

namespace Application.Features.Users.Login;

public sealed class LoginHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ISystemClock _clock;

    public LoginHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService,
        ISystemClock clock
    )
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
        _clock = clock;
    }

    public async Task<LoginResponse> HandleAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var user = await _unitOfWork.Users.GetByEmailAsync(
            request.TenantId,
            email,
            cancellationToken
        );

        if (user is null || !user.IsActive)
        {
            throw new AppException("Invalid credentials.");
        }

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new AppException("Invalid credentials.");
        }

        user.LastLoginAt = _clock.UtcNow;
        _unitOfWork.Users.Update(user);

        var token = _jwtTokenService.CreateToken(user.UserId, user.TenantId, user.Email, ["Admin"]);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LoginResponse(token, user.UserId, user.TenantId, user.Email);
    }
}
