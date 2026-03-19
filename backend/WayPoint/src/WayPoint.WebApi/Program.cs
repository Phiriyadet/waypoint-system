using WayPoint.Application;
using WayPoint.Infrastructure;
using WayPoint.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Clean Architecture Layers
builder.Services
    .AddWebApi()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseWebApi();

app.Run();
