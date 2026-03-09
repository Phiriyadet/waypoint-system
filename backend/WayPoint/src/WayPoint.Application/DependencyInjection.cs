using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WayPoint.Application.Common.Behaviors;
using WayPoint.Application.Common.Mappings;

namespace WayPoint.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);

        services.AddSingleton<LocationMapper>();
        services.AddSingleton<DeliveryMapper>();
        services.AddSingleton<RiderMapper>();
        services.AddSingleton<RouteMapper>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        // CachingBehavior ไม่ register แบบ open generic
        // MediatR resolve ให้อัตโนมัติเฉพาะ TRequest ที่ implement ICacheable

        return services;
    }
}
