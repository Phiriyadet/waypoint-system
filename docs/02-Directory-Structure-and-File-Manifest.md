# 📂 WayPoint Directory Structure & File Manifest

## 🏗️ Complete Project Structure Overview

```
waypoint-system/
├── backend/                          # .NET Solution
├── web-admin/                        # Next.js Admin Panel
├── mobile-app/                       # React Native Mobile App
├── infrastructure/                   # IaC & DevOps
├── docs/                            # Documentation
├── docker-compose.yml               # Local development
├── .gitignore                       # Multi-project ignore rules
└── README.md                        # Project overview
```

---

## 🔧 Backend Structure (.NET 8/9 Clean Architecture)

```
backend/
├── WayPoint.sln                                    # Solution file
├── .editorconfig                                   # Code style rules
├── Directory.Build.props                           # Shared MSBuild properties
├── Directory.Packages.props                        # Central Package Management
├── nuget.config                                    # NuGet sources
│
├── src/
│   ├── WayPoint.Domain/                           # 🎯 Core Domain Layer
│   │   ├── WayPoint.Domain.csproj
│   │   ├── Common/
│   │   │   ├── BaseEntity.cs
│   │   │   ├── IAggregateRoot.cs
│   │   │   ├── IDomainEvent.cs
│   │   │   └── ValueObject.cs
│   │   ├── Entities/
│   │   │   ├── Location.cs                        # 📍 Core GIS entity
│   │   │   ├── Delivery.cs                        # 📦 Delivery aggregate root
│   │   │   ├── Rider.cs                           # 🏍️ Rider entity
│   │   │   ├── Route.cs                           # 🛣️ Optimized route
│   │   │   ├── RouteWaypoint.cs                   # Route stop
│   │   │   └── DeliveryLog.cs                     # Audit trail
│   │   ├── ValueObjects/
│   │   │   ├── LatLng.cs                          # 🌍 Geographic coordinate
│   │   │   ├── Address.cs                         # 📮 Structured address
│   │   │   ├── GpsAccuracy.cs                     # GPS precision level
│   │   │   ├── ConfidenceScore.cs                 # Location confidence (0-100)
│   │   │   └── Distance.cs                        # Distance with unit
│   │   ├── Events/
│   │   │   ├── DeliveryCreatedEvent.cs
│   │   │   ├── DeliveryCompletedEvent.cs          # 🔄 Triggers location learning
│   │   │   ├── LocationVerifiedEvent.cs
│   │   │   └── RouteOptimizedEvent.cs
│   │   ├── Enums/
│   │   │   ├── DeliveryStatus.cs                  # Pending, InTransit, Completed
│   │   │   ├── LocationConfidenceLevel.cs         # Low, Medium, High, Verified
│   │   │   └── RouteOptimizationStrategy.cs       # Fastest, Shortest, Balanced
│   │   └── Exceptions/
│   │       ├── LocationNotFoundException.cs
│   │       ├── InvalidCoordinateException.cs
│   │       └── DeliveryAlreadyCompletedException.cs
│   │
│   ├── WayPoint.Application/                      # 📋 Application Layer (CQRS)
│   │   ├── WayPoint.Application.csproj
│   │   ├── Common/
│   │   │   ├── Interfaces/
│   │   │   │   ├── IApplicationDbContext.cs
│   │   │   │   ├── ILocationRepository.cs
│   │   │   │   ├── IDeliveryRepository.cs
│   │   │   │   ├── IRiderRepository.cs
│   │   │   │   ├── IRouteRepository.cs
│   │   │   │   ├── IGeocodingService.cs           # Nominatim abstraction
│   │   │   │   ├── IRoutingService.cs             # ORS/OSRM abstraction
│   │   │   │   └── ICacheService.cs               # Redis abstraction
│   │   │   ├── Mappings/
│   │   │   │   └── MappingProfile.cs              # AutoMapper profiles
│   │   │   ├── Models/
│   │   │   │   ├── Result.cs                      # Result<T> pattern
│   │   │   │   ├── PagedList.cs                   # Pagination wrapper
│   │   │   │   └── PaginationParams.cs
│   │   │   └── Behaviors/
│   │   │       ├── LoggingBehavior.cs             # MediatR pipeline
│   │   │       ├── ValidationBehavior.cs          # FluentValidation
│   │   │       └── CachingBehavior.cs             # Auto-cache queries
│   │   │
│   │   ├── Locations/
│   │   │   ├── Commands/
│   │   │   │   ├── CreateLocation/
│   │   │   │   │   ├── CreateLocationCommand.cs
│   │   │   │   │   ├── CreateLocationCommandHandler.cs
│   │   │   │   │   └── CreateLocationCommandValidator.cs
│   │   │   │   ├── UpdateVerifiedLocation/
│   │   │   │   │   ├── UpdateVerifiedLocationCommand.cs
│   │   │   │   │   └── UpdateVerifiedLocationCommandHandler.cs
│   │   │   │   └── MergeLocations/               # Dedupe utility
│   │   │   │       ├── MergeLocationsCommand.cs
│   │   │   │       └── MergeLocationsCommandHandler.cs
│   │   │   ├── Queries/
│   │   │   │   ├── GetLocationById/
│   │   │   │   │   ├── GetLocationByIdQuery.cs
│   │   │   │   │   └── GetLocationByIdQueryHandler.cs
│   │   │   │   ├── GetNearbyLocations/
│   │   │   │   │   ├── GetNearbyLocationsQuery.cs
│   │   │   │   │   └── GetNearbyLocationsQueryHandler.cs
│   │   │   │   └── SearchLocations/
│   │   │   │       ├── SearchLocationsQuery.cs
│   │   │   │       └── SearchLocationsQueryHandler.cs
│   │   │   └── DTOs/
│   │   │       ├── LocationDto.cs
│   │   │       ├── NearbyLocationDto.cs
│   │   │       └── LocationStatisticsDto.cs
│   │   │
│   │   ├── Deliveries/
│   │   │   ├── Commands/
│   │   │   │   ├── CreateDelivery/
│   │   │   │   │   ├── CreateDeliveryCommand.cs
│   │   │   │   │   ├── CreateDeliveryCommandHandler.cs
│   │   │   │   │   └── CreateDeliveryCommandValidator.cs
│   │   │   │   ├── AssignRider/
│   │   │   │   │   ├── AssignRiderCommand.cs
│   │   │   │   │   └── AssignRiderCommandHandler.cs
│   │   │   │   └── CompleteDelivery/             # 🧠 Location learning trigger
│   │   │   │       ├── CompleteDeliveryCommand.cs
│   │   │   │       ├── CompleteDeliveryCommandHandler.cs
│   │   │   │       └── CompleteDeliveryCommandValidator.cs
│   │   │   ├── Queries/
│   │   │   │   ├── GetDeliveryById/
│   │   │   │   ├── GetRiderDeliveries/
│   │   │   │   └── GetDeliveryHistory/
│   │   │   └── DTOs/
│   │   │       ├── DeliveryDto.cs
│   │   │       ├── CreateDeliveryDto.cs
│   │   │       └── CompleteDeliveryDto.cs
│   │   │
│   │   ├── Routes/
│   │   │   ├── Commands/
│   │   │   │   ├── OptimizeRoute/
│   │   │   │   │   ├── OptimizeRouteCommand.cs   # 🗺️ TSP/VRP solver
│   │   │   │   │   ├── OptimizeRouteCommandHandler.cs
│   │   │   │   │   └── OptimizeRouteCommandValidator.cs
│   │   │   │   └── RecalculateRoute/
│   │   │   ├── Queries/
│   │   │   │   ├── GetOptimalRoute/
│   │   │   │   ├── GetRouteETA/
│   │   │   │   └── GetRouteAlternatives/
│   │   │   └── DTOs/
│   │   │       ├── RouteDto.cs
│   │   │       ├── WaypointDto.cs
│   │   │       └── RouteStatisticsDto.cs
│   │   │
│   │   ├── Riders/
│   │   │   ├── Commands/
│   │   │   │   ├── CreateRider/
│   │   │   │   ├── UpdateRiderLocation/          # Real-time GPS updates
│   │   │   │   └── UpdateRiderStatus/
│   │   │   ├── Queries/
│   │   │   │   ├── GetAvailableRiders/
│   │   │   │   ├── GetRiderById/
│   │   │   │   └── GetRiderStatistics/
│   │   │   └── DTOs/
│   │   │       ├── RiderDto.cs
│   │   │       └── RiderLocationDto.cs
│   │   │
│   │   └── DependencyInjection.cs                # Service registration
│   │
│   ├── WayPoint.Infrastructure/                   # 🔌 Infrastructure Layer
│   │   ├── WayPoint.Infrastructure.csproj
│   │   ├── Data/
│   │   │   ├── ApplicationDbContext.cs           # EF Core + PostGIS
│   │   │   ├── Configurations/
│   │   │   │   ├── LocationConfiguration.cs      # Fluent API for Location
│   │   │   │   ├── DeliveryConfiguration.cs
│   │   │   │   ├── RiderConfiguration.cs
│   │   │   │   └── RouteConfiguration.cs
│   │   │   ├── Interceptors/
│   │   │   │   ├── AuditableEntityInterceptor.cs # Auto-set CreatedAt
│   │   │   │   └── DomainEventInterceptor.cs     # Dispatch domain events
│   │   │   ├── Migrations/
│   │   │   │   └── (auto-generated files)
│   │   │   └── Repositories/
│   │   │       ├── LocationRepository.cs         # PostGIS queries
│   │   │       ├── DeliveryRepository.cs
│   │   │       ├── RiderRepository.cs
│   │   │       └── RouteRepository.cs
│   │   │
│   │   ├── Services/
│   │   │   ├── Geocoding/
│   │   │   │   ├── NominatimGeocodingService.cs
│   │   │   │   └── PostGisReverseGeocoder.cs    # Fallback geocoder
│   │   │   ├── Routing/
│   │   │   │   ├── OpenRouteService.cs          # Primary routing
│   │   │   │   ├── OsrmRoutingService.cs        # Fallback routing
│   │   │   │   └── RoutingServiceFactory.cs     # Strategy pattern
│   │   │   ├── Caching/
│   │   │   │   ├── RedisCacheService.cs
│   │   │   │   └── InMemoryCacheService.cs      # Dev fallback
│   │   │   └── LocationIntelligence/
│   │   │       ├── LocationLearningService.cs   # 🧠 Core learning logic
│   │   │       ├── SpatialDeduplicationService.cs
│   │   │       └── ConfidenceScoreCalculator.cs
│   │   │
│   │   ├── Resilience/
│   │   │   ├── ResiliencePolicies.cs            # Polly policies
│   │   │   └── CircuitBreakerState.cs
│   │   │
│   │   └── DependencyInjection.cs               # Infrastructure registration
│   │
│   ├── WayPoint.WebApi/                          # 🌐 REST API
│   │   ├── WayPoint.WebApi.csproj
│   │   ├── Program.cs                            # Application entry point
│   │   ├── appsettings.json
│   │   ├── appsettings.Development.json
│   │   ├── appsettings.Production.json.example
│   │   │
│   │   ├── Controllers/
│   │   │   ├── LocationsController.cs
│   │   │   ├── DeliveriesController.cs
│   │   │   ├── RidersController.cs
│   │   │   ├── RoutesController.cs
│   │   │   └── HealthController.cs              # Health checks
│   │   │
│   │   ├── Middleware/
│   │   │   ├── GlobalExceptionHandler.cs        # Centralized error handling
│   │   │   ├── RequestLoggingMiddleware.cs
│   │   │   └── RateLimitingMiddleware.cs
│   │   │
│   │   ├── Filters/
│   │   │   ├── ApiExceptionFilterAttribute.cs
│   │   │   └── ValidateModelAttribute.cs
│   │   │
│   │   ├── Extensions/
│   │   │   ├── ServiceCollectionExtensions.cs
│   │   │   └── WebApplicationExtensions.cs
│   │   │
│   │   └── Hubs/                                # SignalR for real-time
│   │       └── RiderLocationHub.cs              # Live GPS updates
│   │
│   └── WayPoint.Aspire/                         # 🎛️ Orchestration (Optional)
│       ├── WayPoint.Aspire.csproj
│       ├── Program.cs                           # Aspire host
│       └── appsettings.json
│
└── tests/
    ├── WayPoint.UnitTests/
    │   ├── WayPoint.UnitTests.csproj
    │   ├── Domain/
    │   │   ├── ValueObjects/
    │   │   │   ├── LatLngTests.cs
    │   │   │   └── AddressTests.cs
    │   │   └── Entities/
    │   │       └── LocationTests.cs
    │   └── Application/
    │       ├── Locations/
    │       │   └── CreateLocationCommandHandlerTests.cs
    │       └── Deliveries/
    │           └── CompleteDeliveryCommandHandlerTests.cs
    │
    ├── WayPoint.IntegrationTests/
    │   ├── WayPoint.IntegrationTests.csproj
    │   ├── Infrastructure/
    │   │   ├── LocationRepositoryTests.cs       # Test PostGIS queries
    │   │   └── RoutingServiceTests.cs
    │   └── WebApi/
    │       ├── LocationsControllerTests.cs
    │       └── DeliveriesControllerTests.cs
    │
    └── WayPoint.ArchitectureTests/
        ├── WayPoint.ArchitectureTests.csproj
        └── ArchitectureTests.cs                 # NetArchTest rules
```

