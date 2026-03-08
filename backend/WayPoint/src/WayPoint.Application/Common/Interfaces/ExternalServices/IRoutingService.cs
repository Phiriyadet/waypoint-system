using NetTopologySuite.Geometries;

namespace WayPoint.Application.Common.Interfaces.ExternalServices;

public record RouteResult(
    double TotalDistanceMeters,
    int EstimatedDurationSeconds,
    string Provider,
    IReadOnlyList<Guid> OptimizedDeliveryOrder
);

public interface IRoutingService
{
    /// <summary>คำนวณเส้นทางที่ดีที่สุดจากจุดเริ่มต้นผ่านทุก waypoint</summary>
    Task<RouteResult> OptimizeRouteAsync(
        Point origin,
        IReadOnlyList<(Guid DeliveryId, Point Destination)> waypoints,
        CancellationToken ct = default);
}
