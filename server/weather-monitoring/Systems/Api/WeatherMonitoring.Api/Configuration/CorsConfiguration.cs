using WeatherMonitoring.Services.Settings;

namespace WeatherMonitoring.Api.Configuration;

public static class CorsConfiguration
{
    public static IServiceCollection AddAppCors(this IServiceCollection services)
    {
        services.AddCors();

        return services;
    }

    public static void UseAppCors(this WebApplication app)
    {
        var mainSettings = app.Services.GetService<MainSettings>();

        var origins = mainSettings.AllowedOrigins.Split(',', ';').Select(x => x.Trim())
            .Where(x => !string.IsNullOrEmpty(x)).ToArray();

        app.UseCors(policy =>
        {
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowCredentials();

            if (origins.Length > 0)
            {
                policy.WithOrigins(origins);
            }
            else
            {
                policy.SetIsOriginAllowed(origin => true);
            }

            policy.WithExposedHeaders("Content-Disposition");
        }
        );
    }
}
