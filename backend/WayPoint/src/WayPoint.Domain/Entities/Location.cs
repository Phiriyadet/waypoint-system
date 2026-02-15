using NetTopologySuite.Geometries;
using WayPoint.Domain.Common;
using WayPoint.Domain.Enums;
using WayPoint.Domain.Events;
using WayPoint.Domain.ValueObjects;

namespace WayPoint.Domain.Entities;

/// <summary>
/// Core GIS entity เก็บตำแหน่งที่อยู่ผู้รับพัสดุ
/// confidence_score เพิ่มขึ้นทุกครั้งที่จัดส่งสำเร็จและ GPS ตรงกัน
/// </summary>
public class Location : BaseEntity
{
    private Location() { } // EF Core

    /// <summary>ที่อยู่แบบ structured</summary>
    public Address Address { get; private set; } = null!;

    /// <summary>พิกัด GPS เดิมที่ได้จากการ geocode (SRID 4326)</summary>
    public Point Coordinate { get; private set; } = null!;

    /// <summary>พิกัดที่ได้รับการยืนยันจาก GPS จริงขณะจัดส่ง</summary>
    public Point? VerifiedCoordinate { get; private set; }

    /// <summary>คะแนนความเชื่อมั่นของตำแหน่ง 0–100</summary>
    public ConfidenceScore ConfidenceScore { get; private set; } = null!;

    /// <summary>ระดับความเชื่อมั่นของตำแหน่ง (Low, Medium, High, Verified)</summary>
    public LocationConfidenceLevel ConfidenceLevel => GetConfidenceLevel();

    /// <summary>จำนวนครั้งที่จัดส่งสำเร็จ (ใช้คำนวณ score)</summary>
    public int DeliveryCount { get; private set; }

    /// <summary>ชื่อสถานที่ ช่วยให้ rider หาง่ายขึ้น</summary>
    public string? PlaceName { get; private set; }

    /// <summary>หมายเหตุการเข้าถึง เช่น "เข้าซอยซ้าย ตึกสีแดง"</summary>
    public string? AccessNotes { get; private set; }

    public static Location Create(
        Address address, Point coordinate,
        string? placeName = null, string? accessNotes = null)
    {
        ArgumentNullException.ThrowIfNull(address);
        ArgumentNullException.ThrowIfNull(coordinate);
        return new Location
        {
            Address = address,
            Coordinate = coordinate,
            ConfidenceScore = ConfidenceScore.Initial(),
            PlaceName = placeName,
            AccessNotes = accessNotes
        };
    }

    /// <summary>
    /// อัปเดตตำแหน่งที่ยืนยันแล้วจาก GPS จริง และปรับ confidence score
    /// เรียกจาก LocationLearningService เมื่อ delivery complete
    /// </summary>
    public void UpdateVerifiedCoordinate(Point actualCoordinate, double offsetMeters)
    {
        ArgumentNullException.ThrowIfNull(actualCoordinate);
        VerifiedCoordinate = actualCoordinate;
        DeliveryCount++;
        ConfidenceScore = offsetMeters > 50
            ? ConfidenceScore.Increase(10)
            : ConfidenceScore.Increase(1);
        SetUpdatedAt();
        AddDomainEvent(new LocationVerifiedEvent(Id, actualCoordinate, ConfidenceScore, ConfidenceLevel));
    }

    /// <summary>อัปเดต access notes จาก rider feedback หลังจัดส่ง</summary>
    public void UpdateAccessNotes(string notes)
    {
        AccessNotes = notes?.Trim();
        SetUpdatedAt();
    }

    /// <summary>
    /// เปรียบเทียบ confidence score กับเกณฑ์ที่กำหนดเพื่อให้ระดับความเชื่อมั่น
    /// </summary>
    /// <returns>
    /// ระดับความเชื่อมั่นของตำแหน่ง: Low (0–30), Medium (31–60), High (61–90), Verified (91–100)
    /// </returns>
    private LocationConfidenceLevel GetConfidenceLevel()
    {
        return ConfidenceScore.Value switch
        {
            <= 30 => LocationConfidenceLevel.Low,
            <= 60 => LocationConfidenceLevel.Medium,
            <= 90 => LocationConfidenceLevel.High,
            _ => LocationConfidenceLevel.Verified
        };
    }
}