---

## 🌐 Web Admin Structure (Next.js 14+)

```
web-admin/
├── package.json
├── tsconfig.json
├── next.config.js
├── tailwind.config.ts
├── .env.local.example
├── .eslintrc.json
│
├── src/
│   ├── app/                                     # App Router
│   │   ├── layout.tsx                           # Root layout
│   │   ├── page.tsx                             # Dashboard home
│   │   ├── error.tsx                            # Error boundary
│   │   ├── loading.tsx                          # Loading fallback
│   │   │
│   │   ├── (auth)/                              # Auth route group
│   │   │   ├── login/
│   │   │   │   └── page.tsx
│   │   │   └── layout.tsx
│   │   │
│   │   ├── (dashboard)/                         # Protected routes
│   │   │   ├── layout.tsx                       # Sidebar layout
│   │   │   ├── deliveries/
│   │   │   │   ├── page.tsx                     # List deliveries
│   │   │   │   ├── [id]/
│   │   │   │   │   └── page.tsx                 # Delivery detail
│   │   │   │   └── new/
│   │   │   │       └── page.tsx                 # Create delivery
│   │   │   ├── riders/
│   │   │   │   ├── page.tsx
│   │   │   │   └── [id]/
│   │   │   │       └── page.tsx
│   │   │   ├── routes/
│   │   │   │   ├── page.tsx
│   │   │   │   └── optimize/
│   │   │   │       └── page.tsx                 # Route optimizer UI
│   │   │   ├── locations/
│   │   │   │   ├── page.tsx
│   │   │   │   └── map/
│   │   │   │       └── page.tsx                 # Location map view
│   │   │   └── analytics/
│   │   │       └── page.tsx                     # Performance metrics
│   │   │
│   │   └── api/                                 # API routes (BFF)
│   │       ├── auth/
│   │       │   └── [...nextauth]/
│   │       │       └── route.ts                 # NextAuth.js
│   │       └── proxy/                           # Backend proxy
│   │           └── [...path]/
│   │               └── route.ts
│   │
│   ├── components/
│   │   ├── ui/                                  # Shadcn components
│   │   │   ├── button.tsx
│   │   │   ├── input.tsx
│   │   │   ├── table.tsx
│   │   │   ├── card.tsx
│   │   │   └── dialog.tsx
│   │   │
│   │   ├── maps/
│   │   │   ├── MapContainer.tsx                 # Mapbox wrapper
│   │   │   ├── DeliveryMarker.tsx
│   │   │   ├── RiderMarker.tsx
│   │   │   └── RoutePolyline.tsx
│   │   │
│   │   ├── deliveries/
│   │   │   ├── DeliveryList.tsx
│   │   │   ├── DeliveryCard.tsx
│   │   │   └── DeliveryStatusBadge.tsx
│   │   │
│   │   ├── riders/
│   │   │   ├── RiderList.tsx
│   │   │   ├── RiderCard.tsx
│   │   │   └── LiveRiderMap.tsx                 # Real-time tracking
│   │   │
│   │   └── layout/
│   │       ├── Sidebar.tsx
│   │       ├── Header.tsx
│   │       └── Breadcrumbs.tsx
│   │
│   ├── hooks/
│   │   ├── useDeliveries.ts                     # TanStack Query hooks
│   │   ├── useRiders.ts
│   │   ├── useRoutes.ts
│   │   ├── useLocations.ts
│   │   ├── useMapbox.ts                         # Mapbox GL JS hook
│   │   └── useWebSocket.ts                      # SignalR connection
│   │
│   ├── lib/
│   │   ├── api-client.ts                        # Axios/Fetch wrapper
│   │   ├── utils.ts                             # Utilities (cn, formatters)
│   │   ├── constants.ts
│   │   └── validations.ts                       # Zod schemas
│   │
│   ├── stores/
│   │   ├── useAuthStore.ts                      # Zustand: Auth state
│   │   ├── useUIStore.ts                        # Zustand: UI state
│   │   └── useMapStore.ts                       # Zustand: Map state
│   │
│   └── types/
│       ├── delivery.ts
│       ├── rider.ts
│       ├── location.ts
│       └── route.ts
│
└── public/
    ├── markers/
    │   ├── rider-marker.svg
    │   └── delivery-marker.svg
    └── favicon.ico
```

