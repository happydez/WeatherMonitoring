using AutoMapper;
using WeatherMonitoring.Services.Locations;

namespace WeatherMonitoring.Api.Controllers.Locations.Models;

#nullable disable
public class CreateLocationRequestModel
{
    public string Name { get; set; }
    public string Region { get; set; }
    public string Country { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
    public bool Included { get; set; }
    public bool Active { get; set; }
}

public class CreateLocationRequestModelProfile : Profile
{
    public CreateLocationRequestModelProfile()
    {
        CreateMap<CreateLocationRequestModel, CreateLocationModel>();
    }
}