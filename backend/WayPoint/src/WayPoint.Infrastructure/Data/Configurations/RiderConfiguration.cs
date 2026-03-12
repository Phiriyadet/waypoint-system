using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WayPoint.Domain.Entities;

namespace WayPoint.Infrastructure.Data.Configurations;

public class RiderConfiguration : IEntityTypeConfiguration<Rider>
{
    public void Configure(EntityTypeBuilder<Rider> builder)
    {
        builder.HasKey(r => r.Id);

        // Properties
        builder.Property(r => r.Name).HasMaxLength(200).IsRequired();
        builder.Property(r => r.Phone).HasMaxLength(20).IsRequired();
        builder.Property(r => r.VehicleType).IsRequired(); // Enum
        builder.Property(r => r.Status).IsRequired(); // Enum

        // Spatial - current location
        builder.Property(r => r.CurrentLocation)
            .HasColumnType("geography (point, 4326)");

        // Timestamp for last location update
        builder.Property(r => r.LastLocationUpdate);

        // Indexes
        builder.HasIndex(r => r.Status);
        builder.HasIndex(r => r.Phone);

        // Spatial index for current location
        builder.HasIndex(r => r.CurrentLocation)
            .HasMethod("GIST");

        // Computed property - not persisted
        builder.Ignore(r => r.IsAvailable);

        // Ignore Domain Events collection
        builder.Ignore(r => r.DomainEvents);
    }
}
