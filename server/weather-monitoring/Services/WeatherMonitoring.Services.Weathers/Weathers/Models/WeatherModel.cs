using AutoMapper;
using WeatherMonitoring.Context.Entities;

namespace WeatherMonitoring.Services.Weathers;

public class WeatherModel
{
    public int ConditionCode { get; set; }
    public string ConditionText { get; set; }
    public double TemperatureCelsius { get; set; }
    public int Humidity { get; set; }
    public double PressureIn { get; set; }
    public double WindSpeedKph { get; set; }
    public long LastUpdatedEpoch { get; set; }
    public DateTime LastUpdated { get; set; }
}

public class WeatherModelProfile : Profile
{
    public WeatherModelProfile()
    {
        CreateMap<Weather, WeatherModel>();
    }
}