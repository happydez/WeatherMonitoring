using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using WeatherMonitoring.Common.ParamsValidator;
using WeatherMonitoring.Services.Weathers;

namespace WeatherMonitoring.Api.Controllers.Weathers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Weather")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WeathersController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly IParamsValidator _paramsValidator;

        public WeathersController(IWeatherService weatherService, IParamsValidator paramsValidator)
        {
            _weatherService = weatherService;
            _paramsValidator = paramsValidator;
        }

        [HttpGet("{locationId:Guid}")]
        public async Task<IActionResult> Get(
            [FromRoute] Guid locationId, 
            [FromQuery] int? limit, 
            [FromQuery] int? offset,
            [FromQuery] bool? desc,
            [FromQuery] DateTime? dt1,
            [FromQuery] DateTime? dt2)
        {
            if (limit != null && !_paramsValidator.ValidateLimit(limit.Value).IsValid)
            {
                return BadRequest(_paramsValidator.ValidateLimit(limit.Value).Errors);
            }

            if (offset != null && !_paramsValidator.ValidateOffset(offset.Value).IsValid)
            {
                return BadRequest(_paramsValidator.ValidateOffset(offset.Value).Errors);
            }

            var result = await _weatherService.GetAllById(locationId, limit, offset, desc, dt1, dt2);

            return Ok(result);
        }
    }
}
