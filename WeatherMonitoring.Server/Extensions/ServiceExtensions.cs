using Microsoft.EntityFrameworkCore;
using WeatherMonitoring.Contracts;
using WeatherMonitoring.LoggerService;
using WeatherMonitoring.Repository;
using WeatherMonitoring.Service;
using WeatherMonitoring.ServiceContracts;
using WeatherMonitoring.WeatherAPI;
using WeatherMonitoring.Entities.Exceptions;
using Asp.Versioning;

namespace WeatherMonitoring.Server.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                    builder.WithExposedHeaders("X-Pagination");
                });
            });
        }

        public static void ConfigureAPIVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.SubstituteApiVersionInUrl = true;
                options.GroupNameFormat = "'v'VVV";
                options.FormatGroupName = (group, version) => $"{group} - {version}";
            });
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void ConfigureServiceManager(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
        }

        public static void ConfigureRepositoryManager(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }

        public static void ConfigureSqlContextFactory(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextFactory<RepositoryContext>((builder) =>
            {
                builder.UseNpgsql(configuration.GetConnectionString("sqlConnection"),
                b => b.MigrationsAssembly("WeatherMonitoring.Server"));
            });
        }

        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RepositoryContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("sqlConnection"));
            });
        }

        public static void ConfigureWeatherAPI(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<WeatherAPISettings>(configuration.GetSection("WeatherAPI"));

            services.AddHttpClient<IWeatherAPI, WeatherAPI.WeatherAPI>(client =>
            {
                var uri = configuration.GetSection("WeatherAPI:URI").Value ?? throw new WeatherAPIURINotFoundException();
                client.BaseAddress = new Uri(uri);
            });
        }
    }
}
