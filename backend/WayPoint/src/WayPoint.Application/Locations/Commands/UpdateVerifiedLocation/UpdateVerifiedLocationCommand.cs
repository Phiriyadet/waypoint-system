using MediatR;
using WayPoint.Domain.Common;

namespace WayPoint.Application.Locations.Commands.UpdateVerifiedLocation;

public record UpdateVerifiedLocationCommand(
    Guid LocationId,
    double Latitude,
    double Longitude,
    double OffsetMeters
) : IRequest<Result<bool>>;