---

## 📱 Mobile App Structure (React Native + Expo)

```
mobile-app/
├── package.json
├── tsconfig.json
├── app.json                                     # Expo config
├── babel.config.js
├── metro.config.js
├── .env.example
│
├── src/
│   ├── app/                                     # Expo Router
│   │   ├── _layout.tsx                          # Root layout
│   │   ├── index.tsx                            # Landing screen
│   │   │
│   │   ├── (auth)/
│   │   │   ├── _layout.tsx
│   │   │   ├── login.tsx
│   │   │   └── register.tsx
│   │   │
│   │   ├── (tabs)/                              # Main app tabs
│   │   │   ├── _layout.tsx                      # Tab navigator
│   │   │   ├── home.tsx                         # Today's deliveries
│   │   │   ├── deliveries.tsx                   # Delivery history
│   │   │   ├── map.tsx                          # Live map view
│   │   │   └── profile.tsx                      # Rider profile
│   │   │
│   │   └── delivery/
│   │       └── [id].tsx                         # Delivery detail
│   │
│   ├── components/
│   │   ├── ui/
│   │   │   ├── Button.tsx
│   │   │   ├── Card.tsx
│   │   │   ├── Input.tsx
│   │   │   └── LoadingSpinner.tsx
│   │   │
│   │   ├── maps/
│   │   │   ├── MapView.tsx                      # react-native-maps wrapper
│   │   │   ├── RiderMarker.tsx
│   │   │   ├── RoutePolyline.tsx
│   │   │   └── CurrentLocationButton.tsx
│   │   │
│   │   ├── deliveries/
│   │   │   ├── DeliveryCard.tsx
│   │   │   ├── DeliveryList.tsx
│   │   │   ├── DeliveryStatusBadge.tsx
│   │   │   └── CompleteDeliveryButton.tsx       # 🔘 Trigger learning
│   │   │
│   │   └── layout/
│   │       ├── ScreenContainer.tsx
│   │       └── Header.tsx
│   │
│   ├── hooks/
│   │   ├── useDeliveries.ts
│   │   ├── useLocation.ts                       # GPS tracking
│   │   ├── useBackgroundLocation.ts             # Background GPS
│   │   ├── useOfflineQueue.ts                   # Offline sync
│   │   └── useWebSocket.ts
│   │
│   ├── services/
│   │   ├── api.ts                               # API client
│   │   ├── storage.ts                           # AsyncStorage wrapper
│   │   ├── location.ts                          # expo-location
│   │   ├── notifications.ts                     # expo-notifications
│   │   └── offline-queue.ts                     # Queue manager
│   │
│   ├── stores/
│   │   ├── useAuthStore.ts
│   │   ├── useDeliveryStore.ts
│   │   └── useLocationStore.ts
│   │
│   ├── database/
│   │   ├── schema.ts                            # WatermelonDB schema
│   │   ├── models/
│   │   │   ├── Delivery.ts
│   │   │   ├── Location.ts
│   │   │   └── OfflineAction.ts
│   │   └── sync.ts                              # Sync logic
│   │
│   ├── utils/
│   │   ├── constants.ts
│   │   ├── formatters.ts
│   │   ├── validators.ts
│   │   └── permissions.ts
│   │
│   └── types/
│       ├── delivery.ts
│       ├── rider.ts
│       └── navigation.ts
│
└── assets/
    ├── images/
    ├── icons/
    └── fonts/
```

