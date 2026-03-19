# Architecture Overview — WayPoint

## Style

Clean Architecture + Domain-Driven Design

## Layer Map

```
┌─────────────────────────────────────┐
│            WebApi                   │  HTTP in/out, controllers, middleware
├─────────────────────────────────────┤
│           Application               │  Use cases, CQRS, validation, interfaces
├─────────────────────────────────────┤
│            Domain                   │  Entities, value objects, domain events
├─────────────────────────────────────┤
│         Infrastructure              │  EF Core, Redis, ORS, OSRM, Nominatim
└─────────────────────────────────────┘
         .NET Aspire (orchestration)
```

## Dependency Rules

| Layer | May depend on |
|---|---|
| Domain | Nothing |
| Application | Domain |
| Infrastructure | Application, Domain |
| WebApi | Application |

**Infrastructure never flows into WebApi directly.**

## Projects

```
WayPoint.Domain
WayPoint.Application
WayPoint.Infrastructure
WayPoint.WebApi
WayPoint.AppHost          ← .NET Aspire host
WayPoint.ServiceDefaults  ← Aspire shared defaults
WayPoint.Tests.Unit
WayPoint.Tests.Integration
WayPoint.Tests.Architecture
```

## Key Aggregates

| Aggregate | Root | Owned Entities |
|---|---|---|
| Delivery | `Delivery` | `DeliveryLog` (OwnsMany) |
| Route | `Route` | `RouteWaypoint` (OwnsMany) |
| Rider | `Rider` | — |
| Location | `Location` | — |

## Data Flow — Create Delivery

```
POST /api/v1/deliveries
  → DeliveriesController
  → CreateDeliveryCommand (MediatR)
  → CreateDeliveryCommandHandler
  → IDeliveryRepository.AddAsync()
  → DeliveryRepository (EF Core + PostGIS)
  → PostgreSQL
```

## Data Flow — Route Calculation

```
POST /api/v1/routes/calculate
  → RoutesController
  → CalculateRouteCommand
  → CalculateRouteCommandHandler
  → IRouteService → OrsRouteService / OsrmRouteService
  → Redis cache check → External API call
  → Route aggregate saved via IRouteRepository
```

## Health Monitoring

Handled entirely by **.NET Aspire Dashboard** (localhost:15888).  
No custom `HealthController` exists in WebApi.

## External Integrations

| Service | Purpose | Notes |
|---|---|---|
| ORS | Route optimization | Free tier limits; cache results |
| OSRM | Fast routing | Public dev; self-hosted prod |
| Nominatim | Geocoding | Usage policy; cache in Redis |
| PostgreSQL + PostGIS | Persistence + spatial queries | Testcontainers in tests |
| Redis | Cache + session | Testcontainers in tests |
