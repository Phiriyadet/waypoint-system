namespace WayPoint.Domain.Common;

/// <summary>
/// Marker interface สำหรับ Aggregate Root
/// Entity ที่ implement นี้คือจุดเดียวที่ภายนอกจะเข้าถึง aggregate ได้
/// ใช้คู่กับ BaseEntity เสมอ
/// </summary>
public interface IAggregateRoot { }