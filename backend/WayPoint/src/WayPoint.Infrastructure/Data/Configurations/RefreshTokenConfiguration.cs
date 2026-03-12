using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WayPoint.Infrastructure.Identity;

namespace WayPoint.Infrastructure.Data.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.Id);
        builder.Property(rt => rt.Token).HasMaxLength(200).IsRequired();
        builder.Property(rt => rt.UserId).HasMaxLength(450).IsRequired();
        builder.HasIndex(rt => rt.Token).IsUnique();
        builder.HasIndex(rt => rt.UserId);
        builder.HasIndex(rt => rt.ExpiresAt);
    }
}