---

## 🚀 Infrastructure Structure

```
infrastructure/
├── docker/
│   ├── Dockerfile.api                           # Multi-stage .NET build
│   ├── Dockerfile.web                           # Next.js build
│   └── docker-compose.yml                       # Local dev stack
│       ├── postgres + postgis
│       ├── redis
│       ├── waypoint-api
│       └── waypoint-web
│
├── terraform/                                   # IaC (Optional)
│   ├── main.tf
│   ├── variables.tf
│   ├── outputs.tf
│   └── modules/
│       ├── fly-io/
│       ├── supabase/
│       └── vercel/
│
├── k8s/                                         # Kubernetes (Optional)
│   ├── api-deployment.yaml
│   ├── redis-statefulset.yaml
│   └── ingress.yaml
│
└── scripts/
    ├── setup-postgis.sh                         # Enable PostGIS extension
    ├── seed-data.sh                             # Dev data seeding
    ├── backup-db.sh
    └── migrate-production.sh
```

---

## 📄 Critical Files Detail

### Backend Core Files

#### `BaseEntity.cs` (Domain/Common/)
```csharp
public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    
    public void AddDomainEvent(IDomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);
    
    public void ClearDomainEvents()
        => _domainEvents.Clear();
}
```

#### `LatLng.cs` (Domain/ValueObjects/)
```csharp
public class LatLng : ValueObject
{
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    
    private LatLng() { } // EF Core
    
    public LatLng(double latitude, double longitude)
    {
        if (latitude < -90 || latitude > 90)
            throw new InvalidCoordinateException("Latitude must be between -90 and 90");
        if (longitude < -180 || longitude > 180)
            throw new InvalidCoordinateException("Longitude must be between -180 and 180");
            
        Latitude = latitude;
        Longitude = longitude;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Latitude;
        yield return Longitude;
    }
}
```

