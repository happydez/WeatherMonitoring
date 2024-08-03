using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WeatherMonitoring.Context.Entities;

namespace WeatherMonitoring.Context;

public class WeatherConfiguration : BaseEntityConfiguration<Weather>
{
    public override void Configure(EntityTypeBuilder<Weather> entity)
    {
        base.Configure(entity);

        entity.HasKey(p => p.Id);
        entity.ToTable("Weather", schema: "wmapp");

        entity.Property(p => p.Humidity).IsRequired().HasColumnName("humidity");
        entity.Property(p => p.PressureIn).IsRequired().HasColumnName("pressure_in");
        entity.Property(p => p.WindSpeedKph).IsRequired().HasColumnName("wind_kph");
        entity.Property(p => p.TemperatureCelsius).IsRequired().HasColumnName("temp_c");
        entity.Property(p => p.ConditionCode).IsRequired().HasColumnName("c_code");
        entity.Property(p => p.ConditionText).IsRequired().HasColumnName("c_text");

        entity.Property(p => p.LastUpdatedEpoch).IsRequired().HasColumnName("lastup_epoch");
        entity.Property(p => p.LastUpdated).IsRequired().HasColumnName("lastup");

        entity.Ignore(p => p.Uid);
    }
}