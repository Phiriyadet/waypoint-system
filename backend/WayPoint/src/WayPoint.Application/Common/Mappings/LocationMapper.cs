using NetTopologySuite.Geometries;
using Riok.Mapperly.Abstractions;
using WayPoint.Application.Locations.DTOs;
using WayPoint.Domain.Enums;
using WayPoint.Domain.ValueObjects;
using Location = WayPoint.Domain.Entities.Location;

namespace WayPoint.Application.Common.Mappings;

[Mapper]
public partial class LocationMapper
{
    [MapProperty(nameof(Location.Coordinate), nameof(LocationDto.Latitude))]
    [MapProperty(nameof(Location.Coordinate), nameof(LocationDto.Longitude))]

    [MapProperty(nameof(Location.VerifiedCoordinate), nameof(LocationDto.VerifiedLatitude))]
    [MapProperty(nameof(Location.VerifiedCoordinate), nameof(LocationDto.VerifiedLongitude))]

    [MapProperty(nameof(Location.Address), nameof(LocationDto.AddressInfo))]
    [MapProperty(nameof(Location.Address), nameof(LocationDto.Subdistrict))]
    [MapProperty(nameof(Location.Address), nameof(LocationDto.District))]
    [MapProperty(nameof(Location.Address), nameof(LocationDto.Province))]
    [MapProperty(nameof(Location.Address), nameof(LocationDto.PostalCode))]
    [MapProperty(nameof(Location.Address), nameof(LocationDto.MoreInfo))]

    [MapProperty(nameof(Location.ConfidenceScore), nameof(LocationDto.ConfidenceScore))]
    [MapProperty(nameof(Location.ConfidenceLevel), nameof(LocationDto.ConfidenceLevel))]

    public partial LocationDto ToDto(Location location);

    // ---------- Coordinate ----------

    private static double MapToLatitude(Point point)
        => point.Y;

    private static double MapToLongitude(Point point)
        => point.X;

    private static double? MapToVerifiedLatitude(Point? point)
        => point?.Y;

    private static double? MapToVerifiedLongitude(Point? point)
        => point?.X;

    // ---------- Address ValueObject ----------

    private static string MapToAddressInfo(Address address)
        => address.AddressInfo;

    private static string MapToSubdistrict(Address address)
        => address.Subdistrict;

    private static string MapToDistrict(Address address)
        => address.District;

    private static string MapToProvince(Address address)
        => address.Province;

    private static string MapToPostalCode(Address address)
        => address.PostalCode;

    private static string? MapToMoreInfo(Address address)
        => address.MoreInfo;

    // ---------- ValueObjects ----------

    private static int MapToConfidenceScore(ConfidenceScore score)
        => score.Value;

    private static string MapToConfidenceLevel(LocationConfidenceLevel level)
        => level.ToString();
}
