namespace WeatherMonitoring.Context.Seeder;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Collections.Generic;
using WeatherMonitoring.Context.Entities;
using WeatherMonitoring.Services.WeatherApi;
using WeatherMonitoring.Common;
using Npgsql;
using CsvHelper.Configuration;

#pragma warning disable IDE0090

public static class DemoHelper
{
    public static IEnumerable<Entities.Location> GetLocations()
    {
        var path = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            @"Seeds/Demo/WeatherDemoData",
            "locations.json");

        var json = File.ReadAllText(path);
        var locations = JsonConvert.DeserializeObject<IEnumerable<Entities.Location>>(json);

        return locations;
    }

    public static async Task InitWeathersFromCsv(IServiceProvider provider, string path)
    {
        var records = FileExtensions.ReadCsvFile<Weather, WeatherMap>(path);

        using var context = provider.GetRequiredService<IDbContextFactory<MainDbContext>>().CreateDbContext();

        using var connection = (NpgsqlConnection)context.Database.GetDbConnection();
        await connection.OpenAsync();

        using var writer = await connection.BeginBinaryImportAsync("COPY wmapp.\"Weather\" (\"LocationId\", c_code, temp_c, humidity, pressure_in, wind_kph, lastup_epoch, lastup, c_text) FROM STDIN (FORMAT BINARY)");

        foreach (var record in records)
        {
            await writer.StartRowAsync();
            await writer.WriteAsync(record.LocationId);
            await writer.WriteAsync(record.ConditionCode);
            await writer.WriteAsync(record.TemperatureCelsius);
            await writer.WriteAsync(record.Humidity);
            await writer.WriteAsync(record.PressureIn);
            await writer.WriteAsync(record.WindSpeedKph);

            var lastUpdated = record.LastUpdated.ToUniversalTime();
            await writer.WriteAsync(record.LastUpdatedEpoch);
            await writer.WriteAsync(lastUpdated);

            await writer.WriteAsync(record.ConditionText);
        }

        await writer.CompleteAsync();
    }

    public sealed class WeatherMap : ClassMap<Weather>
    {
        public WeatherMap()
        {
            Map(m => m.LocationId).Index(0);
            Map(m => m.ConditionCode).Index(1);
            Map(m => m.TemperatureCelsius).Index(2);
            Map(m => m.Humidity).Index(3);
            Map(m => m.PressureIn).Index(4);
            Map(m => m.WindSpeedKph).Index(5);
            Map(m => m.LastUpdatedEpoch).Index(6);
            Map(m => m.LastUpdated).Index(7);
            Map(m => m.ConditionText).Index(8);
        }
    }

    // Raw location initialization via weather api call
    public static async Task<IEnumerable<Weather>> GetWeathers(IServiceProvider provider)
    {
        List<Weather> weathers = new();

        foreach (var location in GetLocations())
        {
            var waq = WeatherApiQuery.CreateBuilder(location.Name)
                .WithRegion(location.Region).WithCountry(location.Country).Build();

            var today = DateTime.Today;
            var startDate = today.AddMonths(-1);

            for (DateTime date = startDate; date <= today; date = date.AddDays(1))
            {
                var result = await InitWeatherHistoryAsync(provider, waq, new DateOnly(date.Year, date.Month, date.Day));

                if (date == today)
                {
                    var now = DateTime.Now;
                    var timeZoneId = TimeZoneInfo.FindSystemTimeZoneById(location.TzId);
                    var time = TimeZoneInfo.ConvertTime(now, timeZoneId);

                    result = result.Where(w => w.LastUpdated <= time.AddMinutes(-30));
                }

                weathers.AddRange(result);
            }
        }

        return weathers;
    }

    private static async Task<IEnumerable<Weather>> InitWeatherHistoryAsync(IServiceProvider provider, WeatherApiQuery weatherApiQuery, DateOnly date)
    {
        var weatherApi = provider.GetRequiredService<IWeatherApi>();
        var mapper = provider.GetRequiredService<IMapper>();

        var history = await weatherApi.GetWeatherHistoryAsync(weatherApiQuery, date);
        var result = mapper.Map<IEnumerable<Weather>>(history);

        return result;
    }

}

public class WeatherApiHistoryResponseProfile : Profile
{
    public WeatherApiHistoryResponseProfile()
    {
        CreateMap<WeatherApiHistoryResponse, Weather>()
            .BeforeMap<WeatherApiHistoryResponseAction>()
            .ForMember(dest => dest.LocationId, opt => opt.Ignore())
            .ForMember(dest => dest.Location, opt => opt.Ignore());

        CreateMap<Forecastday, Weather>()
            .ForMember(dest => dest.TemperatureCelsius, opt => opt.MapFrom(src => src.TemperatureCelsius))
            .ForMember(dest => dest.Humidity, opt => opt.MapFrom(src => src.Humidity))
            .ForMember(dest => dest.PressureIn, opt => opt.MapFrom(src => src.PressureIn))
            .ForMember(dest => dest.WindSpeedKph, opt => opt.MapFrom(src => src.WindSpeedKph))
            .ForMember(dest => dest.LastUpdatedEpoch, opt => opt.MapFrom(src => src.TimeEpoch))
            .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(src => src.Time));

        CreateMap<WeatherCondition, Weather>()
            .ForMember(dest => dest.ConditionCode, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.ConditionText, opt => opt.MapFrom(src => src.Text));
    }

    public class WeatherApiHistoryResponseAction : IMappingAction<WeatherApiHistoryResponse, Weather>
    {
        private readonly IDbContextFactory<MainDbContext> _contextFactory;

        public WeatherApiHistoryResponseAction(IDbContextFactory<MainDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void Process(WeatherApiHistoryResponse source, Weather destination, ResolutionContext context)
        {
            using var db = _contextFactory.CreateDbContext();

            var location = db.Locations
                .FirstOrDefault(loc => loc.Name == source.Location.Name &&
                                        loc.Country == source.Location.Country &&
                                        loc.Region == source.Location.Region);

            if (location != null)
            {
                destination.LocationId = location.Id;
            }

            context.Mapper.Map(source.Condition, destination);
            context.Mapper.Map(source.Forecastday, destination);
        }
    }
}