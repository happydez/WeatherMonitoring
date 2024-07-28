using static WeatherMonitoring.Settings.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace WeatherMonitoring.Services.Settings;

public static class Bootstrapper
{
    public static IServiceCollection AddLogSettings(this IServiceCollection services, IConfiguration configuration = null)
    {
        var settings = Load<LogSettings>("Log", configuration);
        services.AddSingleton<LogSettings>(settings);

        return services;
    }

    public static IServiceCollection AddMainSettings(this IServiceCollection services, IConfiguration configuration = null)
    {
        var settings = Load<MainSettings>("Main", configuration);
        services.AddSingleton<MainSettings>(settings);

        return services;
    }

    public static IServiceCollection AddSwaggerSettings(this IServiceCollection services, IConfiguration configuration = null)
    {
        var settings = Load<SwaggerSettings>("Swagger", configuration);
        services.AddSingleton<SwaggerSettings>(settings);

        return services;
    }
}
