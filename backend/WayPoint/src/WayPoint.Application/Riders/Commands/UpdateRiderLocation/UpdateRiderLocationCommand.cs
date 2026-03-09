using MediatR;
using WayPoint.Domain.Common;

namespace WayPoint.Application.Riders.Commands.UpdateRiderLocation;

public record UpdateRiderLocationCommand(
    Guid RiderId,
    double Latitude,
    double Longitude
) : IRequest<Result<bool>>;
