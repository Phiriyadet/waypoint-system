using NetTopologySuite.Geometries;

using Polly;

using WayPoint.Application.Common.Interfaces.ExternalServices;

namespace WayPoint.Infrastructure.Services.Routing;

public class RoutingServiceFactory : IRoutingService
{
    private readonly OpenRouteService _orsService;
    private readonly OsrmRoutingService _osrmService;

    public RoutingServiceFactory(
        OpenRouteService orsService,
        OsrmRoutingService osrmService)
    {
        _orsService = orsService;
        _osrmService = osrmService;
    }

    public async Task<RouteResult> OptimizeRouteAsync(
        Point origin,
        IReadOnlyList<(Guid DeliveryId, Point Destination)> waypoints,
        CancellationToken ct = default)
    {
        // Define fallback policy: ORS → OSRM
        var fallbackPolicy = Policy<RouteResult>
            .Handle<HttpRequestException>()
            .Or<TimeoutException>()
            .FallbackAsync(
                fallbackAction: async (ctx, token) =>
                    await _osrmService.OptimizeRouteAsync(origin, waypoints, token),
                onFallbackAsync: async (result, ctx) =>
                {
                    // Log fallback event
                    await Task.CompletedTask;
                });

        // Execute with fallback
        return await fallbackPolicy.ExecuteAsync(async (token) =>
            await _orsService.OptimizeRouteAsync(origin, waypoints, token),
            ct);
    }
}
