using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using WeatherMonitoring.Common.ParamsValidator;

namespace WeatherMonitoring.Helpers;

public static class ValidatorRegisterHelper
{
    public static void Register(IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(s => s.FullName != null && s.FullName.StartsWith("weathermonitoring.", StringComparison.CurrentCultureIgnoreCase));

        assemblies.ToList().ForEach(x => 
        { 
            services.AddValidatorsFromAssembly(x, ServiceLifetime.Singleton);
        });

        services.AddSingleton<IParamsValidator, ParamsValidator>();
    }
}
