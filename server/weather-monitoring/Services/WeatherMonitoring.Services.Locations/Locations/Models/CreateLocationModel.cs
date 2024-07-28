using AutoMapper;
using WeatherMonitoring.Context.Entities;

namespace WeatherMonitoring.Services.Locations;

public class CreateLocationModel
{
    public string Name { get; set; }
    public string Region { get; set; }
    public string Country { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
    public bool Included { get; set; }
    public bool Active { get; set; }
}

public class CreateLocationModelProfile : Profile
{
    public CreateLocationModelProfile()
    {
        CreateMap<CreateLocationModel, Location>();
    }
}