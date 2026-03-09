namespace WayPoint.Application.Deliveries.DTOs;

public record CompleteDeliveryRequest(
    double Latitude,
    double Longitude,
    double AccuracyMeters
);