#### `Location.cs` (Domain/Entities/)
```csharp
public class Location : BaseEntity, IAggregateRoot
{
    public string Address { get; private set; }
    public LatLng Coordinate { get; private set; }            // Geocoded location
    public LatLng? VerifiedCoordinate { get; private set; }   // 🧠 GPS-verified location
    public ConfidenceScore ConfidenceScore { get; private set; } = new(0);
    public int DeliveryCount { get; private set; } = 0;
    
    // PostGIS will store this as Geography(Point, 4326)
    public Point GeographyPoint => CreatePoint(Coordinate);
    public Point? VerifiedGeographyPoint => VerifiedCoordinate != null 
        ? CreatePoint(VerifiedCoordinate) 
        : null;
    
    private Location() { } // EF Core
    
    public Location(string address, LatLng coordinate)
    {
        Address = address;
        Coordinate = coordinate;
        AddDomainEvent(new LocationCreatedEvent(Id));
    }
    
    public void UpdateVerifiedLocation(LatLng actualLocation, GpsAccuracy accuracy)
    {
        if (accuracy.IsHigh && CalculateDistance(actualLocation) > 50) // meters
        {
            VerifiedCoordinate = actualLocation;
            ConfidenceScore = ConfidenceScore.Increase(10);
            AddDomainEvent(new LocationVerifiedEvent(Id, actualLocation));
        }
        else
        {
            ConfidenceScore = ConfidenceScore.Increase(1);
        }
        
        DeliveryCount++;
    }
    
    private double CalculateDistance(LatLng other)
    {
        // Haversine formula - this will be done by PostGIS in production
        // Kept here for domain logic clarity
        return GeoCalculator.HaversineDistance(Coordinate, other);
    }
    
    private static Point CreatePoint(LatLng coord)
    {
        var point = new Point(coord.Longitude, coord.Latitude) { SRID = 4326 };
        return point;
    }
}
```

