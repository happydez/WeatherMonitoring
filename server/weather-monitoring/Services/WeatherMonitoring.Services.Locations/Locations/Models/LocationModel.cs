using AutoMapper;
using WeatherMonitoring.Context.Entities;

namespace WeatherMonitoring.Services.Locations;

public class LocationModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Region { get; set; }
    public string Country { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
    public string TzId { get; set; }
    public bool Active { get; set; }
    public bool Included { get; set; }
}

public class LocationModelProfile : Profile
{
    public LocationModelProfile()
    {
        CreateMap<Location, LocationModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Uid))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Region))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
            .ForMember(dest => dest.Lat, opt => opt.MapFrom(src => src.Lat))
            .ForMember(dest => dest.Lon, opt => opt.MapFrom(src => src.Lon))
            .ForMember(dest => dest.TzId, opt => opt.MapFrom(src => src.TzId))
            .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
            .ForMember(dest => dest.Included, opt => opt.MapFrom(src => src.Included)
            );
    }
}
