using WayPoint.Domain.Common;
using WayPoint.Domain.Enums;
using WayPoint.Domain.Events;

namespace WayPoint.Domain.Entities;

/// <summary>
/// Aggregate Root ของ RouteWaypoint
/// เส้นทางที่คำนวณแล้วสำหรับ Rider 1 คนใน 1 รอบ
/// </summary>
public class Route : BaseEntity, IAggregateRoot
{
    private readonly List<RouteWaypoint> _waypoints = [];
    private Route() { }

    /// <summary>FK ไปยัง Rider ที่ใช้เส้นทางนี้</summary>
    public Guid RiderId { get; private set; }

    /// <summary>ระยะทางรวมทั้งเส้นทาง (เมตร)</summary>
    public double TotalDistanceMeters { get; private set; }

    /// <summary>เวลาโดยประมาณ (วินาที)</summary>
    public int EstimatedDurationSeconds { get; private set; }

    /// <summary>strategy ที่ใช้คำนวณเส้นทางนี้</summary>
    public RouteOptimizationStrategy Strategy { get; private set; }

    /// <summary>ชื่อ routing provider เช่น "ORS" หรือ "OSRM"</summary>
    public string Provider { get; private set; } = null!;

    /// <summary>เวลาที่คำนวณเส้นทาง (UTC)</summary>
    public DateTime OptimizedAt { get; private set; }

    /// <summary>รายการ waypoints เรียงตามลำดับที่ควรไป</summary>
    public IReadOnlyCollection<RouteWaypoint> Waypoints => _waypoints.AsReadOnly();

    /// <summary>สร้าง Route เปล่าสำหรับ Rider</summary>
    public static Route Create(Guid riderId, RouteOptimizationStrategy strategy)
        => new()
        {
            RiderId = riderId,
            Strategy = strategy,
            Provider = string.Empty,
            OptimizedAt = DateTime.UtcNow
        };

    /// <summary>เพิ่ม waypoint (จุดส่งพัสดุ) เข้าเส้นทาง</summary>
    public void AddWaypoint(Guid deliveryId, Guid locationId)
    {
        var order = _waypoints.Count + 1;
        _waypoints.Add(RouteWaypoint.Create(Id, deliveryId, locationId, order));
        SetUpdatedAt();
    }

    /// <summary>
    /// บันทึกผลการคำนวณเส้นทางจาก routing provider
    /// Raise RouteOptimizedEvent เพื่อ cache invalidation
    /// </summary>
    public void Optimize(double totalDistanceMeters, int estimatedDurationSeconds,
        string provider, IReadOnlyList<Guid> orderedDeliveryIds)
    {
        if (_waypoints.Count == 0)
            throw new InvalidOperationException("ไม่สามารถ optimize route ที่ไม่มี waypoint");
        TotalDistanceMeters = totalDistanceMeters;
        EstimatedDurationSeconds = estimatedDurationSeconds;
        Provider = provider;
        OptimizedAt = DateTime.UtcNow;
        ReorderWaypoints(orderedDeliveryIds);
        SetUpdatedAt();
        AddDomainEvent(new RouteOptimizedEvent(
            Id, RiderId, totalDistanceMeters, estimatedDurationSeconds, provider));
    }

    /// <summary>true ถ้าเส้นทางนี้คำนวณมานานเกิน threshold ควร recalculate</summary>
    public bool IsStale(TimeSpan threshold) => DateTime.UtcNow - OptimizedAt > threshold;

    private void ReorderWaypoints(IReadOnlyList<Guid> orderedDeliveryIds)
    {
        for (int i = 0; i < orderedDeliveryIds.Count; i++)
            _waypoints.FirstOrDefault(w => w.DeliveryId == orderedDeliveryIds[i])
                      ?.SetOrder(i + 1);
        _waypoints.Sort((a, b) => a.Order.CompareTo(b.Order));
    }
}