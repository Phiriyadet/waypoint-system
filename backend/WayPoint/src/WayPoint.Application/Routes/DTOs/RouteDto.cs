namespace WayPoint.Application.Routes.DTOs;

public record RouteDto(
    Guid Id,
    Guid RiderId,
    string Strategy,
    double TotalDistanceMeters,
    int EstimatedDurationSeconds,
    string Provider,
    DateTime OptimizedAt,
    List<WaypointDto> Waypoints
);
