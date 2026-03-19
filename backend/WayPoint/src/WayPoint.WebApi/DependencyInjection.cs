using WayPoint.WebApi.Hubs;

namespace WayPoint.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services)
    {
        // Controllers
        services.AddControllers();

        // Swagger / OpenAPI
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // SignalR
        services.AddSignalR();

        // CORS Policy
        services.AddCors(options =>
        {
            options.AddPolicy("default", policy =>
            {
                policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .SetIsOriginAllowed(_ => true);
            });
        });

        return services;
    }

    public static WebApplication UseWebApi(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors("default");

        // Authentication ต้องมาก่อน Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        // SignalR Hub
        app.MapHub<RiderLocationHub>("/hubs/rider-location");

        return app;
    }
}
