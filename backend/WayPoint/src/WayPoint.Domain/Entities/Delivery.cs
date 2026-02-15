using NetTopologySuite.Geometries;
using WayPoint.Domain.Common;
using WayPoint.Domain.Enums;
using WayPoint.Domain.Events;
using WayPoint.Domain.Exceptions;
using WayPoint.Domain.ValueObjects;

namespace WayPoint.Domain.Entities;

/// <summary>
/// Aggregate Root ของ Delivery
/// ทุก state change ต้องผ่าน method ของ class นี้เท่านั้น
/// </summary>
public class Delivery : BaseEntity, IAggregateRoot
{
    private readonly List<DeliveryLog> _logs = [];
    private Delivery() { }

    /// <summary>FK ไปยัง Location ของผู้รับ</summary>
    public Guid LocationId { get; private set; }

    /// <summary>FK ไปยัง Rider ที่รับผิดชอบ (null ถ้ายังไม่ assign)</summary>
    public Guid? RiderId { get; private set; }

    /// <summary>ชื่อผู้รับพัสดุ</summary>
    public string RecipientName { get; private set; } = null!;

    /// <summary>เบอร์โทรผู้รับ format +66XXXXXXXXX</summary>
    public string RecipientPhone { get; private set; } = null!;

    /// <summary>สถานะปัจจุบันของการจัดส่ง</summary>
    public DeliveryStatus Status { get; private set; }

    /// <summary>รหัสยืนยันรับพัสดุ (ตัวเลข 6 หลัก)</summary>
    public string DeliveryCode { get; private set; } = null!;

    /// <summary>พิกัด GPS จริง ณ จุดที่จัดส่งสำเร็จ</summary>
    public Point? ActualDeliveryCoordinate { get; private set; }

    /// <summary>ความแม่นยำ GPS ขณะจัดส่ง (meters)</summary>
    public GpsAccuracy? GpsAccuracy { get; private set; }

    /// <summary>เวลาจัดส่งสำเร็จ (UTC)</summary>
    public DateTime? CompletedAt { get; private set; }

    /// <summary>ประวัติการเปลี่ยนสถานะ — append-only</summary>
    public IReadOnlyCollection<DeliveryLog> Logs => _logs.AsReadOnly();

    public static Delivery Create(Guid locationId, string recipientName, string recipientPhone)
    {
        if (string.IsNullOrWhiteSpace(recipientName))
            throw new ArgumentException("RecipientName is required.");
        if (string.IsNullOrWhiteSpace(recipientPhone))
            throw new ArgumentException("RecipientPhone is required.");
        var delivery = new Delivery
        {
            LocationId = locationId,
            RecipientName = recipientName.Trim(),
            RecipientPhone = recipientPhone.Trim(),
            Status = DeliveryStatus.Pending,
            DeliveryCode = GenerateDeliveryCode()
        };
        delivery._logs.Add(DeliveryLog.Create(delivery.Id, DeliveryEventType.Created));
        delivery.AddDomainEvent(new DeliveryCreatedEvent(delivery.Id, locationId));
        return delivery;
    }

    /// <summary>มอบหมาย Rider เปลี่ยนสถานะเป็น Assigned</summary>
    public void AssignRider(Guid riderId)
    {
        if (Status != DeliveryStatus.Pending)
            throw new InvalidOperationException($"ไม่สามารถ assign rider ได้เมื่อสถานะเป็น {Status}");
        RiderId = riderId;
        Status = DeliveryStatus.Assigned;
        _logs.Add(DeliveryLog.Create(Id, DeliveryEventType.Assigned));
        SetUpdatedAt();
    }

    /// <summary>บันทึกว่า Rider กำลังเดินทางไปส่ง</summary>
    public void StartDelivery()
    {
        if (Status != DeliveryStatus.Assigned)
            throw new InvalidOperationException($"ไม่สามารถเริ่มจัดส่งได้เมื่อสถานะเป็น {Status}");
        Status = DeliveryStatus.InTransit;
        _logs.Add(DeliveryLog.Create(Id, DeliveryEventType.InTransit));
        SetUpdatedAt();
    }

    /// <summary>
    /// บันทึกการจัดส่งสำเร็จพร้อม GPS จริง
    /// Raise DeliveryCompletedEvent เพื่อ trigger LocationLearningService
    /// </summary>
    public void Complete(Point actualCoordinate, GpsAccuracy accuracy)
    {
        if (Status == DeliveryStatus.Completed)
            throw new DeliveryAlreadyCompletedException(Id);
        ArgumentNullException.ThrowIfNull(actualCoordinate);
        ArgumentNullException.ThrowIfNull(accuracy);
        ActualDeliveryCoordinate = actualCoordinate;
        GpsAccuracy = accuracy;
        Status = DeliveryStatus.Completed;
        CompletedAt = DateTime.UtcNow;
        _logs.Add(DeliveryLog.Create(Id, DeliveryEventType.Completed));
        SetUpdatedAt();
        AddDomainEvent(new DeliveryCompletedEvent(Id, LocationId, actualCoordinate, accuracy));
    }

    /// <summary>ยกเลิก Delivery พร้อมระบุเหตุผล</summary>
    public void Cancel(string reason)
    {
        if (Status == DeliveryStatus.Completed)
            throw new InvalidOperationException("ไม่สามารถยกเลิก Delivery ที่ส่งแล้ว");
        Status = DeliveryStatus.Cancelled;
        _logs.Add(DeliveryLog.Create(Id, DeliveryEventType.Cancelled, reason));
        SetUpdatedAt();
    }

    /// <summary>ตรวจสอบ delivery code ที่ผู้รับกรอกยืนยัน</summary>
    public bool VerifyDeliveryCode(string code) => DeliveryCode == code?.Trim();

    private static string GenerateDeliveryCode() =>
        Random.Shared.Next(100_000, 999_999).ToString();
}