namespace WayPoint.Application.Locations.DTOs;

public record CreateLocationRequest(
    string AddressInfo,
    string Subdistrict,
    string District,
    string Province,
    string PostalCode,
    string? MoreInfo,
    string? PlaceName,
    string? AccessNotes
);