#### `LocationRepository.cs` (Infrastructure/Data/Repositories/)
```csharp
public class LocationRepository : ILocationRepository
{
    private readonly ApplicationDbContext _context;
    
    public async Task<Location?> FindNearbyLocationAsync(
        LatLng coordinate, 
        double radiusMeters = 50,
        CancellationToken ct = default)
    {
        var point = new Point(coordinate.Longitude, coordinate.Latitude) { SRID = 4326 };
        
        return await _context.Locations
            .Where(l => l.GeographyPoint.Distance(point) <= radiusMeters)
            .OrderBy(l => l.GeographyPoint.Distance(point))
            .FirstOrDefaultAsync(ct);
    }
    
    public async Task<List<Location>> GetNearbyLocationsAsync(
        LatLng center,
        double radiusMeters,
        int limit = 10,
        CancellationToken ct = default)
    {
        var point = new Point(center.Longitude, center.Latitude) { SRID = 4326 };
        
        return await _context.Locations
            .Where(l => l.GeographyPoint.Distance(point) <= radiusMeters)
            .OrderBy(l => l.GeographyPoint.Distance(point))
            .Take(limit)
            .ToListAsync(ct);
    }
}
```

### Frontend Core Files

#### `useMapbox.ts` (web-admin/src/hooks/)
```typescript
import { useEffect, useRef } from 'react';
import mapboxgl from 'mapbox-gl';

export function useMapbox(containerId: string, initialCenter: [number, number]) {
  const mapRef = useRef<mapboxgl.Map | null>(null);
  
  useEffect(() => {
    mapboxgl.accessToken = process.env.NEXT_PUBLIC_MAPBOX_TOKEN!;
    
    mapRef.current = new mapboxgl.Map({
      container: containerId,
      style: 'mapbox://styles/mapbox/streets-v12',
      center: initialCenter,
      zoom: 12
    });
    
    return () => mapRef.current?.remove();
  }, [containerId, initialCenter]);
  
  return mapRef;
}
```

