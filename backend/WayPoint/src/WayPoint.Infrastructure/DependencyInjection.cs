using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using WayPoint.Application.Common.Interfaces.ExternalServices;
using WayPoint.Infrastructure.Data;
using WayPoint.Infrastructure.Identity;
using WayPoint.Infrastructure.Services.Caching;
using WayPoint.Infrastructure.Services.Geocoding;
using WayPoint.Infrastructure.Services.RealTime;
using WayPoint.Infrastructure.Services.Routing;

namespace WayPoint.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddPersistence(configuration)
            .AddIdentityInfra(configuration)
            .AddCachingService(configuration)
            .AddGeocodingService()
            .AddRoutingSerivce();

        // ==========================================
        // SIGNALR BROADCASTER
        // ==========================================

        // Note: IHubContext<RiderLocationHub> is auto-injected by ASP.NET Core
        // after builder.Services.AddSignalR() in Program.cs
        services.AddScoped<IRiderLocationBroadcaster, SignalRRiderLocationBroadcaster>();

        return services;
    }
}
