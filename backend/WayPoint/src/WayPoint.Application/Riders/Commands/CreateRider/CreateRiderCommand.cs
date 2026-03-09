using MediatR;
using WayPoint.Application.Riders.DTOs;
using WayPoint.Domain.Common;

namespace WayPoint.Application.Riders.Commands.CreateRider;

public record CreateRiderCommand(
    string Name,
    string Phone,
    string VehicleType
) : IRequest<Result<RiderDto>>;
