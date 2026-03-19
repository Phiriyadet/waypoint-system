# Project Context — WayPoint

## Overview

**WayPoint** is a delivery management system that enables real-time route optimization, rider assignment, and delivery tracking.

## Goals

- Manage deliveries end-to-end: creation → assignment → tracking → completion
- Optimize routes using ORS and OSRM based on real-world road networks
- Provide geocoding and reverse geocoding via Nominatim
- Expose a REST API consumed by web/mobile frontends

## Scope

| In Scope | Out of Scope |
|---|---|
| Delivery CRUD | Payment processing |
| Rider management | Customer-facing mobile app |
| Route calculation + optimization | Third-party logistics integration |
| Location geocoding | Real-time push notifications (future) |
| Health monitoring via Aspire | Multi-tenant SaaS |

## Current Status

| Layer | Status |
|---|---|
| Domain | ✅ Complete |
| Application | ✅ Complete |
| Infrastructure | ✅ Reviewed & corrected |
| WebApi | ✅ Controllers rewritten |
| Tests | ✅ Unit + Integration + Architecture |
| Documentation | 🔄 In progress (Notion) |

## Key Constraints

- EF Core owned entities (`DeliveryLog`, `RouteWaypoint`) use inline `OwnsMany` — no separate config files
- .NET Aspire replaces custom health endpoints
- ORS free tier: respect rate limits; cache aggressively in Redis
- All secrets managed via User Secrets (dev) or environment variables (prod)
