# Coding Rules — WayPoint

## General Principles

- Clean Architecture layer boundaries are non-negotiable
- Domain-Driven Design: model the business, not the database
- Result pattern for all error handling — no exceptions across boundaries
- Immutability preferred: use `private set`, `init`, or value objects
- No magic strings: use constants, enums, or strongly-typed IDs

---

## Domain Layer

### Rules

- No EF Core, no HTTP, no Redis — zero infrastructure dependencies
- Entities have private constructors + static factory methods
- Value objects override `Equals` / `GetHashCode`
- Domain events raised inside aggregates, dispatched by Application

### Patterns

```csharp
// ✅ Factory method
public static Result<Delivery> Create(Guid id, Address origin, Address destination) { ... }

// ✅ Domain event
AddDomainEvent(new DeliveryCreatedEvent(Id));

// ❌ Never
public void SetStatus(string status) { Status = status; }  // use enum + encapsulation
```

---

## Application Layer

### Rules

- One handler per Command/Query
- Handlers depend only on domain interfaces (`IDeliveryRepository`, `IRouteService`, etc.)
- Validation via FluentValidation; register as pipeline behavior
- Return `Result<T>` — never throw from handlers

### Naming

```
CreateDeliveryCommand       → CreateDeliveryCommandHandler
GetDeliveryByIdQuery        → GetDeliveryByIdQueryHandler
AssignRiderToDeliveryCommand → AssignRiderToDeliveryCommandHandler
```

---

## Infrastructure Layer

### Rules

- Implements interfaces from Application
- EF Core configurations in `EntityTypeConfiguration<T>` — property names must exactly match domain
- Owned entities (`DeliveryLog`, `RouteWaypoint`) use `OwnsMany` inline — no standalone config files
- All HTTP clients via `IHttpClientFactory`; resilience via Polly
- Cache keys must be deterministic and namespaced: `geocode:{lat}:{lng}`, `route:ors:{hash}`

### EF Core Example

```csharp
// ✅ Correct
builder.Property(d => d.Status)
    .HasConversion<string>()
    .IsRequired();

builder.OwnsMany(d => d.Logs, log => {
    log.Property(l => l.Message).IsRequired();
    log.Property(l => l.Timestamp).IsRequired();
});
```

---

## WebApi Layer

### Rules

- Controllers are thin: map HTTP → Command/Query → return HTTP result
- Never inject repositories directly into controllers
- Validation handled by Application pipeline, not controllers
- Use `ActionResult<T>` with explicit status codes

### Controller Pattern

```csharp
[HttpPost]
public async Task<ActionResult<DeliveryDto>> Create(
    CreateDeliveryRequest request, CancellationToken ct)
{
    var command = new CreateDeliveryCommand(request.OriginId, request.DestinationId);
    var result = await _mediator.Send(command, ct);
    return result.IsSuccess ? Ok(result.Value) : result.ToProblemDetails();
}
```

---

## Database Rules

- All schema changes via EF Core migrations only — no manual DDL
- Spatial columns use `NetTopologySuite.Geometries.Point` / `LineString`
- Add spatial indexes on geometry columns
- PKs are `Guid` (uuid); no int identity keys

---

## Testing Rules

- Unit tests: mock all dependencies with NSubstitute
- Integration tests: use Testcontainers; never share state between tests
- Architecture tests: assert layer boundaries with NetArchTest on every PR
- Use builder pattern for test data

```csharp
var delivery = new DeliveryBuilder()
    .WithOrigin(TestData.AddressA)
    .WithDestination(TestData.AddressB)
    .Build();
```

---

## Error Handling

```csharp
// Domain errors — strongly typed
public static class DeliveryErrors
{
    public static Error NotFound(Guid id) => Error.NotFound("Delivery.NotFound", $"Delivery {id} not found");
    public static readonly Error AlreadyAssigned = Error.Conflict("Delivery.AlreadyAssigned", "Delivery is already assigned");
}

// Handler usage
var delivery = await _repository.GetByIdAsync(id, ct);
if (delivery is null) return Result.Failure(DeliveryErrors.NotFound(id));
```

---

## Secrets & Configuration

| Environment | Method |
|---|---|
| Development | `dotnet user-secrets` |
| Production | Environment variables |

**Never** commit secrets or `appsettings.Production.json` with sensitive values.
