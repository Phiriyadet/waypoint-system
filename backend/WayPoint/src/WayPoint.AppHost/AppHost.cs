var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.WayPoint_WebApi>("waypoint-webapi");

builder.Build().Run();
