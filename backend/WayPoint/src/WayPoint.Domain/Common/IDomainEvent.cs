namespace WayPoint.Domain.Common;

/// <summary>
/// Base interface for all domain events
/// Domain events represent something that happened in the domain
/// that domain experts care about
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// When the event occurred
    /// </summary>
    DateTime OccurredOn { get; }
    
    /// <summary>
    /// Unique identifier for this event instance
    /// </summary>
    Guid EventId { get; }
}