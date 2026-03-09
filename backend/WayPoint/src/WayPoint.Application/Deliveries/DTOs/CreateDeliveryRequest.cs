namespace WayPoint.Application.Deliveries.DTOs;

public record CreateDeliveryRequest(
    Guid LocationId,
    string RecipientName,
    string RecipientPhone
);
