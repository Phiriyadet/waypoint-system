using WayPoint.Domain.Common;

namespace WayPoint.Domain.Events;

public record RouteOptimizedEvent(
  Guid RouteId,
  Guid RiderId,
  double TotalDistanceMeters,
  int EstimatedDurationSeconds,
  string Provider
) : IDomainEvent
{
  public Guid EventId => Guid.NewGuid();
  public DateTime OccurredAt => DateTime.UtcNow;
}
