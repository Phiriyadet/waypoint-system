using NetTopologySuite.Geometries;
using WayPoint.Domain.Common;
using WayPoint.Domain.Enums;
using WayPoint.Domain.ValueObjects;

namespace WayPoint.Domain.Events;

public record LocationVerifiedEvent(
    Guid LocationId,
    Point ActualCoordinate,
    ConfidenceScore ConfidenceScore,
    LocationConfidenceLevel ConfidenceLevel
) : IDomainEvent
{
    public Guid EventId => Guid.NewGuid();
    public DateTime OccurredAt => DateTime.UtcNow;
}
