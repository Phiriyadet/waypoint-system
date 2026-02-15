using WayPoint.Domain.Common;

namespace WayPoint.Domain.Entities;

/// <summary>
/// Entity แทนจุดหยุดแต่ละจุดในเส้นทาง
/// Managed ผ่าน Route aggregate root เท่านั้น
/// </summary>
public class RouteWaypoint : BaseEntity
{
    private RouteWaypoint() { }

    /// <summary>FK ไปยัง Route ที่ waypoint นี้สังกัด</summary>
    public Guid RouteId { get; private set; }

    /// <summary>FK ไปยัง Delivery ที่ต้องส่ง ณ จุดนี้</summary>
    public Guid DeliveryId { get; private set; }

    /// <summary>FK ไปยัง Location ปลายทาง</summary>
    public Guid LocationId { get; private set; }

    /// <summary>ลำดับที่ควรไป (1 = จุดแรก) เปลี่ยนได้เมื่อ Route.Optimize()</summary>
    public int Order { get; private set; }

    /// <summary>true เมื่อ Rider ถึงจุดนี้แล้ว</summary>
    public bool IsCompleted { get; private set; }

    /// <summary>เวลาที่ถึงจุดนี้ (UTC) null ถ้ายังไม่ถึง</summary>
    public DateTime? ArrivedAt { get; private set; }

    internal static RouteWaypoint Create(
        Guid routeId, Guid deliveryId, Guid locationId, int order)
        => new()
        {
            RouteId = routeId,
            DeliveryId = deliveryId,
            LocationId = locationId,
            Order = order
        };

    /// <summary>บันทึกว่า Rider ถึงจุดนี้แล้ว</summary>
    public void MarkArrived()
    {
        IsCompleted = true;
        ArrivedAt = DateTime.UtcNow;
        SetUpdatedAt();
    }

    /// <summary>เปลี่ยนลำดับ waypoint (เรียกจาก Route.ReorderWaypoints เท่านั้น)</summary>
    internal void SetOrder(int order)
    {
        if (order < 1) throw new ArgumentOutOfRangeException(nameof(order), "Order ต้องมากกว่า 0");
        Order = order;
    }
}