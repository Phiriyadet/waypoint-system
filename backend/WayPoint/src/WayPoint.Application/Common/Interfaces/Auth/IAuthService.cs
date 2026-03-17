using WayPoint.Application.Auth.DTOs;
using WayPoint.Domain.Common;

namespace WayPoint.Application.Common.Interfaces.Auth;

public interface IAuthService
{
    Task<Result<AuthResponseDto>> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);
    Task<Result<AuthResponseDto>> RefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default);
    Task LogoutAsync(
        string refreshToken,
        CancellationToken cancellationToken = default);
}
