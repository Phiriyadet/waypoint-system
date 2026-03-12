using WayPoint.Application.Common.Interfaces.ExternalServices;
using WayPoint.Application.Common.Interfaces.Infrastructure;
using WayPoint.Application.Common.Interfaces.Repositories;

namespace WayPoint.Infrastructure.Services.Geocoding;

public class GeocodingService : IGeocodingService
{
    private readonly ICacheService _cache;
    private readonly OpenRouteGeocodingClient _orsClient;
    private readonly NominatimGeocodingClient _nominatimClient;
    private readonly ILocationRepository _locationRepo;

    public GeocodingService(
        ICacheService cache,
        OpenRouteGeocodingClient orsClient,
        NominatimGeocodingClient nominatimClient,
        ILocationRepository locationRepo)
    {
        _cache = cache;
        _orsClient = orsClient;
        _nominatimClient = nominatimClient;
        _locationRepo = locationRepo;
    }

    public async Task<GeocodingResult?> GeocodeAsync(
        string address, CancellationToken ct = default)
    {
        var cacheKey = $"geocode:{address}";

        // 1. Try Redis cache
        var cached = await _cache.GetAsync<GeocodingResult>(cacheKey, ct);
        if (cached is not null) return cached;

        // 2. Try OpenRouteService
        var orsResult = await _orsClient.GeocodeAsync(address, ct);
        if (orsResult is not null)
        {
            await _cache.SetAsync(cacheKey, orsResult, TimeSpan.FromDays(30), ct);
            return orsResult;
        }

        // 3. Fallback to Nominatim
        var nominatimResult = await _nominatimClient.GeocodeAsync(address, ct);
        if (nominatimResult is not null)
        {
            await _cache.SetAsync(cacheKey, nominatimResult, TimeSpan.FromDays(30), ct);
            return nominatimResult;
        }

        // 4. Last resort: Full-text search in existing locations
        var locations = await _locationRepo.SearchAsync(address, 1, ct);
        if (locations.Count == 0) return null;

        var location = locations[0];
        var result = new GeocodingResult(
            location.VerifiedCoordinate ?? location.Coordinate,
            location.ConfidenceScore.Value / 100.0);

        await _cache.SetAsync(cacheKey, result, TimeSpan.FromDays(7), ct);
        return result;
    }
}
