using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WayPoint.Domain.Entities;

namespace WayPoint.Infrastructure.Data.Configurations;

public class RouteConfiguration : IEntityTypeConfiguration<Route>
{
    public void Configure(EntityTypeBuilder<Route> builder)
    {
        builder.HasKey(r => r.Id);

        // Properties
        builder.Property(r => r.TotalDistanceMeters).IsRequired();
        builder.Property(r => r.EstimatedDurationSeconds).IsRequired();
        builder.Property(r => r.Strategy).IsRequired(); // RouteOptimizationStrategy enum
        builder.Property(r => r.Provider).HasMaxLength(50).IsRequired();
        builder.Property(r => r.OptimizedAt).IsRequired();

        // Owned collection: RouteWaypoint
        builder.OwnsMany(r => r.Waypoints, waypoint =>
        {
            waypoint.HasKey("Id");
            waypoint.Property<Guid>("Id").ValueGeneratedOnAdd();
            waypoint.Property(w => w.DeliveryId).IsRequired();
            waypoint.Property(w => w.LocationId).IsRequired();
            waypoint.Property(w => w.Order).IsRequired();
            waypoint.Property(w => w.IsCompleted).IsRequired();
            waypoint.Property(w => w.ArrivedAt);

            // Index on Order for efficient sorting
            waypoint.HasIndex(w => w.Order);
        });

        // Relationships
        builder.HasOne<Rider>()
            .WithMany()
            .HasForeignKey(r => r.RiderId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(r => r.RiderId);
        builder.HasIndex(r => r.CreatedAt);
        builder.HasIndex(r => r.OptimizedAt);

        // Ignore Domain Events collection
        builder.Ignore(r => r.DomainEvents);
    }
}
