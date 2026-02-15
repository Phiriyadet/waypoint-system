namespace WayPoint.Domain.ValueObjects;

/// <summary>
/// Value Object แทนความแม่นยำของสัญญาณ GPS (หน่วยเป็นเมตร)
/// ค่าน้อย = แม่นยำมาก, ค่ามาก = แม่นยำน้อย
/// ใช้ sealed record — equality เปรียบเทียบจาก Meters อัตโนมัติ
/// </summary>
public sealed record GpsAccuracy
{
    /// <summary>ความแม่นยำ GPS (meters) ต้องมากกว่า 0</summary>
    public double Meters { get; }

    private GpsAccuracy(double meters) => Meters = meters;

    /// <summary>สร้าง GpsAccuracy พร้อม validate ค่า</summary>
    public static GpsAccuracy Create(double meters)
    {
        if (meters <= 0)
            throw new ArgumentException("GPS accuracy must be greater than 0.", nameof(meters));
        return new GpsAccuracy(meters);
    }

    /// <summary>true ถ้า GPS แม่นยำสูง (< 10ม) — เชื่อถือได้สำหรับ location learning</summary>
    public bool IsHighAccuracy => Meters < 10;

    /// <summary>true ถ้า GPS แม่นยำต่ำ (> 50ม) — ไม่ควรใช้อัปเดต verified coordinate</summary>
    public bool IsLowAccuracy => Meters > 50;

    public override string ToString() => $"±{Meters:F1}m";
}