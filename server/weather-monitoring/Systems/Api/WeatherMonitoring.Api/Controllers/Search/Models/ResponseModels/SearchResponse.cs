using AutoMapper;
using Newtonsoft.Json;
using WeatherMonitoring.Services.WeatherApi;

namespace WeatherMonitoring.Api.Controllers.Search;

#nullable disable
public class SearchResponse
{
    public string Name { get; set; }
    public string Region { get; set; }
    public string Country { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
}

public class SearchResponseProfile : Profile
{
    public SearchResponseProfile()
    {
        CreateMap<WeatherApiSearchResponse, SearchResponse>();
    }
}