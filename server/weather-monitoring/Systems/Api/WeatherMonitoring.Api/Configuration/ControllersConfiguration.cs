using Microsoft.AspNetCore.Mvc;
using WeatherMonitoring.Common.Extensions;

namespace WeatherMonitoring.Api.Configuration;

public static class ControllersConfiguration
{
    public static IServiceCollection AddAppControllers(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddNewtonsoftJson(options => options.SerializerSettings.SetDefaultSettings())
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                    new BadRequestObjectResult(context.ModelState.ToErrorResponse());
            });

        return services;
    }

    public static IEndpointRouteBuilder UseAppControllers(this IEndpointRouteBuilder app)
    {
        app.MapControllers();

        return app;
    }
}