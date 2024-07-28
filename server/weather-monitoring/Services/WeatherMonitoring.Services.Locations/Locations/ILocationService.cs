namespace WeatherMonitoring.Services.Locations;

public interface ILocationService
{
    Task<LocationModel> GetById(Guid id);
    Task<IEnumerable<LocationModel>> GetAll(bool? included, bool? active, int? limit, int? offest);
    Task<LocationModel> Create(CreateLocationModel model);
    Task Update(Guid id, UpdateLocationModel model);
}