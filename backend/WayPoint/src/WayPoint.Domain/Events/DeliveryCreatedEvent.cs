using WayPoint.Domain.Common;

namespace WayPoint.Domain.Events;

public record DeliveryCreatedEvent(
  Guid DeliveryId,
  Guid LocationId
) : IDomainEvent
{
  public Guid EventId => Guid.NewGuid();
  public DateTime OccurredAt => DateTime.UtcNow;
}
