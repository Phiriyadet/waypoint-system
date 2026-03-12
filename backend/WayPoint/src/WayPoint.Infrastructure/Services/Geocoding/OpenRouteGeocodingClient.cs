using System.Net.Http.Json;

using Microsoft.Extensions.Configuration;

using NetTopologySuite.Geometries;

using WayPoint.Application.Common.Interfaces.ExternalServices;

namespace WayPoint.Infrastructure.Services.Geocoding;

public class OpenRouteGeocodingClient
{
    private readonly HttpClient _http;
    private readonly string _apiKey;

    public OpenRouteGeocodingClient(HttpClient http, IConfiguration config)
    {
        _http = http;
        _apiKey = config["OpenRouteService:ApiKey"]!;
        _http.BaseAddress = new Uri("https://api.openrouteservice.org/");
    }

    public async Task<GeocodingResult?> GeocodeAsync(
        string address, CancellationToken ct)
    {
        var response = await _http.GetAsync(
            $"geocode/search?api_key={_apiKey}&text={Uri.EscapeDataString(address)}",
            ct);

        if (!response.IsSuccessStatusCode) return null;

        var data = await response.Content.ReadFromJsonAsync<OrsGeocodingResponse>(ct);
        if (data?.Features?.Length == 0) return null;

        var feature = data.Features[0];
        var coords = feature.Geometry.Coordinates;
        var point = new Point(coords[0], coords[1]) { SRID = 4326 };
        var confidence = feature.Properties.Confidence;

        return new GeocodingResult(point, confidence);
    }

    private record OrsGeocodingResponse(
        OrsFeature[] Features);

    private record OrsFeature(
        OrsGeometry Geometry,
        OrsProperties Properties);

    private record OrsGeometry(double[] Coordinates);
    private record OrsProperties(double Confidence);
}
