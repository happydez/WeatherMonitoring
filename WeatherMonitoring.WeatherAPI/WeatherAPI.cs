using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeatherMonitoring.Contracts;
using WeatherMonitoring.Entities.Exceptions;
using WeatherMonitoring.Entities.WeatherAPIModels;
using WeatherMonitoring.Shared.RequestFeatures;

namespace WeatherMonitoring.WeatherAPI
{
    public sealed class WeatherAPI : IWeatherAPI
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public WeatherAPI(HttpClient httpClient, IOptions<WeatherAPISettings> settings)
        {
            _httpClient = httpClient;
            _apiKey = settings.Value.APIKey ?? throw new WeatherAPIKEYNotFoundException();
        }

#nullable disable
        public async Task<APIResponse> GetWeatherDataAsync(string q)
        {
            var response = await _httpClient.GetAsync($"current.json?key={_apiKey}&q={q}");

            response.EnsureSuccessStatusCode();

            var serializerSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<APIResponse>(content, serializerSettings);

            return result;
        }

        public async Task<PagedList<APIResponse>> GetWeatherHistoryAsync(string q, SearchParameters searchParameters)
        {
            if (searchParameters.Date is null)
                throw new SearchDateBadRequestException();

            var response = await _httpClient.GetAsync($"history.json?key={_apiKey}" +
                $"&q={q}&dt={searchParameters.Date:yyyy-MM-dd}");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var serializerSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };

            var jsonObject = JsonConvert.DeserializeObject<JObject>(content, serializerSettings);
            var locationObject = jsonObject["location"];
            var forecastDays = jsonObject["forecast"]["forecastday"];

            var result = new List<APIResponse>();

            var location = new APILocation
            {
                Name = (string)locationObject["name"],
                Region = (string)locationObject["region"],
                Country = (string)locationObject["country"],
                Lat = (double)locationObject["lat"],
                Lon = (double)locationObject["lon"],
                TzId = (string)locationObject["tz_id"]
            };

            foreach (var day in forecastDays)
            {
                var hourlyData = day["hour"];

                foreach (var hour in hourlyData)
                {
                    var weather = new APIWeather
                    {
                        TemperatureCelsius = (double)hour["temp_c"],
                        Humidity = (int)hour["humidity"],
                        PressureIn = (double)hour["pressure_in"],
                        WindSpeedKph = (double)hour["wind_kph"],
                        LastUpdatedEpoch = (long)hour["time_epoch"],
                        LastUpdated = DateTime.SpecifyKind((DateTime)hour["time"], DateTimeKind.Utc),
                        Condition = new()
                        {
                            Code = (int)hour["condition"]["code"],
                            Text = (string)hour["condition"]["text"],
                        }
                    };

                    result.Add(new()
                    {
                        Location = location,
                        Weather = weather
                    });
                }
            }

            return PagedList<APIResponse>.ToPagedList(result, searchParameters.Offset, searchParameters.Limit);
        }

        public async Task<IEnumerable<APIResponse>> SearchLocationsAsync(string q)
        {
            var response = await _httpClient.GetAsync($"search.json?key={_apiKey}" +
                $"&q={q}");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var locations = JsonConvert.DeserializeObject<IEnumerable<APILocation>>(content);

            var result = locations?.Select(location => new APIResponse
            {
                Location = location
            }) ?? [];

            return result;
        }
    }
}
