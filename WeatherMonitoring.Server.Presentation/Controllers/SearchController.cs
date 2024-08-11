using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using WeatherMonitoring.ServiceContracts;

namespace WeatherMonitoring.Server.Presentation.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Location")]
    [Route("api/v{version:apiVersion}/locations/search")]
    public class SearchController : ControllerBase
    {
        private readonly IServiceManager _service;

        public SearchController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> SearchLocations([FromQuery] string q)
        {
            var locations = await _service.WeatherAPIService.SearchLocationsAsync(q);

            return Ok(locations);
        }
    }
}
