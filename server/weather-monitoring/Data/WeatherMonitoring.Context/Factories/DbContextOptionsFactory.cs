using Microsoft.EntityFrameworkCore;

namespace WeatherMonitoring.Context;

public static class DbContextOptionsFactory
{
    private const string migrationProjectPrefix = "WeatherMonitoring.Context.Migrations.";

    public static DbContextOptions<MainDbContext> Create(string connectionString, DbType dbType, bool detailedLogging = false)
    {
        var builder = new DbContextOptionsBuilder<MainDbContext>();

        Configure(connectionString, dbType, detailedLogging).Invoke(builder);

        return builder.Options;
    }

    public static Action<DbContextOptionsBuilder> Configure(string connectionString, DbType dbType, bool detailedLogging = false)
    {
        return (builder) =>
        {
            switch (dbType)
            {
                case DbType.PgSql:
                    builder.UseNpgsql(connectionString, options =>
                    {
                        options.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
                        options.MigrationsHistoryTable("_migrations");
                        options.MigrationsAssembly($"{migrationProjectPrefix}{DbType.PgSql}");
                    });
                    break;
                default:
                    throw new NotSupportedException($"Provider {dbType} is not available at this time.");
            }

            if (detailedLogging)
            {
                builder.EnableSensitiveDataLogging();
            }

            builder.UseLazyLoadingProxies();
        };
    }
}
