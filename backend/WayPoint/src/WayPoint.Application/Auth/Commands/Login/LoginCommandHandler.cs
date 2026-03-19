using MediatR;

using WayPoint.Application.Auth.DTOs;
using WayPoint.Application.Common.Interfaces.Auth;
using WayPoint.Domain.Common;

namespace WayPoint.Application.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponseDto>>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result<AuthResponseDto>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(
            request.Email,
            request.Password,
            cancellationToken);

        if (result.IsFailure)
            return Result<AuthResponseDto>.Failure(result.Error);

        return Result<AuthResponseDto>.Success(result.Value);
    }
}
