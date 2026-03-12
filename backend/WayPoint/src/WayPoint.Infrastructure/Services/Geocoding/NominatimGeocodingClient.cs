using System.Net.Http.Json;

using NetTopologySuite.Geometries;

using WayPoint.Application.Common.Interfaces.ExternalServices;

namespace WayPoint.Infrastructure.Services.Geocoding;

public class NominatimGeocodingClient
{
    private readonly HttpClient _http;

    public NominatimGeocodingClient(HttpClient http)
    {
        _http = http;
        _http.BaseAddress = new Uri("https://nominatim.openstreetmap.org/");
        _http.DefaultRequestHeaders.Add("User-Agent", "WayPoint/1.0");
    }

    public async Task<GeocodingResult?> GeocodeAsync(
        string address, CancellationToken ct)
    {
        var response = await _http.GetAsync(
            $"search?q={Uri.EscapeDataString(address)}&format=json&limit=1",
            ct);

        if (!response.IsSuccessStatusCode) return null;

        var data = await response.Content
            .ReadFromJsonAsync<NominatimResult[]>(ct);
        if (data is null || data.Length == 0) return null;

        var result = data[0];
        var point = new Point(
            double.Parse(result.Lon),
            double.Parse(result.Lat))
        { SRID = 4326 };

        // Nominatim importance is 0-1, use as confidence
        return new GeocodingResult(point, result.Importance);
    }

    private record NominatimResult(
        string Lat,
        string Lon,
        double Importance);
}
