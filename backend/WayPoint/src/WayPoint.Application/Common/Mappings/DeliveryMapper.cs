using NetTopologySuite.Geometries;
using Riok.Mapperly.Abstractions;
using WayPoint.Application.Deliveries.DTOs;
using WayPoint.Domain.Entities;
using WayPoint.Domain.Enums;
using WayPoint.Domain.ValueObjects;

namespace WayPoint.Application.Common.Mappings;

[Mapper]
public partial class DeliveryMapper
{
    [MapProperty(nameof(Delivery.ActualDeliveryCoordinate), nameof(DeliveryDto.ActualLatitude))]
    [MapProperty(nameof(Delivery.ActualDeliveryCoordinate), nameof(DeliveryDto.ActualLongitude))]
    [MapProperty(nameof(Delivery.GpsAccuracy), nameof(DeliveryDto.GpsAccuracyMeters))]
    [MapProperty(nameof(Delivery.Status), nameof(DeliveryDto.Status))]

    public partial DeliveryDto ToDto(Delivery delivery);

    private static double? MapToActualLatitude(Point? point)
        => point?.Y;

    private static double? MapToActualLongitude(Point? point)
        => point?.X;

    private static double? MapToGpsAccuracyMeters(GpsAccuracy? accuracy)
        => accuracy?.Meters;

    private static string MapToStatus(DeliveryStatus status)
    => status.ToString();
}
