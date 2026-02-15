using NetTopologySuite.Geometries;
using WayPoint.Domain.Common;
using WayPoint.Domain.ValueObjects;

namespace WayPoint.Domain.Events;

public record DeliveryCompletedEvent(
   Guid DeliveryId,
   Guid LocationId,
   Point ActualDeliveryCoordinate,
   GpsAccuracy GpsAccuracy
) : IDomainEvent
{
    public Guid EventId => Guid.NewGuid();
    public DateTime OccurredAt => DateTime.UtcNow;
}
