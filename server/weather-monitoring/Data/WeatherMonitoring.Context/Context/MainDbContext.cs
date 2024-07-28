using Microsoft.EntityFrameworkCore;
using WeatherMonitoring.Context.Entities;

namespace WeatherMonitoring.Context;

public class MainDbContext : DbContext
{
    public DbSet<Weather> Weathers { get; set; } = null!;
    public DbSet<Location> Locations { get; set; } = null!;

    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new WeatherConfiguration());
        modelBuilder.ApplyConfiguration(new LocationConfiguration());
    }

}
