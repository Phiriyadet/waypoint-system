using Riok.Mapperly.Abstractions;
using WayPoint.Application.Routes.DTOs;
using WayPoint.Domain.Entities;
using WayPoint.Domain.Enums;

namespace WayPoint.Application.Common.Mappings;

[Mapper]
public partial class RouteMapper
{
    [MapProperty(nameof(Route.Strategy), nameof(RouteDto.Strategy))]
    public partial RouteDto ToDto(Route route);
    public partial WaypointDto ToDto(RouteWaypoint waypoint);

    private static string MapToStrategy(RouteOptimizationStrategy strategy)
            => strategy.ToString();
}