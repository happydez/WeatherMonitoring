using FluentValidation.AspNetCore;
using WeatherMonitoring.Common.Validator;
using WeatherMonitoring.Helpers;


namespace WeatherMonitoring.Api.Configuration;

public static class ValidatorConfiguration
{
    public static IServiceCollection AddAppValidator(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation(opt => { opt.DisableDataAnnotationsValidation = true; });

        ValidatorRegisterHelper.Register(services);

        return services;
    }
}
