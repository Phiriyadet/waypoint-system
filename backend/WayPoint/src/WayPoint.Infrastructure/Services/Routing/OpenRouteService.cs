using System.Net.Http.Json;

using Microsoft.Extensions.Configuration;

using NetTopologySuite.Geometries;

using WayPoint.Application.Common.Interfaces.ExternalServices;

namespace WayPoint.Infrastructure.Services.Routing;

public class OpenRouteService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;

    public OpenRouteService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _apiKey = config["OpenRouteService:ApiKey"]!;
        _http.BaseAddress = new Uri("https://api.openrouteservice.org/");
        _http.Timeout = TimeSpan.FromSeconds(30);
    }

    public async Task<RouteResult> OptimizeRouteAsync(
        Point origin,
        IReadOnlyList<(Guid DeliveryId, Point Destination)> waypoints,
        CancellationToken ct)
    {
        var coordinates = new List<double[]> { new[] { origin.X, origin.Y } };
        coordinates.AddRange(waypoints.Select(w => new[] { w.Destination.X, w.Destination.Y }));

        var request = new
        {
            coordinates,
            profile = "driving-car",
            optimize_waypoints = true
        };

        var response = await _http.PostAsJsonAsync(
            $"v2/directions/driving-car/geojson?api_key={_apiKey}",
            request, ct);

        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<OrsResponse>(ct);
        var route = data!.Features[0];

        // Map optimized order back to delivery IDs
        var optimizedOrder = route.Properties.WaypointIndices
            .Skip(1) // Skip origin
            .Select(i => waypoints[i - 1].DeliveryId)
            .ToList();

        return new RouteResult(
            route.Properties.Summary.Distance,
            (int)route.Properties.Summary.Duration,
            "ORS",
            optimizedOrder);
    }

    private record OrsResponse(OrsFeature[] Features);
    private record OrsFeature(OrsProperties Properties);
    private record OrsProperties(
        OrsSummary Summary,
        int[] WaypointIndices);
    private record OrsSummary(double Distance, double Duration);
}
