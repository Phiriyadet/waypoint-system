namespace WayPoint.Application.Riders.DTOs;

/// <summary>ใช้ส่งผ่าน SignalR Hub ทุก 5 วินาที</summary>
public record RiderLocationDto(
    Guid RiderId,
    double Latitude,
    double Longitude,
    string Status,
    DateTime UpdatedAt
);
