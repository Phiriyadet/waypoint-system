using MediatR;
using NetTopologySuite.Geometries;
using WayPoint.Application.Common.Interfaces.ExternalServices;
using WayPoint.Application.Common.Interfaces.Repositories;
using WayPoint.Application.Riders.DTOs;
using WayPoint.Domain.Common;
using WayPoint.Domain.Exceptions;

namespace WayPoint.Application.Riders.Commands.UpdateRiderLocation;

public class UpdateRiderLocationCommandHandler
    : IRequestHandler<UpdateRiderLocationCommand, Result<bool>>
{
    private readonly IRiderRepository _repo;
    private readonly IRiderLocationBroadcaster _broadcaster; // ← interface ไม่ใช้ IHubContext

    public UpdateRiderLocationCommandHandler(
        IRiderRepository repo,
        IRiderLocationBroadcaster broadcaster)
    {
        _repo = repo;
        _broadcaster = broadcaster;
    }

    public async Task<Result<bool>> Handle(
        UpdateRiderLocationCommand request, CancellationToken ct)
    {
        var rider = await _repo.GetByIdAsync(request.RiderId, ct)
            ?? throw new RiderNotFoundException(request.RiderId);

        var point = new Point(request.Longitude, request.Latitude) { SRID = 4326 };
        rider.UpdateLocation(point, DateTime.UtcNow);

        await _repo.SaveChangesAsync(ct);

        var dto = new RiderLocationDto(
            rider.Id,
            request.Latitude,
            request.Longitude,
            rider.Status.ToString(),
            rider.LastLocationUpdate ?? DateTime.UtcNow);

        // Broadcast ผ่าน abstraction — Infrastructure implement SignalR
        await _broadcaster.BroadcastAsync(dto, ct);

        return Result<bool>.Success(true);
    }
}
