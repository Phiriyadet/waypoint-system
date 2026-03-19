using Microsoft.Extensions.DependencyInjection;

using WayPoint.Application.Common.Interfaces.ExternalServices;
using WayPoint.Infrastructure.Resilience;

namespace WayPoint.Infrastructure.Services.Routing;

public static class RoutingDependencyInjection
{
    public static IServiceCollection AddRoutingSerivce(this IServiceCollection services)
    {
        // ==========================================
        // EXTERNAL SERVICES - ROUTING
        // ==========================================

        // ORS Routing with resilience
        services.AddHttpClient<OpenRouteService>()
            .AddPolicyHandler(ResiliencePolicies.GetCombinedPolicy());

        // OSRM Routing (fallback) with resilience
        services.AddHttpClient<OsrmRoutingService>()
            .AddPolicyHandler(ResiliencePolicies.GetRetryPolicy());

        // Routing factory (ORS → OSRM fallback)
        services.AddScoped<IRoutingService, RoutingServiceFactory>();

        return services;
    }
}
