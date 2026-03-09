using MediatR;
using NetTopologySuite.Geometries;
using WayPoint.Application.Common.Interfaces.Repositories;
using WayPoint.Application.Locations.DTOs;
using WayPoint.Domain.Common;

namespace WayPoint.Application.Locations.Queries.GetNearbyLocations;

public class GetNearbyLocationsQueryHandler
    : IRequestHandler<GetNearbyLocationsQuery, Result<List<NearbyLocationDto>>>
{
    private readonly ILocationRepository _repo;

    public GetNearbyLocationsQueryHandler(ILocationRepository repo) => _repo = repo;

    public async Task<Result<List<NearbyLocationDto>>> Handle(
        GetNearbyLocationsQuery request, CancellationToken ct)
    {
        var origin = new Point(request.Longitude, request.Latitude) { SRID = 4326 };
        var locations = await _repo.GetNearbyAsync(origin, request.RadiusMeters, request.Limit, ct);

        var dtos = locations.Select(l => new NearbyLocationDto(
            l.Id,
            l.Address.AddressInfo,
            l.Address.District,
            l.Address.Province,
            l.Coordinate.Y,   // Latitude
            l.Coordinate.X,   // Longitude
            l.ConfidenceScore.Value,
            l.PlaceName,
            // Distance() คืน degree — × 111,000 เป็น meters (เทียบเคียง equator)
            l.Coordinate.Distance(origin) * 111_000
        )).ToList();

        return Result<List<NearbyLocationDto>>.Success(dtos);
    }
}
