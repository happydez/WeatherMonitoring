using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using WeatherMonitoring.Context;
using WeatherMonitoring.Services.WeatherApi;

namespace WeatherMonitoring.Services.Weathers;

public class CreateWeatherModel
{
    public Guid LocationId { get; set; }
    public int ConditionCode { get; set; }
    public string ConditionText { get; set; }
    public double TemperatureCelsius { get; set; }
    public int Humidity { get; set; }
    public double PressureIn { get; set; }
    public double WindSpeedKph { get; set; }
    public long LastUpdatedEpoch { get; set; }
    public DateTime LastUpdated { get; set; }
}

public class CreateWeatherModelProfile : Profile
{
    public CreateWeatherModelProfile()
    {
        CreateMap<WeatherApiResponse, CreateWeatherModel>()
            .ForMember(dest => dest.ConditionCode, opt => opt.MapFrom(src => src.Current.Condition.Code))
            .ForMember(dest => dest.ConditionText, opt => opt.MapFrom(src => src.Current.Condition.Text))
            .ForMember(dest => dest.TemperatureCelsius, opt => opt.MapFrom(src => src.Current.TemperatureCelsius))
            .ForMember(dest => dest.WindSpeedKph, opt => opt.MapFrom(src => src.Current.WindSpeedKph))
            .ForMember(dest => dest.Humidity, opt => opt.MapFrom(src => src.Current.Humidity))
            .ForMember(dest => dest.PressureIn, opt => opt.MapFrom(src => src.Current.PressureIn))
            .ForMember(dest => dest.LastUpdatedEpoch, opt => opt.MapFrom(src => src.Current.LastUpdatedEpoch))
            .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(src => src.Current.LastUpdated))
            .ForMember(dest => dest.LocationId, opt => opt.Ignore());

        CreateMap<CreateWeatherModel, Context.Entities.Weather>()
            .ForMember(dest => dest.LocationId, opt => opt.Ignore())
            .AfterMap<CreateWeatherModelActions>();
    }

    public class CreateWeatherModelActions : IMappingAction<CreateWeatherModel, Context.Entities.Weather>
    {
        private readonly IDbContextFactory<MainDbContext> _dbContextFactory;

        public CreateWeatherModelActions(IDbContextFactory<MainDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async void Process(CreateWeatherModel source, Context.Entities.Weather destination, ResolutionContext context)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();

            var location = db.Locations.FirstOrDefault(loc => loc.Uid == source.LocationId);

            destination.LocationId = location.Id;
        }
    }

}