using Microsoft.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static WeatherMonitoring.Settings.Settings;

namespace WeatherMonitoring.Services.WeatherApi;

public static class Bootstrapper
{
    public static IServiceCollection AddWeatherApi(this IServiceCollection services, IConfiguration configuration = null)
    {
        var settings = Load<WeatherApiSettings>("WeatherApi", configuration);
        services.AddSingleton<WeatherApiSettings>(settings);

        services.AddHttpClient<IWeatherApi, WeatherApi>(client =>
        {
            client.BaseAddress = new Uri(@"https://api.weatherapi.com/v1/");
        });

        return services;
    }
}
