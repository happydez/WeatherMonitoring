using Microsoft.EntityFrameworkCore;
using WeatherMonitoring.Entities.Models;
using WeatherMonitoring.Repository.Configuration;

namespace WeatherMonitoring.Repository
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LocationConfiguration());
            modelBuilder.ApplyConfiguration(new WeatherConfiguration());
        }

        public DbSet<Location>? Locations { get; set; }
        public DbSet<Weather>? Weathers { get; set; }
    }
}
