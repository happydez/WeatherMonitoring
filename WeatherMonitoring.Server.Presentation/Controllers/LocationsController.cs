using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WeatherMonitoring.Server.Presentation.ActionFilters;
using WeatherMonitoring.Server.Presentation.ModelBinders;
using WeatherMonitoring.ServiceContracts;
using WeatherMonitoring.Shared.DataTransferObjects;
using WeatherMonitoring.Shared.RequestFeatures;

namespace WeatherMonitoring.Server.Presentation.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Location")]
    [Route("api/v{version:apiVersion}/locations")]
    public class LocationsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IServiceManager _service;

        public LocationsController(IServiceManager service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetLocations([FromQuery] LocationParameters locationParameters)
        {
            var pagedResult = await _service.LocationService.GetAllLocationsAsync(locationParameters, trackChanges: false);

            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(pagedResult.metaData);

            return Ok(pagedResult.locations);
        }

        [HttpGet("{id:guid}", Name = "LocationById")]
        public async Task<IActionResult> GetLocation(Guid id)
        {
            var location = await _service.LocationService.GetLocationAsync(id, trackChanges: false);

            return Ok(location);
        }

        [HttpGet("collection/({ids})", Name = "LocationCollection")]
        public async Task<IActionResult> GetLocationCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            var locations = await _service.LocationService.GetByIdsAsync(ids, trackChanges: false);

            return Ok(locations);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateLocationCollection([FromBody] IEnumerable<LocationForCreationDto> locationCollection)
        {
            var weathersDto = new List<WeatherForCreationDto>();

            foreach (var location in locationCollection)
            {
                string q = string.Join(',', location.Name, location.Region, location.Country);
                var apiResponse = await _service.WeatherAPIService.GetWeatherDataAsync(q);

                location.TzId = apiResponse.Location!.TzId;

                var weatherDto = _mapper.Map<WeatherForCreationDto>(apiResponse);
                weathersDto.Add(weatherDto);
            }

            var createdLocations = await _service.LocationService.CreateLocationCollectionAsync(locationCollection);

            var weatherParametrs = new WeatherParameters()
            {
                Offset = 0,
                Limit = 1,
                Desc = true
            };

            int wi = 0;
            foreach (var createdLocation in createdLocations.locations)
            {
                bool needWeather = false;
                var weather = (await _service.WeatherService.GetWeathersAsync(createdLocation.Id, weatherParametrs, trackChanges: false)).weathers;

                if (weather.Count() == 0)
                {
                    needWeather = true;
                }
                else
                {
                    var now = DateTime.Now;
                    var timeZoneId = TimeZoneInfo.FindSystemTimeZoneById(createdLocation.TzId!);
                    var time = TimeZoneInfo.ConvertTime(now, timeZoneId);

                    if (weather.First().LastUpdated <= time.AddMinutes(-30))
                    {
                        needWeather = true;
                    }
                }

                if (needWeather)
                {
                    await _service.WeatherService.CreateWeatherForLocationAsync(createdLocation.Id, weathersDto[wi], trackChanges: false);
                }

                wi++;
            }

            return CreatedAtRoute("LocationCollection", new { createdLocations.ids }, createdLocations.locations);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateLocation([FromBody] LocationForCreationDto location)
        {
            string q = string.Join(',', location.Name, location.Region, location.Country);
            var apiResponse = await _service.WeatherAPIService.GetWeatherDataAsync(q);

            location.TzId = apiResponse.Location!.TzId;
            var createdLocation = await _service.LocationService.CreateLocationAsync(location);

            var weatherParametrs = new WeatherParameters()
            {
                Offset = 0,
                Limit = 1,
                Desc = true
            };

            bool needWeather = false;
            var weather = (await _service.WeatherService.GetWeathersAsync(createdLocation.Id, weatherParametrs, trackChanges: false)).weathers;

            if (weather.Count() == 0)
            {
                needWeather = true;
            }
            else
            {
                var now = DateTime.Now;
                var timeZoneId = TimeZoneInfo.FindSystemTimeZoneById(createdLocation.TzId!);
                var time = TimeZoneInfo.ConvertTime(now, timeZoneId);

                if (weather.First().LastUpdated <= time.AddMinutes(-30))
                {
                    needWeather = true;
                }
            }

            if (needWeather)
            {
                var weatherDto = _mapper.Map<WeatherForCreationDto>(apiResponse);
                await _service.WeatherService.CreateWeatherForLocationAsync(createdLocation.Id, weatherDto, trackChanges: false);
            }

            return CreatedAtRoute("LocationById", new { id = createdLocation.Id }, createdLocation);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteLocation(Guid id)
        {
            await _service.LocationService.DeleteLocationAsync(id, trackChanges: false);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateLocation(Guid id, [FromBody] LocationForUpdateDto location)
        {
            await _service.LocationService.UpdateLocationAsync(id, location, trackChanges: true);

            return NoContent();
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> PartiallyUpdateLocation(Guid id, [FromBody] JsonPatchDocument<LocationForUpdateDto> patchDoc)
        {
            if (patchDoc is null)
            {
                return BadRequest("patchDoc object sent from client is null.");
            }

            var result = await _service.LocationService.GetLocationForPatchAsync(id, trackChanges: true);

            patchDoc.ApplyTo(result.locationToPatch, ModelState);

            TryValidateModel(result.locationToPatch);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            await _service.LocationService.SaveChangesForPatchAsync(result.locationToPatch, result.locationEntity);

            return NoContent();
        }
    }
}
