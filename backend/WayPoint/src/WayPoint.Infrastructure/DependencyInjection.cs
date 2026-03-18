using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using WayPoint.Application.Common.Interfaces.Auth;
using WayPoint.Application.Common.Interfaces.ExternalServices;
using WayPoint.Application.Common.Interfaces.Infrastructure;
using WayPoint.Application.Common.Interfaces.Repositories;
using WayPoint.Domain.Entities;
using WayPoint.Infrastructure.Data;
using WayPoint.Infrastructure.Data.Interceptors;
using WayPoint.Infrastructure.Data.Repositories;
using WayPoint.Infrastructure.Identity;
using WayPoint.Infrastructure.Resilience;
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

        // ==========================================
        // IDENTITY & AUTHENTICATION
        // ==========================================

        // ASP.NET Core Identity
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // JWT Authentication
        var jwtSecret = configuration["Jwt:Secret"]!;
        var jwtIssuer = configuration["Jwt:Issuer"]!;
        var jwtAudience = configuration["Jwt:Audience"]!;

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSecret))
            };

            // SignalR JWT from query string
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                        context.Token = accessToken;
                    return Task.CompletedTask;
                }
            };
        });

        // Identity services
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddHttpContextAccessor();


        // ==========================================
        // CACHING (REDIS)
        // ==========================================

        services.AddStackExchangeRedisCache(options =>
            options.Configuration = configuration.GetConnectionString("Redis"));

        services.AddScoped<ICacheService, RedisCacheService>();

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

        // ==========================================
        // SIGNALR BROADCASTER
        // ==========================================

        // Note: IHubContext<RiderLocationHub> is auto-injected by ASP.NET Core
        // after builder.Services.AddSignalR() in Program.cs
        services.AddScoped<IRiderLocationBroadcaster, SignalRRiderLocationBroadcaster>();

        return services;
    }
}
