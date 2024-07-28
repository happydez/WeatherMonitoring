using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using WeatherMonitoring.Api.Controllers.Search;
using WeatherMonitoring.Services.WeatherApi;

namespace WeatherMonitoring.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Search")]
[Route("api/v{version:apiVersion}/[controller]")]
public class SearchController : ControllerBase
{
    private readonly IWeatherApi _weatherApi;
    private readonly IMapper _mapper;

    public SearchController(IWeatherApi weatherApi, IMapper mapper)
    {
        _weatherApi = weatherApi;
        _mapper = mapper;
    }

    [HttpGet("")]
    public async Task<IEnumerable<SearchResponse>> Search([Required] string qs, [BindRequired] string q)
    {
        var waq = WeatherApiQuery.CreateBuilder(q).Build();

        var location = await _weatherApi.SearchLocationAsync(waq);

        var result = _mapper.Map<IEnumerable<SearchResponse>>(location);

        return result;
    }
}
