using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WeatherMonitoring.Context;
using WeatherMonitoring.Context.Entities;

namespace WeatherMonitoring.Services.Weathers;

public class WeatherService : IWeatherService
{
    private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
    private readonly IMapper _mapper;

    public WeatherService(IDbContextFactory<MainDbContext> dbContextFactory, IMapper mapper)
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }

    public async Task<IEnumerable<WeatherModel>> GetAllById(
        Guid locationId, int? limit, int? offset, 
        bool? desc, DateTime? dt1, DateTime? dt2)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();

        var location = await context.Locations.Where(loc => loc.Uid == locationId)
            .Include(loc => loc.Weathers).FirstOrDefaultAsync();

        if (location == null) return [];

        var result = _mapper.Map<IEnumerable<WeatherModel>>(location.Weathers);

        if (desc != null) result = 
                desc.Value 
                ? result.OrderByDescending(p => p.LastUpdatedEpoch) 
                : result.OrderBy(p => p.LastUpdatedEpoch);

        if ((dt1 != null && dt2 != null) && (dt2.Value >= dt1.Value))
        {
            result = result.Where(p => p.LastUpdated >= dt1.Value && p.LastUpdated <= dt2.Value);
        }

        if (offset != null) result = result.Skip(offset.Value);
        if (limit != null) result = result.Take(limit.Value);

        return result;
    }

    public async Task<WeatherModel> Create(CreateWeatherModel model)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();

        var weather = _mapper.Map<Weather>(model);

        await context.AddAsync(weather);

        await context.SaveChangesAsync();

        return _mapper.Map<WeatherModel>(weather);
    }
}
