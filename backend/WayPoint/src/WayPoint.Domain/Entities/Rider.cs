using NetTopologySuite.Geometries;
using WayPoint.Domain.Common;
using WayPoint.Domain.Enums;

namespace WayPoint.Domain.Entities;

/// <summary>
/// Entity แทน Rider (พนักงานส่งพัสดุ)
/// เก็บสถานะการทำงานและตำแหน่ง GPS ปัจจุบัน
/// </summary>
public class Rider : BaseEntity
{
    private Rider() { }

    /// <summary>ชื่อ-นามสกุล Rider</summary>
    public string Name { get; private set; } = null!;

    /// <summary>เบอร์โทร format +66XXXXXXXXX ใช้ login</summary>
    public string Phone { get; private set; } = null!;

    /// <summary>ประเภทยานพาหนะ (enum) - Motorcycle, Car, Bicycle, ElectricScooter</summary>
    public VehicleType VehicleType { get; private set; }

    /// <summary>สถานะการทำงานปัจจุบัน</summary>
    public RiderStatus Status { get; private set; }

    /// <summary>ตำแหน่ง GPS ล่าสุดที่ส่งมาจาก mobile app (SRID 4326)</summary>
    public Point? CurrentLocation { get; private set; }

    /// <summary>เวลาที่รับตำแหน่ง GPS ล่าสุด (UTC)</summary>
    public DateTime? LastLocationUpdate { get; private set; }

    /// <summary>สร้าง Rider ใหม่ สถานะเริ่มต้น Offline</summary>
    public static Rider Create(string name, string phone, VehicleType vehicleType)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.");
        if (string.IsNullOrWhiteSpace(phone)) throw new ArgumentException("Phone is required.");
        return new Rider
        {
            Name = name.Trim(),
            Phone = phone.Trim(),
            VehicleType = vehicleType,
            Status = RiderStatus.Offline
        };
    }

    /// <summary>อัปเดตตำแหน่ง GPS ปัจจุบัน — เรียกจาก mobile ทุก 5 วินาทีผ่าน SignalR</summary>
    public void UpdateLocation(Point location, DateTime timestamp)
    {
        ArgumentNullException.ThrowIfNull(location);
        CurrentLocation = location;
        LastLocationUpdate = timestamp;
        SetUpdatedAt();
    }

    /// <summary>เปลี่ยนสถานะการทำงาน — Offline ไม่สามารถไป Busy โดยตรง</summary>
    public void ChangeStatus(RiderStatus status)
    {
        if (Status == RiderStatus.Offline && status == RiderStatus.Busy)
            throw new InvalidOperationException("Rider ต้องเปลี่ยนเป็น Available ก่อนรับงาน");
        Status = status;
        SetUpdatedAt();
    }

    /// <summary>true ถ้า Rider พร้อมรับงาน</summary>
    public bool IsAvailable => Status == RiderStatus.Available;
}