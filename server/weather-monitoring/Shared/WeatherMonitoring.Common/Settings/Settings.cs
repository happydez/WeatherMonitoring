using Microsoft.Extensions.Configuration;

namespace WeatherMonitoring.Settings;

public static class Settings
{
    public static T Load<T>(string key, IConfiguration configuration = null)
    {
        var settings = (T)Activator.CreateInstance(typeof(T));

        SettingsFactory.Create(configuration)
            .GetSection(key)
            .Bind(settings, (p) =>
            {
                p.BindNonPublicProperties = true;
            });

        return settings;
    }

    public static string GetConnectionString(string provider, IConfiguration configuration = null)
    {
        var connectionString = SettingsFactory.Create(configuration)
            .GetConnectionString(provider);

        return connectionString is null ? throw new KeyNotFoundException($"connection string not found.") : connectionString;
    }
}
