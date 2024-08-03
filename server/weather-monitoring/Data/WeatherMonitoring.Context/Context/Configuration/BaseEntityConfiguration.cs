using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeatherMonitoring.Context.Entities;

namespace WeatherMonitoring.Context;

public class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> entity)
    {
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.Uid).IsUnique();
    }
}