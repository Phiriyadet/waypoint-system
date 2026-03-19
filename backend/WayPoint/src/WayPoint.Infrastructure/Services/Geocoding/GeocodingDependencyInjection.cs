using Microsoft.Extensions.DependencyInjection;

using WayPoint.Application.Common.Interfaces.ExternalServices;
using WayPoint.Infrastructure.Resilience;

namespace WayPoint.Infrastructure.Services.Geocoding;

public static class GeocodingDependencyInjection
{
    public static IServiceCollection AddGeocodingService(this IServiceCollection services)
    {
        // ==========================================
        // EXTERNAL SERVICES - GEOCODING
        // ==========================================

        // ORS Geocoding Client with resilience
        services.AddHttpClient<OpenRouteGeocodingClient>()
            .AddPolicyHandler(ResiliencePolicies.GetRetryPolicy())
            .AddPolicyHandler(ResiliencePolicies.GetCircuitBreakerPolicy());

        // Nominatim Client with resilience
        services.AddHttpClient<NominatimGeocodingClient>()
            .AddPolicyHandler(ResiliencePolicies.GetRetryPolicy());

        // Geocoding service (coordinates fallback chain)
        services.AddScoped<IGeocodingService, GeocodingService>();

        return services;
    }
}
