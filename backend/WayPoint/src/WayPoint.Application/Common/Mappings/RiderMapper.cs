using Riok.Mapperly.Abstractions;
using WayPoint.Application.Riders.DTOs;
using WayPoint.Domain.Entities;
using WayPoint.Domain.Enums;

namespace WayPoint.Application.Common.Mappings;

[Mapper]
public partial class RiderMapper
{
    [MapProperty(nameof(Rider.VehicleType), nameof(RiderDto.VehicleType))]
    [MapProperty(nameof(Rider.Status), nameof(RiderDto.Status))]
    [MapProperty(nameof(Rider.CurrentLocation), nameof(RiderDto.CurrentLatitude))]
    [MapProperty(nameof(Rider.CurrentLocation), nameof(RiderDto.CurrentLongitude))]

    public partial RiderDto ToDto(Rider rider);

    private static string MapToVehicleType(VehicleType vehicleType)
        => vehicleType.ToString();

    private static string MapToStatus(RiderStatus status)
        => status.ToString();

    private static double? MapToCurrentLatitude(NetTopologySuite.Geometries.Point? point)
        => point?.Y;

    private static double? MapToCurrentLongitude(NetTopologySuite.Geometries.Point? point)
        => point?.X;
}
