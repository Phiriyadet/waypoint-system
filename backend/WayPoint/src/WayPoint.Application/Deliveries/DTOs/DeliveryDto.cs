namespace WayPoint.Application.Deliveries.DTOs;

public record DeliveryDto(
    Guid Id,
    string DeliveryCode,
    Guid LocationId,
    string RecipientName,
    string RecipientPhone,
    string Status,
    Guid? RiderId,
    double? ActualLatitude,
    double? ActualLongitude,
    double? GpsAccuracyMeters,
    DateTime? CompletedAt,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
