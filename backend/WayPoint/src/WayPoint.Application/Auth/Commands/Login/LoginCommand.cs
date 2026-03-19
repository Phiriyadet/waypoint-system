using MediatR;

using WayPoint.Application.Auth.DTOs;
using WayPoint.Domain.Common;

namespace WayPoint.Application.Auth.Commands.Login;

public record LoginCommand(
    string Email,
    string Password
) : IRequest<Result<AuthResponseDto>>;
