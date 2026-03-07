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
        AddDomainEvent(new LocationVerifiedEvent(Id, VerifiedCoordinate, ConfidenceScore, ConfidenceLevel));
    }

    /// <summary>อัปเดต access notes จาก rider feedback หลังจัดส่ง</summary>
    public void UpdateAccessNotes(string notes)
    {
        AccessNotes = notes?.Trim();
        SetUpdatedAt();
    }

    /// <summary>
    /// เรียนรู้จาก GPS ที่ได้จริงขณะจัดส่ง และตัดสินใจว่าควรทำอะไร
    /// ✅ ทุก business logic และ calculation อยู่ใน Domain
    /// </summary>
    public LocationLearningResult LearnFromDelivery(Point actualCoordinate, GpsAccuracy accuracy)
    {
        ArgumentNullException.ThrowIfNull(actualCoordinate);
        ArgumentNullException.ThrowIfNull(accuracy);

        // ✅ คำนวณ offset ระหว่างพิกัดที่ geocode ไว้ vs GPS จริง
        var distanceDeg = Coordinate.Distance(actualCoordinate);
        var latRad = actualCoordinate.Y * Math.PI / 180.0;
        var offsetMeters = distanceDeg * 111_320 * Math.Cos(latRad);

        // ✅ Business rules: ตรวจสอบความแม่นยำและ offset
        var isHighAccuracy = accuracy.IsHighAccuracy; // < 20m
        var isSignificantOffset = offsetMeters > 50;

        if (isSignificantOffset && isHighAccuracy)
        {
            // GPS แม่นยำสูงและห่างจากที่ geocode มาก → ควรอัปเดต verified coordinate
            DeliveryCount++;
            SetUpdatedAt();

            return new LocationLearningResult
            {
                ShouldUpdateVerifiedCoordinate = true,
                NewCoordinate = actualCoordinate,
                OffsetMeters = offsetMeters
            };
        }
        else
        {
            // Offset น้อย หรือ GPS ไม่แม่นยำพอ → แค่เพิ่ม confidence score
            IncrementConfidenceScore();
            DeliveryCount++;
            SetUpdatedAt();

            return new LocationLearningResult
            {
                ShouldUpdateVerifiedCoordinate = false,
                NewCoordinate = actualCoordinate,
                OffsetMeters = offsetMeters
            };
        }
    }

    /// <summary>
    /// เพิ่ม confidence score ขึ้น 1 คะแนน (ใช้เมื่อ GPS ตรงกับ geocoded location)
    /// </summary>
    public void IncrementConfidenceScore()
    {
        ConfidenceScore = ConfidenceScore.Increase(1);
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

    /// <summary>
    /// Result object จาก Location.LearnFromDelivery()
    /// บอกว่า Handler ควรทำอะไรต่อ
    /// </summary>
    public class LocationLearningResult
    {
        public bool ShouldUpdateVerifiedCoordinate { get; init; }
        public Point NewCoordinate { get; init; } = null!;
        public double OffsetMeters { get; init; }
    }
}