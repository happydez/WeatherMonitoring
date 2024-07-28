namespace WeatherMonitoring.Helpers;

using Microsoft.Extensions.DependencyInjection;

public static class AutoMapperRegisterHelper
{
    public static void Register(IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(s => s.FullName != null && s.FullName.StartsWith("weathermonitoring.", StringComparison.CurrentCultureIgnoreCase));

        services.AddAutoMapper(assemblies);
    }
}
