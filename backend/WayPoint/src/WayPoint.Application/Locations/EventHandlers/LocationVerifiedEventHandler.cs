using MediatR;
using WayPoint.Application.Common.Interfaces.Infrastructure;
using WayPoint.Domain.Events;

namespace WayPoint.Application.Locations.EventHandlers;

public class LocationVerifiedEventHandler
    : INotificationHandler<LocationVerifiedEvent>
{
    private readonly ICacheService _cache;

    public LocationVerifiedEventHandler(ICacheService cache) => _cache = cache;

    public async Task Handle(LocationVerifiedEvent notification, CancellationToken ct)
    {
        // Invalidate cache เมื่อ location ถูกอัปเดต
        await _cache.RemoveAsync($"location:{notification.LocationId}", ct);
    }
}
