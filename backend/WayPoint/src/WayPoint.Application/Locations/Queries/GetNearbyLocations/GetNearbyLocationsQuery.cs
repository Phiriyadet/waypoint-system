using MediatR;
using WayPoint.Application.Locations.DTOs;
using WayPoint.Domain.Common;

namespace WayPoint.Application.Locations.Queries.GetNearbyLocations;

public record GetNearbyLocationsQuery(
    double Latitude,
    double Longitude,
    double RadiusMeters,
    int Limit = 20
) : IRequest<Result<List<NearbyLocationDto>>>;
