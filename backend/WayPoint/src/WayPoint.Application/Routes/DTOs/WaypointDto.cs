namespace WayPoint.Application.Routes.DTOs;

public record WaypointDto(
    Guid Id,
    Guid DeliveryId,
    Guid LocationId,
    int Order,
    bool IsCompleted,
    DateTime? ArrivedAt
);
