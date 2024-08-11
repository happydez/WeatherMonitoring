using AutoMapper;
using WeatherMonitoring.Entities.Models;
using WeatherMonitoring.Entities.WeatherAPIModels;
using WeatherMonitoring.Shared.DataTransferObjects;

#nullable disable
namespace WeatherMonitoring.Server
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Location, LocationDto>();
            CreateMap<Weather, WeatherDto>();
            CreateMap<LocationForCreationDto, Location>();
            CreateMap<WeatherForCreationDto, Weather>();
            CreateMap<LocationForUpdateDto, Location>().ReverseMap();

            CreateMap<APIResponse, LocationForManipulationDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Location.Name))
                .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Location.Region))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Location.Country))
                .ForMember(dest => dest.Lat, opt => opt.MapFrom(src => src.Location.Lat))
                .ForMember(dest => dest.Lon, opt => opt.MapFrom(src => src.Location.Lon));

            CreateMap<APIResponse, WeatherForCreationDto>()
                .ForMember(dest => dest.ConditionCode, opt => opt.MapFrom(src => src.Weather.Condition.Code))
                .ForMember(dest => dest.ConditionText, opt => opt.MapFrom(src => src.Weather.Condition.Text))
                .ForMember(dest => dest.TemperatureCelsius, opt => opt.MapFrom(src => src.Weather.TemperatureCelsius))
                .ForMember(dest => dest.Humidity, opt => opt.MapFrom(src => src.Weather.Humidity))
                .ForMember(dest => dest.PressureIn, opt => opt.MapFrom(src => src.Weather.PressureIn))
                .ForMember(dest => dest.WindSpeedKph, opt => opt.MapFrom(src => src.Weather.WindSpeedKph))
                .ForMember(dest => dest.LastUpdatedEpoch, opt => opt.MapFrom(src => src.Weather.LastUpdatedEpoch))
                .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(src => src.Weather.LastUpdated));

        }
    }
}