---

## 📦 File Count Summary

| Category | File Count | Key Technologies |
|----------|-----------|------------------|
| Backend C# Files | ~80 files | .NET 8/9, EF Core, MediatR |
| Backend Test Files | ~30 files | xUnit, FluentAssertions |
| Frontend Web Files | ~50 files | Next.js 14, React, TypeScript |
| Mobile App Files | ~45 files | React Native, Expo, WatermelonDB |
| Infrastructure Files | ~15 files | Docker, Terraform, Scripts |
| **Total** | **~220 files** | |

---

## ✅ Next Steps

1. ✅ Review this directory structure
2. → Proceed to **Page 3**: Database & GIS Implementation
3. Use this structure to scaffold the project with CLI:
   ```bash
   # Backend
   dotnet new sln -n WayPoint
   dotnet new classlib -n WayPoint.Domain
   dotnet new classlib -n WayPoint.Application
   dotnet new classlib -n WayPoint.Infrastructure
   dotnet new webapi -n WayPoint.WebApi
   
   # Frontend
   npx create-next-app@latest web-admin --typescript --tailwind --app
   npx create-expo-app mobile-app --template blank-typescript
   ```

---

**Document Version**: 1.0  
**Last Updated**: February 2026  
**Status**: Ready for Scaffolding
