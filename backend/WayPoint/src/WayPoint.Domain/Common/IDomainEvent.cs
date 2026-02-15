using MediatR;

namespace WayPoint.Domain.Common;

/// <summary>
/// Marker interface สำหรับ Domain Events ทุกตัว
/// Implement INotification ของ MediatR เพื่อให้ dispatch ผ่าน pipeline ได้
/// </summary>
public interface IDomainEvent : INotification
{
    /// <summary>Unique identifier ของ Event</summary>
    Guid EventId { get; }
    /// <summary>เวลาที่ Event เกิดขึ้น (UTC)</summary>
    DateTime OccurredAt { get; }
}