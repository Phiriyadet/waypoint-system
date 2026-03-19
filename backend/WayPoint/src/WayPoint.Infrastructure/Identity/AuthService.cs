using Microsoft.AspNetCore.Identity;

using WayPoint.Application.Auth.DTOs;
using WayPoint.Application.Common.Interfaces.Auth;
using WayPoint.Application.Common.Interfaces.Repositories;
using WayPoint.Domain.Common;
using WayPoint.Infrastructure.Identity;

namespace WayPoint.Domain.Entities;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepo;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        ITokenService tokenService,
        IRefreshTokenRepository refreshTokenRepo)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _refreshTokenRepo = refreshTokenRepo;
    }

    public async Task<Result<AuthResponseDto>> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return Result<AuthResponseDto>.Failure("Invalid email or password");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
        if (!isPasswordValid)
            return Result<AuthResponseDto>.Failure("Invalid email or password");

        var roles = await _userManager.GetRolesAsync(user);

        var accessToken = await _tokenService.GenerateAccessTokenAsync(
            user.Id,
            user.FullName,
            user.Email!,
            roles,
            user.RiderId);

        var refreshToken = _tokenService.GenerateRefreshToken();

        // Store refresh token
        await _refreshTokenRepo.AddAsync(new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        }, cancellationToken);

        return Result<AuthResponseDto>.Success(new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            UserId = user.Id,
            Email = user.Email!,
            FullName = user.FullName,
            Roles = roles.ToList(),
            RiderId = user.RiderId
        });
    }

    public async Task<Result<AuthResponseDto>> RefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        var storedToken = await _refreshTokenRepo.GetByTokenAsync(refreshToken, cancellationToken);
        if (storedToken is null || storedToken.IsRevoked || storedToken.ExpiresAt < DateTime.UtcNow)
            return Result<AuthResponseDto>.Failure("Invalid or expired refresh token");

        var user = await _userManager.FindByIdAsync(storedToken.UserId);
        if (user is null)
            return Result<AuthResponseDto>.Failure("User not found");

        var roles = await _userManager.GetRolesAsync(user);

        var newAccessToken = await _tokenService.GenerateAccessTokenAsync(
            user.Id,
            user.FullName,
            user.Email!,
            roles,
            user.RiderId);

        var newRefreshToken = _tokenService.GenerateRefreshToken();

        // Revoke old token
        await _refreshTokenRepo.RevokeAsync(refreshToken, "Rotated", cancellationToken);

        // Store new token
        await _refreshTokenRepo.AddAsync(new RefreshToken
        {
            UserId = user.Id,
            Token = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        }, cancellationToken);

        return Result<AuthResponseDto>.Success(new AuthResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            UserId = user.Id,
            Email = user.Email!,
            FullName = user.FullName,
            Roles = roles.ToList(),
            RiderId = user.RiderId
        });
    }

    public async Task LogoutAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        await _refreshTokenRepo.RevokeAsync(refreshToken, "Logout", cancellationToken);
    }
}
