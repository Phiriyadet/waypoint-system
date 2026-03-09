using MediatR;
using WayPoint.Application.Common.Interfaces.Infrastructure;
using WayPoint.Application.Common.Interfaces.Repositories;
using WayPoint.Application.Common.Mappings;
using WayPoint.Domain.Events;

namespace WayPoint.Application.Routes.EventHandlers;

public class RouteOptimizedEventHandler
    : INotificationHandler<RouteOptimizedEvent>
{
    private readonly IRouteRepository _repo;
    private readonly ICacheService _cache;
    private readonly RouteMapper _mapper;

    public RouteOptimizedEventHandler(
        IRouteRepository repo,
        ICacheService cache,
        RouteMapper mapper)
    {
        _repo = repo;
        _cache = cache;
        _mapper = mapper;
    }

    public async Task Handle(RouteOptimizedEvent notification, CancellationToken ct)
    {
        var route = await _repo.GetByIdAsync(notification.RouteId, ct);
        if (route is null) return;

        // เก็บผลลัพธ์ ให้ query ถัดไปไวขึ้น
        var dto = _mapper.ToDto(route);
        await _cache.SetAsync(
            $"route:{notification.RouteId}",
            dto,
            TimeSpan.FromHours(2),
            ct);
    }
}
