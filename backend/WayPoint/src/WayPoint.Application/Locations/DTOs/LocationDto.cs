namespace WayPoint.Application.Locations.DTOs;

public record LocationDto(
    Guid Id,
    string AddressInfo,
    string Subdistrict,
    string District,
    string Province,
    string PostalCode,
    string? MoreInfo,
    double Latitude,
    double Longitude,
    double? VerifiedLatitude,
    double? VerifiedLongitude,
    int ConfidenceScore,
    string ConfidenceLevel,   // Low / Medium / High / Verified
    int DeliveryCount,
    string? PlaceName,
    string? AccessNotes,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
