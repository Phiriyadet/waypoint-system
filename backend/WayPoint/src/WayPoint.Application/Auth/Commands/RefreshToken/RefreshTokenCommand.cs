using MediatR;

using WayPoint.Application.Auth.DTOs;
using WayPoint.Domain.Common;

namespace WayPoint.Application.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(
    string RefreshToken
) : IRequest<Result<AuthResponseDto>>;
