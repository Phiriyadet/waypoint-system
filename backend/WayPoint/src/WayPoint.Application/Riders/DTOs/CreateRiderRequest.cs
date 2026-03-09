namespace WayPoint.Application.Riders.DTOs;

public record CreateRiderRequest(
    string Name,
    string Phone,
    string VehicleType
);
