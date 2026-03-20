using NetTopologySuite.Geometries;

using WayPoint.Domain.Common;
using WayPoint.Domain.Enums;
using WayPoint.Domain.Events;
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

    /// <summary>
    /// สร้าง Delivery ใหม่ โดยระบุ LocationId, RecipientName และ RecipientPhone
    /// </summary>
    /// <param name="locationId">ID ของ Location ที่ผู้รับอยู่</param>
    /// <param name="recipientName">ชื่อผู้รับพัสดุ</param>
    /// <param name="recipientPhone">เบอร์โทรผู้รับ format +66XXXXXXXXX</param>
    /// <returns>Delivery ที่ถูกสร้างใหม่</returns>
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

    /// <summary>
    /// มอบหมายไรเดอร์ให้กับ Delivery โดยตรวจสอบสถานะของไรเดอร์และสถานะการจัดส่ง
    /// </summary>
    /// <param name="riderId">ID ของไรเดอร์ที่จะมอบหมาย</param>
    /// <param name="rider">ออบเจ็กต์ไรเดอร์ที่จะมอบหมาย</param>
    /// <returns>ผลการดำเนินการ โดยมีข้อมูลว่าการมอบหมายสำเร็จหรือไม่ และเหตุผลถ้าไม่สำเร็จ</returns>
    public Result AssignRider(Guid riderId, Rider rider)
    {
        if (rider.Status != RiderStatus.Available)
            return Result.Failure("ไรเดอร์ไม่อยู่ในสถานะ Available");
        if (Status != DeliveryStatus.Pending)
            return Result.Failure($"ไม่สามารถ assign rider ได้เมื่อสถานะเป็น {Status}");

        RiderId = riderId;
        Status = DeliveryStatus.Assigned;

        _logs.Add(DeliveryLog.Create(Id, DeliveryEventType.Assigned));
        SetUpdatedAt();
        AddDomainEvent(new DeliveryAssignedEvent(Id, riderId));

        return Result.Success();
    }

    /// <summary>
    /// เริ่มการจัดส่ง โดยตรวจสอบสถานะของไรเดอร์และสถานะการจัดส่ง
    /// </summary>
    /// <param name="requestingRiderId">ID ของไรเดอร์ที่ขอเริ่มการจัดส่ง</param>
    /// <returns>ผลการดำเนินการ โดยมีข้อมูลว่าการเริ่มการจัดส่งสำเร็จหรือไม่ และเหตุผลถ้าไม่สำเร็จ</returns>
    public Result StartDelivery(Guid requestingRiderId)
    {
        if (RiderId != requestingRiderId)
            return Result.Failure("ไม่ใช่ไรเดอร์ที่รับผิดชอบ Delivery นี้");
        if (Status != DeliveryStatus.Assigned)
            return Result.Failure($"ไม่สามารถเริ่มจัดส่งได้เมื่อสถานะเป็น {Status}");

        Status = DeliveryStatus.InTransit;

        _logs.Add(DeliveryLog.Create(Id, DeliveryEventType.InTransit));
        SetUpdatedAt();
        AddDomainEvent(new DeliveryStartedEvent(Id, RiderId.Value));

        return Result.Success();
    }

    /// <summary>
    /// จัดส่งสำเร็จ โดยตรวจสอบสถานะของไรเดอร์และสถานะการจัดส่ง
    /// </summary>
    /// <param name="requestingRiderId">ID ของไรเดอร์ที่ขอจัดส่งสำเร็จ</param>
    /// <param name="actualCoordinate">พิกัด GPS จริง ณ จุดที่จัดส่งสำเร็จ</param>
    /// <param name="accuracy">ความแม่นยำ GPS ขณะจัดส่ง (meters)</param>
    /// <returns>ผลการดำเนินการ โดยมีข้อมูลว่าการจัดส่งสำเร็จหรือไม่ และเหตุผลถ้าไม่สำเร็จ</returns>
    public Result Complete(Guid requestingRiderId, Point actualCoordinate, GpsAccuracy accuracy)
    {
        if (RiderId != requestingRiderId)
            return Result.Failure("ไม่ใช่ไรเดอร์ที่รับผิดชอบ Delivery นี้");
        if (Status == DeliveryStatus.Completed)
            return Result.Failure($"Delivery with ID '{Id}' has already been completed.");
        if (Status != DeliveryStatus.InTransit)
            return Result.Failure("Delivery must be InTransit to complete");

        ArgumentNullException.ThrowIfNull(actualCoordinate);
        ArgumentNullException.ThrowIfNull(accuracy);

        ActualDeliveryCoordinate = actualCoordinate;
        GpsAccuracy = accuracy;
        Status = DeliveryStatus.Completed;
        CompletedAt = DateTime.UtcNow;

        _logs.Add(DeliveryLog.Create(Id, DeliveryEventType.Completed));
        SetUpdatedAt();
        AddDomainEvent(new DeliveryCompletedEvent(Id, LocationId, actualCoordinate, accuracy));

        return Result.Success();
    }

    /// <summary>
    /// ยกเลิกการจัดส่ง โดยตรวจสอบสถานะของ Delivery และเพิ่มเหตุผลในการยกเลิก
    /// </summary>
    /// <param name="reason">เหตุผลที่ทำการยกเลิก</param>
    public void Cancel(string reason)
    {
        if (Status == DeliveryStatus.Completed)
            throw new InvalidOperationException("ไม่สามารถยกเลิก Delivery ที่ส่งแล้ว");

        Status = DeliveryStatus.Cancelled;
        _logs.Add(DeliveryLog.Create(Id, DeliveryEventType.Cancelled, reason));
        SetUpdatedAt();
    }

    /// <summary>
    /// ตรวจสอบรหัสการจัดส่งว่าถูกต้องหรือไม่
    /// </summary>
    /// <param name="code">รหัสการจัดส่งที่จะตรวจสอบ</param>
    /// <returns>ผลการตรวจสอบ โดยมีข้อมูลว่ารหัสถูกต้องหรือไม่</returns>
    public bool VerifyDeliveryCode(string code) => DeliveryCode == code?.Trim();

    private static string GenerateDeliveryCode() =>
        Random.Shared.Next(100_000, 999_999).ToString();
}
