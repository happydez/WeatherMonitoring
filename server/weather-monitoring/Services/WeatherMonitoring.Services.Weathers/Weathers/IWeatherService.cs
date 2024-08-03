namespace WeatherMonitoring.Services.Weathers;

public interface IWeatherService
{
    Task<WeatherModel> Create(CreateWeatherModel model);
    Task<IEnumerable<WeatherModel>> GetAllById(Guid locationId, int? limit, int? offset, bool? desc, DateTime? dt1, DateTime? dt2);
}
