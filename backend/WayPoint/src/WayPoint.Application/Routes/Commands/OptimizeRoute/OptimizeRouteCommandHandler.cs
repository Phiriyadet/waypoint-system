using MediatR;
using WayPoint.Application.Common.Interfaces.ExternalServices;
using WayPoint.Application.Common.Interfaces.Repositories;
using WayPoint.Application.Common.Mappings;
using WayPoint.Application.Routes.DTOs;
using WayPoint.Domain.Common;
using WayPoint.Domain.Entities;
using WayPoint.Domain.Enums;
using WayPoint.Domain.Exceptions;

namespace WayPoint.Application.Routes.Commands.OptimizeRoute;

public class OptimizeRouteCommandHandler
    : IRequestHandler<OptimizeRouteCommand, Result<RouteDto>>
{
    private readonly IRouteRepository _routeRepo;
    private readonly IRiderRepository _riderRepo;
    private readonly IDeliveryRepository _deliveryRepo;
    private readonly ILocationRepository _locationRepo;
    private readonly IRoutingService _routing;
    private readonly RouteMapper _mapper;

    public OptimizeRouteCommandHandler(
        IRouteRepository routeRepo,
        IRiderRepository riderRepo,
        IDeliveryRepository deliveryRepo,
        ILocationRepository locationRepo,
        IRoutingService routing,
        RouteMapper mapper)
    {
        _routeRepo = routeRepo;
        _riderRepo = riderRepo;
        _deliveryRepo = deliveryRepo;
        _locationRepo = locationRepo;
        _routing = routing;
        _mapper = mapper;
    }

    public async Task<Result<RouteDto>> Handle(
        OptimizeRouteCommand request, CancellationToken ct)
    {
        var rider = await _riderRepo.GetByIdAsync(request.RiderId, ct)
            ?? throw new RiderNotFoundException(request.RiderId);

        if (rider.CurrentLocation is null)
            return Result<RouteDto>.Failure("ไรเดอร์ยังไม่ได้อัปเดตตำแหน่ง GPS");

        // =========================
        // 1️⃣ Load waypoint data
        // =========================
        var waypointData = new List<(Guid DeliveryId, Guid LocationId, NetTopologySuite.Geometries.Point Destination)>();

        foreach (var deliveryId in request.DeliveryIds)
        {
            var delivery = await _deliveryRepo.GetByIdAsync(deliveryId, ct)
                ?? throw new DeliveryNotFoundException(deliveryId);

            var location = await _locationRepo.GetByIdAsync(delivery.LocationId, ct)
                ?? throw new LocationNotFoundException(delivery.LocationId);

            var coord =
                location.ConfidenceScore.IsHighConfidence && location.VerifiedCoordinate is not null
                    ? location.VerifiedCoordinate
                    : location.Coordinate;

            waypointData.Add((deliveryId, location.Id, coord));
        }

        // ✅ Senior improvement (O(1) lookup)
        var waypointLookup = waypointData.ToDictionary(w => w.DeliveryId);

        // =========================
        // 2️⃣ Call routing engine
        // =========================
        var strategy = Enum.Parse<RouteOptimizationStrategy>(request.Strategy);

        var routingWaypoints = waypointData
            .Select(w => (w.DeliveryId, w.Destination))
            .ToList();

        var result = await _routing.OptimizeRouteAsync(
            rider.CurrentLocation,
            routingWaypoints,
            ct);

        // =========================
        // 3️⃣ Create aggregate
        // =========================
        var route = Route.Create(request.RiderId, strategy);

        // เพิ่ม waypoint ทั้งหมดก่อน (unordered)
        foreach (var w in waypointData)
        {
            route.AddWaypoint(w.DeliveryId, w.LocationId);
        }

        // =========================
        // 4️⃣ Domain applies optimization (DDD ✔)
        // =========================
        route.Optimize(
            result.TotalDistanceMeters,
            result.EstimatedDurationSeconds,
            result.Provider,
            result.OptimizedDeliveryOrder);

        // =========================
        // 5️⃣ Persist
        // =========================
        await _routeRepo.AddAsync(route, ct);
        await _routeRepo.SaveChangesAsync(ct);

        return Result<RouteDto>.Success(_mapper.ToDto(route));
    }
}