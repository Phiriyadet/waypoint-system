using MediatR;
using WayPoint.Application.Riders.DTOs;
using WayPoint.Domain.Common;

namespace WayPoint.Application.Riders.Commands.UpdateRiderStatus;

public record UpdateRiderStatusCommand(Guid RiderId, string Status) : IRequest<Result<RiderDto>>;
