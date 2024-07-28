using Microsoft.Extensions.DependencyInjection;

namespace WeatherMonitoring.Services.Logger;

public static class Bootstrapper
{
    public static IServiceCollection AddAppLogger(this IServiceCollection services)
    {
        return services.AddSingleton<IAppLogger, AppLogger>();
    }
}
