using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using WayPoint.Application.Common.Interfaces.Repositories;
using WayPoint.Infrastructure.Data.Interceptors;
using WayPoint.Infrastructure.Data.Repositories;
using WayPoint.Infrastructure.Identity;

namespace WayPoint.Infrastructure.Data;

public static class PersistenceDependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        // ==========================================
        // DATABASE & EF CORE
        // ==========================================

        // Register interceptors
        services.AddSingleton<AuditableEntityInterceptor>();
        services.AddScoped<DomainEventInterceptor>();

        // Register DbContext with PostGIS
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var auditInterceptor = sp.GetRequiredService<AuditableEntityInterceptor>();
            var eventInterceptor = sp.GetRequiredService<DomainEventInterceptor>();

            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsql => npgsql.UseNetTopologySuite())
            .AddInterceptors(auditInterceptor, eventInterceptor);
        });

        // ==========================================
        // REPOSITORIES
        // ==========================================

        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<IDeliveryRepository, DeliveryRepository>();
        services.AddScoped<IRiderRepository, RiderRepository>();
        services.AddScoped<IRouteRepository, RouteRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        return services;
    }

}
