using WayPoint.Application.Riders.DTOs;

namespace WayPoint.Application.Common.Interfaces.ExternalServices;

/// <summary>
/// Abstraction สำหรับ broadcast ตำแหน่ง Rider —
/// ให้ Application ไม่ต้องรู้จัก SignalR Hub โดยตรง
/// </summary>
public interface IRiderLocationBroadcaster
{
    Task BroadcastAsync(RiderLocationDto dto, CancellationToken ct = default);
}
