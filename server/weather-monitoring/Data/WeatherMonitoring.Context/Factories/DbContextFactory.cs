using Microsoft.EntityFrameworkCore;

namespace WeatherMonitoring.Context;

public class DbContextFactory
{
    private readonly DbContextOptions<MainDbContext> _options;

    public DbContextFactory(DbContextOptions<MainDbContext> options)
    {
        _options = options;
    }

    public MainDbContext Create()
    {
        return new MainDbContext(_options);
    }
}
