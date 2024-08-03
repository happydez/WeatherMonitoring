using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WeatherMonitoring.Services.WeatherApi;

namespace WeatherMonitoring.Context.Seeder;

public static class DbSeeder
{
    private static IServiceScope ServiceScope(IServiceProvider serviceProvider)
    {
        return serviceProvider.GetService<IServiceScopeFactory>()!.CreateScope();
    }

    private static MainDbContext DbContext(IServiceProvider serviceProvider)
    {
        return ServiceScope(serviceProvider)
            .ServiceProvider.GetRequiredService<IDbContextFactory<MainDbContext>>().CreateDbContext();
    }

    public static void Execute(IServiceProvider serviceProvider)
    {
        Task.Run(async () =>
        {
            await AddDemoData(serviceProvider);
        })
        .GetAwaiter()
        .GetResult();
    }

    private static async Task AddDemoData(IServiceProvider serviceProvider)
    {
        using var scope = ServiceScope(serviceProvider);
        if (scope == null)
        {
            return;
        }

        var settings = scope.ServiceProvider.GetService<DbSettings>();
        if (!(settings.Init?.AddDemoData ?? false))
        {
            return;
        }

        await using var context = DbContext(serviceProvider);

        if (await context.Locations.AnyAsync())
        {
            return;
        }

        if (await context.Weathers.AnyAsync())
        {
            return;
        }

        var locations = DemoHelper.GetLocations();
        await context.Locations.AddRangeAsync(locations);
        await context.SaveChangesAsync();

        // Raw location initialization via weather api call
        // var weathers = await DemoHelper.GetWeathers(serviceProvider);
        // await context.Weathers.AddRangeAsync(weathers);

        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            @"Seeds/Demo/WeatherDemoData", "weathers.csv");
        
        await DemoHelper.InitWeathersFromCsv(serviceProvider, path);

        await context.SaveChangesAsync();
    }
}