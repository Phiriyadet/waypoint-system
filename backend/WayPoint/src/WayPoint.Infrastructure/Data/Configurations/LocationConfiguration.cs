using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WayPoint.Domain.Entities;

namespace WayPoint.Infrastructure.Data.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.HasKey(l => l.Id);

        // Value Objects
        builder.OwnsOne(l => l.Address, a =>
        {
            a.Property(x => x.AddressInfo).HasMaxLength(500).IsRequired();
            a.Property(x => x.Subdistrict).HasMaxLength(100).IsRequired();
            a.Property(x => x.District).HasMaxLength(100).IsRequired();
            a.Property(x => x.Province).HasMaxLength(100).IsRequired();
            a.Property(x => x.PostalCode).HasMaxLength(5).IsRequired();
            a.Property(x => x.MoreInfo).HasMaxLength(500);
        });

        builder.OwnsOne(l => l.ConfidenceScore, c =>
        {
            c.Property(x => x.Value).IsRequired();
        });

        // Spatial column with SRID 4326 (WGS84)
        builder.Property(l => l.Coordinate)
            .HasColumnType("geography (point, 4326)")
            .IsRequired();

        builder.Property(l => l.VerifiedCoordinate)
            .HasColumnType("geography (point, 4326)");

        // Spatial index
        builder.HasIndex(l => l.Coordinate)
            .HasMethod("GIST");

        // Other properties
        builder.Property(l => l.PlaceName).HasMaxLength(200);
        builder.Property(l => l.AccessNotes).HasMaxLength(500);

        // Computed column for ConfidenceLevel
        builder.Ignore(l => l.ConfidenceLevel);
    }
}
