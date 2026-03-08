namespace WayPoint.Application.Common.Interfaces.Infrastructure;

/// <summary>Implement interface นี้ใน Query ที่ต้องการ Redis cache</summary>
public interface ICacheable
{
    string CacheKey { get; }
    TimeSpan? Expiry => TimeSpan.FromMinutes(5);
}
