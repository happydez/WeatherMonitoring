using AutoMapper;
using WeatherMonitoring.Services.Locations;

namespace WeatherMonitoring.Api.Controllers.Locations;

public class UpdateLocationRequestModel
{
    public bool Active { get; set; }
    public bool Included { get; set; }
}

public class UpdateLocationRequestModelProfile : Profile
{
    public UpdateLocationRequestModelProfile()
    {
        CreateMap<UpdateLocationRequestModel, UpdateLocationModel>();
    }
}
