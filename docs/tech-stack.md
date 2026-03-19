# Tech Stack — WayPoint

## Runtime & Framework

| Component | Technology | Notes |
|---|---|---|
| Language | C# 12 | |
| Framework | .NET 8 | |
| API | ASP.NET Core 8 | Minimal hosting model |
| Orchestration | .NET Aspire | AppHost + ServiceDefaults |

## Backend Libraries

| Purpose | Library |
|---|---|
| CQRS / Mediator | MediatR |
| Validation | FluentValidation |
| ORM | EF Core 8 |
| Spatial | NetTopologySuite + Npgsql.EntityFrameworkCore.PostgreSQL |
| Caching client | StackExchange.Redis |
| HTTP clients | `IHttpClientFactory` + Polly |
| Auth | ASP.NET Core Identity + JWT Bearer |
| OpenAPI | Swashbuckle (Swagger) |
| Result pattern | Custom `Result<T>` / `Error` types |

## Database

| Component | Technology |
|---|---|
| Primary DB | PostgreSQL 16 |
| Spatial extension | PostGIS 3.x |
| Cache / Session | Redis 7 |
| Migrations | EF Core migrations (Infrastructure project) |
| PK type | `Guid` (uuid) |

## External Services

| Service | Usage | Env var |
|---|---|---|
| OpenRouteService | Route optimization | `ORS__ApiKey` |
| OSRM | Fast matrix routing | `OSRM__BaseUrl` |
| Nominatim | Geocoding / reverse geocoding | `NOMINATIM__BaseUrl` |

## Testing

| Type | Tools |
|---|---|
| Unit | xUnit, NSubstitute, FluentAssertions |
| Integration | xUnit, Testcontainers (`postgres:16-postgis`, `redis:7`) |
| Architecture | NetArchTest.Rules |

## DevOps & Tooling

| Tool | Purpose |
|---|---|
| Docker | Containerization |
| GitHub Actions | CI/CD pipeline |
| .NET Aspire Dashboard | Health, traces, logs (localhost:15888) |
| Notion (MCP) | Documentation hub |
| n8n | Automation / workflow integration |
| User Secrets | Dev-time secrets |
| Environment Variables | Production secrets |

## CI/CD Pipeline Steps

```
build → lint (dotnet format) → unit-tests → integration-tests → architecture-tests → publish
```

Architecture tests are a **required gate** — pipeline fails on boundary violations.
