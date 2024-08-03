using WeatherMonitoring.Context.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WeatherMonitoring.Context;

public class LocationConfiguration : BaseEntityConfiguration<Location>
{
    public override void Configure(EntityTypeBuilder<Location> entity)
    {
        base.Configure(entity);

        entity.HasKey(p => p.Id);
        entity.ToTable("Location", schema: "wmapp");

        entity.Property(p => p.Name).IsRequired().HasMaxLength(48).HasColumnName("name");
        entity.Property(p => p.Region).HasMaxLength(48).HasColumnName("region");
        entity.Property(p => p.Country).HasMaxLength(48).HasColumnName("country");

        entity.Property(p => p.Lat).IsRequired().HasColumnName("lat");
        entity.Property(p => p.Lon).IsRequired().HasColumnName("lon");

        entity.Property(p => p.TzId).IsRequired().HasColumnName("tz_id");

        entity.Property(p => p.Active).HasColumnName("active").HasDefaultValue(true);
        entity.Property(p => p.Included).HasColumnName("included").HasDefaultValue(false);

        entity.HasMany(l => l.Weathers).WithOne(w => w.Location)
            .HasForeignKey(w => w.LocationId);

        entity.HasIndex(p => p.Uid).IsUnique();
    }
}