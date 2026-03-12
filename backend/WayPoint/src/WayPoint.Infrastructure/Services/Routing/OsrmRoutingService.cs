using System.Net.Http.Json;

using NetTopologySuite.Geometries;

using WayPoint.Application.Common.Interfaces.ExternalServices;

namespace WayPoint.Infrastructure.Services.Routing;

public class OsrmRoutingService
{
    private readonly HttpClient _http;

    public OsrmRoutingService(HttpClient http)
    {
        _http = http;
        _http.BaseAddress = new Uri("https://router.project-osrm.org/");
        _http.Timeout = TimeSpan.FromSeconds(30);
    }

    public async Task<RouteResult> OptimizeRouteAsync(
        Point origin,
        IReadOnlyList<(Guid DeliveryId, Point Destination)> waypoints,
        CancellationToken ct)
    {
        // Build coordinates string: origin;waypoint1;waypoint2;...
        var coordinates = new List<string> { $"{origin.X},{origin.Y}" };
        coordinates.AddRange(waypoints.Select(w => $"{w.Destination.X},{w.Destination.Y}"));

        var coordsString = string.Join(";", coordinates);
        // OSRM Trip API for route optimization
        var url = $"trip/v1/driving/{coordsString}?source=first&roundtrip=false&steps=false";

        var response = await _http.GetAsync(url, ct);
        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<OsrmResponse>(ct);

        if (data?.Code != "Ok" || data.Trips is null || data.Trips.Length == 0)
            throw new InvalidOperationException("OSRM optimization failed");

        var trip = data.Trips[0];
        // Map waypoint indices to delivery IDs
        var optimizedOrder = data.Waypoints
            .Skip(1) // Skip origin
            .OrderBy(w => w.TripsIndex)
            .ThenBy(w => w.WaypointIndex)
            .Select(w => waypoints[w.WaypointIndex - 1].DeliveryId)
            .ToList();

        return new RouteResult(
            trip.Distance,
            (int)trip.Duration,
            "OSRM",
            optimizedOrder);
    }

    private record OsrmResponse(
        string Code,
        OsrmTrip[]? Trips,
        OsrmWaypoint[] Waypoints);
    private record OsrmTrip(
        double Distance,
        double Duration);
    private record OsrmWaypoint(
        int WaypointIndex,
        int TripsIndex);
}
