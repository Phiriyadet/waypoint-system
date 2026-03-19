using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using WayPoint.Application.Common.Interfaces.Infrastructure;

namespace WayPoint.Infrastructure.Services.Caching;

public static class CachingDependencyInjection
{
    public static IServiceCollection AddCachingService(this IServiceCollection services, IConfiguration configuration)
    {
        // ==========================================
        // CACHING (REDIS)
        // ==========================================

        services.AddStackExchangeRedisCache(options =>
            options.Configuration = configuration.GetConnectionString("Redis"));

        services.AddScoped<ICacheService, RedisCacheService>();

        return services;
    }
}
