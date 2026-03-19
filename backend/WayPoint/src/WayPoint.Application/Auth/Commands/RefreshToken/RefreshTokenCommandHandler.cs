using MediatR;

using WayPoint.Application.Auth.DTOs;
using WayPoint.Application.Common.Interfaces.Auth;
using WayPoint.Domain.Common;

namespace WayPoint.Application.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<AuthResponseDto>>
{
    private readonly IAuthService _authService;

    public RefreshTokenCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result<AuthResponseDto>> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _authService.RefreshTokenAsync(
            request.RefreshToken,
            cancellationToken);

        if (result.IsFailure)
            return Result<AuthResponseDto>.Failure(result.Error);

        return Result<AuthResponseDto>.Success(result.Value);
    }
}
