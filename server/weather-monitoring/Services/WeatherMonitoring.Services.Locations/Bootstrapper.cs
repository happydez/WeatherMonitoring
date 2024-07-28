using Microsoft.Extensions.DependencyInjection;

namespace WeatherMonitoring.Services.Locations;

public static class Bootstrapper
{
    public static IServiceCollection AddLocationService(this IServiceCollection services)
    {
        return services.AddSingleton<ILocationService, LocationService>();
    }
}
