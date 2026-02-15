namespace WayPoint.Domain.ValueObjects;

/// <summary>
/// Value Object แทนคะแนนความเชื่อมั่นของตำแหน่ง (0–100)
/// เพิ่มขึ้นทุกครั้งที่จัดส่งสำเร็จ ลดลงเมื่อ GPS offset ผิดปกติ
/// ใช้ sealed record — equality เปรียบเทียบจาก Value อัตโนมัติ
/// </summary>
public sealed record ConfidenceScore
{
    /// <summary>ค่า score 0–100</summary>
    public int Value { get; }

    private ConfidenceScore(int value) => Value = Math.Clamp(value, 0, 100);

    /// <summary>ค่าเริ่มต้นเมื่อสร้าง Location ใหม่</summary>
    public static ConfidenceScore Initial() => new(10);

    /// <summary>สร้างจากค่าที่มีอยู่ (ใช้ตอน reconstitute จาก DB)</summary>
    public static ConfidenceScore FromValue(int value) => new(value);

    /// <summary>เพิ่มคะแนน (จำกัดสูงสุดที่ 100)</summary>
    public ConfidenceScore Increase(int amount) => new(Value + amount);

    /// <summary>ลดคะแนน (จำกัดต่ำสุดที่ 0)</summary>
    public ConfidenceScore Decrease(int amount) => new(Value - amount);

    /// <summary>true ถ้าตำแหน่งเชื่อถือได้สูง (> 60)</summary>
    public bool IsHighConfidence => Value > 60;

    /// <summary>true ถ้าตำแหน่งยังไม่ได้รับการยืนยัน (< 30)</summary>
    public bool IsLowConfidence => Value < 30;

    public override string ToString() => $"{Value}/100";
}