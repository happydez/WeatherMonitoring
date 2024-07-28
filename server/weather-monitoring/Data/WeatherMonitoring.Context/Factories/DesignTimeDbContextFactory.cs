using Microsoft.EntityFrameworkCore.Design;
using static WeatherMonitoring.Settings.Settings;

namespace WeatherMonitoring.Context;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MainDbContext>
{
    public MainDbContext CreateDbContext(string[] args)
    {
        var provider = (args?[0] ?? $"{DbType.PgSql}").ToLower();
        var connectionString = GetConnectionString(provider);

        DbType dbType = DbType.PgSql;

        if (!provider.Equals($"{dbType}".ToLower()))
        {
            throw new Exception($"Unsupported provider: {provider}");
        }

        var options = DbContextOptionsFactory.Create(connectionString, dbType, false);
        var factory = new DbContextFactory(options);
        var context = factory.Create();

        return context;
    }
}
