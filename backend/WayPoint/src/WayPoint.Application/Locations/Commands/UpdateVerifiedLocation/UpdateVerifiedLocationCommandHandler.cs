using MediatR;
using NetTopologySuite.Geometries;
using WayPoint.Application.Common.Interfaces.Repositories;
using WayPoint.Domain.Common;
using WayPoint.Domain.Exceptions;

namespace WayPoint.Application.Locations.Commands.UpdateVerifiedLocation;

public class UpdateVerifiedLocationCommandHandler
    : IRequestHandler<UpdateVerifiedLocationCommand, Result<bool>>
{
    private readonly ILocationRepository _repo;

    public UpdateVerifiedLocationCommandHandler(ILocationRepository repo)
        => _repo = repo;

    public async Task<Result<bool>> Handle(
        UpdateVerifiedLocationCommand request, CancellationToken ct)
    {
        var location = await _repo.GetByIdAsync(request.LocationId, ct)
            ?? throw new LocationNotFoundException(request.LocationId);

        var point = new Point(request.Longitude, request.Latitude) { SRID = 4326 };
        location.UpdateVerifiedCoordinate(point, request.OffsetMeters);

        await _repo.SaveChangesAsync(ct);
        return Result<bool>.Success(true);
    }
}
