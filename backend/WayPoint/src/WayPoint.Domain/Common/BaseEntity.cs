namespace WayPoint.Domain.Common;

/// <summary>
/// Base class สำหรับทุก Entity ใน Domain
/// มี Id, CreatedAt, UpdatedAt และ Domain Events collection
/// </summary>
public abstract class BaseEntity
{
    private readonly List<IDomainEvent> _domainEvents = [];

    /// <summary>Primary key ของ Entity</summary>
    public Guid Id { get; protected set; } = Guid.NewGuid();

    /// <summary>เวลาที่สร้าง Entity (UTC)</summary>
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

    /// <summary>เวลาที่แก้ไขล่าสุด (UTC)</summary>
    public DateTime UpdatedAt { get; protected set; } = DateTime.UtcNow;

    /// <summary>รายการ Domain Events ที่ยังไม่ได้ dispatch</summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>เพิ่ม Domain Event เข้า queue</summary>
    protected void AddDomainEvent(IDomainEvent domainEvent) =>
        _domainEvents.Add(domainEvent);

    /// <summary>ล้าง Domain Events ทั้งหมด (เรียกหลัง dispatch แล้ว)</summary>
    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void SetUpdatedAt() => UpdatedAt = DateTime.UtcNow;
}