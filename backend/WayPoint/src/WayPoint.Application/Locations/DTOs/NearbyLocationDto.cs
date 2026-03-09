namespace WayPoint.Application.Locations.DTOs;

public record NearbyLocationDto(
    Guid Id,
    string AddressInfo,
    string District,
    string Province,
    double Latitude,
    double Longitude,
    int ConfidenceScore,
    string? PlaceName,
    double DistanceMeters   // ระยะห่างจากจุดที่ค้นหา
);
