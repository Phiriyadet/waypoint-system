using WayPoint.Domain.Common;
using WayPoint.Domain.Enums;

namespace WayPoint.Domain.Entities;

/// <summary>
/// Audit trail ของ Delivery — บันทึกทุก event ที่เกิดกับ Delivery
/// Append-only — ไม่มี update method
/// Managed ผ่าน Delivery aggregate root เท่านั้น
/// </summary>
public class DeliveryLog : BaseEntity
{
    private DeliveryLog() { }

    /// <summary>FK ไปยัง Delivery ที่ log นี้สังกัด</summary>
    public Guid DeliveryId { get; private set; }

    /// <summary>ประเภท event ที่เกิดขึ้น</summary>
    public DeliveryEventType EventType { get; private set; }

    /// <summary>เวลาที่ event เกิดขึ้น (UTC)</summary>
    public DateTime OccurredAt { get; private set; }

    /// <summary>หมายเหตุเพิ่มเติม เช่น "ผู้รับไม่อยู่บ้าน" (optional)</summary>
    public string? Note { get; private set; }

    /// <summary>สร้าง log entry ใหม่ — เรียกจาก Delivery methods เท่านั้น</summary>
    internal static DeliveryLog Create(
        Guid deliveryId, DeliveryEventType eventType, string? note = null)
        => new()
        {
            DeliveryId = deliveryId,
            EventType = eventType,
            OccurredAt = DateTime.UtcNow,
            Note = note
        };
}