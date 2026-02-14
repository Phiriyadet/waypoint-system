namespace WayPoint.Domain.Common;

/// <summary>
/// Marker interface to identify Aggregate Roots in DDD
/// Only aggregate roots should have repositories
/// </summary>
public interface IAggregateRoot
{
    // Empty interface - used only as a marker
}