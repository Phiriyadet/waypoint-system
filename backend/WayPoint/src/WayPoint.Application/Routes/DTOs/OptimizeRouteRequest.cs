namespace WayPoint.Application.Routes.DTOs;

public record OptimizeRouteRequest(
    Guid RiderId,
    List<Guid> DeliveryIds,
    string Strategy = "Shortest"
);
