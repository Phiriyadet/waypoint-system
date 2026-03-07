using WayPoint.Domain.Common;

namespace WayPoint.Domain.Events;

public record DeliveryAssignedEvent(
    Guid DeliveryId,
    Guid RiderId
) : IDomainEvent
{
    public Guid EventId => Guid.NewGuid();
    public DateTime OccurredAt => DateTime.UtcNow;
}
