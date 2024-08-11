using Microsoft.AspNetCore.Mvc;
using WeatherMonitoring.Shared.RequestFeatures;
using WeatherMonitoring.Server.Presentation.ActionFilters;
using WeatherMonitoring.ServiceContracts;
using WeatherMonitoring.Shared.DataTransferObjects;
using System.Text.Json;
using Asp.Versioning;

namespace WeatherMonitoring.Server.Presentation.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Location")]
    [Route("api/v{version:apiVersion}/locations/{locationId}/weather")]
    public class WeatherController : ControllerBase
    {
        private readonly IServiceManager _service;

        public WeatherController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetWeathersForLocation(Guid locationId, [FromQuery] WeatherParameters weatherParameters)
        {
            var pagedResult = await _service.WeatherService.GetWeathersAsync(locationId, weatherParameters, trackChanges: false);

            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(pagedResult.metaData);

            return Ok(pagedResult.weathers);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateWeatherForLocation(Guid locationId, [FromBody] WeatherForCreationDto weather)
        {
            var weatherToReturn = await _service.WeatherService.CreateWeatherForLocationAsync(locationId, weather, trackChanges: false);

            return CreatedAtAction(nameof(GetWeathersForLocation), new { locationId }, weatherToReturn);
        }
    }
}
