using MediatR;

using WayPoint.Domain.Common;

namespace WayPoint.Application.Auth.Commands.Logout;

public record LogoutCommand(
    string RefreshToken
) : IRequest<Result>;
