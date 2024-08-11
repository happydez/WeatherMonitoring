using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WeatherMonitoring.Contracts;
using WeatherMonitoring.Entities.Models;
using WeatherMonitoring.Shared.RequestFeatures;

namespace WeatherMonitoring.Repository
{
    public static class RepositorySeeder
    {
        private static IServiceScope ServiceScope(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService<IServiceScopeFactory>()!.CreateScope();
        }

        private static RepositoryContext RepositoryContext(IServiceProvider serviceProvider)
        {
            return ServiceScope(serviceProvider)
                .ServiceProvider.GetRequiredService<IDbContextFactory<RepositoryContext>>().CreateDbContext();
        }

        public static void Init(IServiceProvider serviceProvider, bool initWeatherFromCsv = true)
        {
            Task.Run(async () =>
            {
                using var scope = serviceProvider.CreateScope();

                await AddDemoData(scope.ServiceProvider, initWeatherFromCsv);
            })
                .GetAwaiter()
                .GetResult();
        }

        private static async Task AddDemoData(IServiceProvider serviceProvider, bool initWeatherFromCsv = true)
        {
            await using var context = RepositoryContext(serviceProvider);

            if (await context.Locations!.AnyAsync())
            {
                return;
            }

            if (await context.Weathers!.AnyAsync())
            {
                return;
            }

            var locations = GetLocations();
            await context.Locations!.AddRangeAsync(locations);
            await context.SaveChangesAsync();

            var weathers = initWeatherFromCsv ? GetWeathersFromCsv() : await GetWeathersFromWeatherAPI(serviceProvider);
            await context.Weathers!.AddRangeAsync(weathers);
            await context.SaveChangesAsync();
        }

        private static IEnumerable<Location> GetLocations()
        {
            return
            [
                new Location
                {
                    Id = new Guid("29ba59f0-8781-4a0c-ae0d-306329af7647"),
                    Name = "Voronezh",
                    Region = "Voronezh",
                    Country = "Russia",
                    Lat = 51.67D,
                    Lon = 39.17D,
                    TzId = "Europe/Moscow",
                    Active = true,
                    Included = true,
                },
                new Location
                {
                    Id = new Guid("2bc5bb22-acf7-471b-84eb-6b60c9ea332f"),
                    Name = "Moscow",
                    Region = "Moscow City",
                    Country = "Russia",
                    Lat = 55.75D,
                    Lon = 37.62D,
                    TzId = "Europe/Moscow",
                    Active = true,
                    Included = true,
                },
                new Location
                {
                    Id = new Guid("71c574d7-a8e1-42b8-9ba9-e4a772fccfc9"),
                    Name = "Tokyo",
                    Region = "Tokyo",
                    Country = "Japan",
                    Lat = 35.69D,
                    Lon = 139.69D,
                    TzId = "Asia/Tokyo",
                    Active = true,
                    Included = true,
                },
                new Location
                {
                    Id = new Guid("7e37d49c-1a4e-426a-97b1-fda3ce8aef57"),
                    Name = "London",
                    Region = "City of London, Greater London",
                    Country = "United Kingdom",
                    Lat = 51.52D,
                    Lon = -0.11D,
                    TzId = "Europe/London",
                    Active = false,
                    Included = true,
                },
                new Location
                {
                    Id = new Guid("2a228e53-03f5-4531-a1e9-91107f4c2be1"),
                    Name = "Paris",
                    Region = "Ile-de-France",
                    Country = "France",
                    Lat = 48.87D,
                    Lon = 2.33D,
                    TzId = "Europe/Paris",
                    Active = false,
                    Included = true,
                },
                new Location
                {
                    Id = new Guid("17288828-b285-4523-989c-9b00ba1abd70"),
                    Name = "Volgograd",
                    Region = "Volgograd",
                    Country = "Russia",
                    Lat = 48.8D,
                    Lon = 44.59D,
                    TzId = "Europe/Volgograd",
                    Active = false,
                    Included = true,
                }
            ];
        }

        private static IEnumerable<Location> GetLocationsFromCsv()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RawData", "locations.csv");

            var data = Common.Helpers.CsvHelper.ReadCsvFile<Location, LocationMap>(filePath);

            return data;
        }

        private static IEnumerable<Weather> GetWeathersFromCsv()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RawData", "weathers.csv");

            var data = Common.Helpers.CsvHelper.ReadCsvFile<Weather, WeatherMap>(filePath);

            foreach (var d in data)
            {
                d.LastUpdated = d.LastUpdated.ToUniversalTime();
            }

            return data;
        }

        private static async Task<IEnumerable<Weather>> GetWeathersFromWeatherAPI(IServiceProvider serviceProvider)
        {
            List<Weather> weathers = [];

            var weatherAPI = serviceProvider.GetRequiredService<IWeatherAPI>();

            foreach (var location in GetLocations())
            {
                var q = string.Join(',', location.Name, location.Region, location.Country);

                var today = DateTime.Today;
                var startDate = today.AddMonths(-1);

                for (DateTime date = startDate; date <= today; date = date.AddDays(1))
                {
                    SearchParameters searchParameters = new()
                    {
                        Date = new DateOnly(date.Year, date.Month, date.Day)
                    };

                    var apiResponse = await weatherAPI.GetWeatherHistoryAsync(q, searchParameters);

                    var weatherHistory = apiResponse.Select(r => new Weather
                    {
                        LocationId = location.Id,
                        ConditionCode = r.Weather!.Condition!.Code,
                        ConditionText = r.Weather!.Condition!.Text,
                        TemperatureCelsius = r.Weather.TemperatureCelsius,
                        Humidity = r.Weather.Humidity,
                        PressureIn = r.Weather.PressureIn,
                        WindSpeedKph = r.Weather.WindSpeedKph,
                        LastUpdatedEpoch = r.Weather.LastUpdatedEpoch,
                        LastUpdated = r.Weather.LastUpdated,
                    });

                    if (date == today)
                    {
                        var now = DateTime.Now;
                        var timeZoneId = TimeZoneInfo.FindSystemTimeZoneById(location.TzId!);
                        var time = TimeZoneInfo.ConvertTime(now, timeZoneId);

                        weatherHistory = weatherHistory.Where(w => w.LastUpdated <= time.AddMinutes(-30));
                    }

                    weathers.AddRange(weatherHistory);
                }
            }

            return weathers;
        }
    }

    internal sealed class LocationMap : ClassMap<Location>
    {
        public LocationMap()
        {
            Map(l => l.Id).Index(0);
            Map(l => l.Name).Index(1);
            Map(l => l.Region).Index(2);
            Map(l => l.Country).Index(3);
            Map(l => l.Lat).Index(4);
            Map(l => l.Lon).Index(5);
            Map(l => l.TzId).Index(6);
            Map(l => l.Active).Index(7);
            Map(l => l.Included).Index(8);
        }
    }

    internal sealed class WeatherMap : ClassMap<Weather>
    {
        public WeatherMap()
        {
            Map(w => w.LocationId).Index(0);
            Map(w => w.ConditionCode).Index(1);
            Map(w => w.TemperatureCelsius).Index(2);
            Map(w => w.Humidity).Index(3);
            Map(w => w.PressureIn).Index(4);
            Map(w => w.WindSpeedKph).Index(5);
            Map(w => w.LastUpdatedEpoch).Index(6);
            Map(w => w.LastUpdated).Index(7);
            Map(w => w.ConditionText).Index(8);
        }
    }
}
