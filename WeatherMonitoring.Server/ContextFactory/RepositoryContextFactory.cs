using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using WeatherMonitoring.Repository;

namespace WeatherMonitoring.Server.ContextFactory
{
    public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
    {
        public RepositoryContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json")
                .Build();

            var builder = new DbContextOptionsBuilder()
                .UseNpgsql(configuration.GetConnectionString("sqlConnection"),
                b => b.MigrationsAssembly("WeatherMonitoring.Server"));

            return new RepositoryContext(builder.Options);
        }
    }
}
