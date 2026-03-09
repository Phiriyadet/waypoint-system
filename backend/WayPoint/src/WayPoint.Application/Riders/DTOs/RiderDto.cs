namespace WayPoint.Application.Riders.DTOs;

public record RiderDto(
    Guid Id,
    string Name,
    string Phone,
    string VehicleType,
    string Status,
    double? CurrentLatitude,
    double? CurrentLongitude,
    DateTime? LastLocationUpdate,
    DateTime CreatedAt
);
