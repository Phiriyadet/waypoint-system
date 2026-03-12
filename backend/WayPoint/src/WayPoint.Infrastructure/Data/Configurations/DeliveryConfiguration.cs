using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WayPoint.Domain.Entities;

namespace WayPoint.Infrastructure.Data.Configurations;

public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> builder)
    {
        builder.HasKey(d => d.Id);

        // Properties
        builder.Property(d => d.RecipientName).HasMaxLength(200).IsRequired();
        builder.Property(d => d.RecipientPhone).HasMaxLength(20).IsRequired();
        builder.Property(d => d.DeliveryCode).HasMaxLength(6).IsRequired();
        builder.Property(d => d.Status).IsRequired();

        // Spatial
        builder.Property(d => d.ActualDeliveryCoordinate)
            .HasColumnType("geography (point, 4326)");

        // Value Object
        builder.OwnsOne(d => d.GpsAccuracy, g =>
        {
            g.Property(x => x.Meters).HasColumnName("GpsAccuracyMeters");
        });

        // Owned collection: DeliveryLog
        builder.OwnsMany(d => d.Logs, log =>
        {
            log.HasKey("Id");
            log.Property<Guid>("Id").ValueGeneratedOnAdd();
            log.Property(l => l.DeliveryId).IsRequired();
            log.Property(l => l.EventType).IsRequired();
            log.Property(l => l.OccurredAt).IsRequired();
            log.Property(l => l.Note).HasMaxLength(500);
        });

        // Relationships
        builder.HasOne<Location>()
            .WithMany()
            .HasForeignKey(d => d.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Rider>()
            .WithMany()
            .HasForeignKey(d => d.RiderId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(d => d.Status);
        builder.HasIndex(d => d.RiderId);
        builder.HasIndex(d => d.DeliveryCode).IsUnique();

        // Ignore Domain Events collection
        builder.Ignore(d => d.DomainEvents);
    }
}
