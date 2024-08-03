using Microsoft.Extensions.DependencyInjection;

namespace WeatherMonitoring.Services.Weathers;

public static class Bootstrapper
{
    public static IServiceCollection AddWeatherService(this IServiceCollection services)
    {
        return services.AddSingleton<IWeatherService, WeatherService>();
    }

}
