using WeatherMonitoring.Contracts;

namespace WeatherMonitoring.Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly Lazy<ILocationRepository> _locationRepository;
        private readonly Lazy<IWeatherRepository> _weatherRepository;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _locationRepository = new Lazy<ILocationRepository>(() => new LocationRepository(repositoryContext));
            _weatherRepository = new Lazy<IWeatherRepository>(() => new WeatherRepository(repositoryContext));
        }

        public ILocationRepository Location => _locationRepository.Value;

        public IWeatherRepository Weather => _weatherRepository.Value;

        public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
    }
}
