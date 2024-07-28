using AutoMapper;
using WeatherMonitoring.Context.Entities;

namespace WeatherMonitoring.Services.Locations;

public class UpdateLocationModel
{
    public bool Active { get; set; }
    public bool Included { get; set; }
}

public class UpdateLocationModelProfile : Profile
{
    public UpdateLocationModelProfile()
    {
        CreateMap<UpdateLocationModel, Location>();
    }
}
