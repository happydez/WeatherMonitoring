using Microsoft.Extensions.Configuration;

namespace WeatherMonitoring.Settings;

public static class SettingsFactory
{
    public static IConfiguration Create(IConfiguration configuration = null)
    {
        var cfg = configuration ?? new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile("appsettings.Development.json", false)
            .AddJsonFile("appsettings.context.json", false)
            .AddEnvironmentVariables()
            .Build();

        return cfg;
    }
}

