using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WeatherMonitoring.Api.Controllers.Locations;
using WeatherMonitoring.Api.Controllers.Locations.Models;
using WeatherMonitoring.Common.ParamsValidator;
using WeatherMonitoring.Services.Locations;

namespace WeatherMonitoring.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Location")]
[Route("api/v{version:apiVersion}/[controller]")]
public class LocationsController : ControllerBase
{
    private readonly ILocationService _locationService;
    private readonly IParamsValidator _paramsValidator;
    private readonly IMapper _mapper;

    public LocationsController(ILocationService locationService, IParamsValidator paramsValidator, IMapper mapper)
    {
        _locationService = locationService;
        _paramsValidator = paramsValidator;
        _mapper = mapper;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll(
        [FromQuery] bool? included, 
        [FromQuery] bool? active,
        [FromQuery] int? limit,
        [FromQuery] int? offset)
    {
        if (limit != null && !_paramsValidator.ValidateLimit(limit.Value).IsValid)
        {
            return BadRequest(_paramsValidator.ValidateLimit(limit.Value).Errors);
        }

        if (offset != null && !_paramsValidator.ValidateOffset(offset.Value).IsValid)
        {
            return BadRequest(_paramsValidator.ValidateOffset(offset.Value).Errors);
        }

        var result = await _locationService.GetAll(included, active, limit, offset);

        return Ok(result);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await _locationService.GetById(id);
        
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("")]
    public async Task<IActionResult> Create(CreateLocationRequestModel model)
    {
        var location = _mapper.Map<CreateLocationModel>(model);

        var result = await _locationService.Create(location);

        return Ok(result);
    }

    [HttpPut("{id:Guid}")]
    public async Task Update([FromRoute] Guid id, UpdateLocationRequestModel model)
    {
        var location = _mapper.Map<UpdateLocationModel>(model);

        await _locationService.Update(id, location);
    }
}
