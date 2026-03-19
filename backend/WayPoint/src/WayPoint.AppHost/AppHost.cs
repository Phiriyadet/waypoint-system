var builder = DistributedApplication.CreateBuilder(args);


// 1. เพิ่ม PostgreSQL + PostGIS
var postgres = builder.AddPostgres("DefaultConnection")
    .WithImage("postgis/postgis", "16-3.4")
    .WithPgAdmin()  // เปิด pgAdmin UI ที่ http://localhost:5050
    .AddDatabase("waypoint");

// 2. เพิ่ม Redis
var redis = builder.AddRedis("Redis")
    .WithRedisCommander();  // เปิด Redis UI ที่ http://localhost:8081

// 3. เพิ่ม API project
var api = builder.AddProject<Projects.WayPoint_WebApi>("waypoint-webapi")
    .WithReference(postgres)
    .WithReference(redis);

builder.Build().Run();
