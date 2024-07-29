using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WeatherMonitoring.Context;
using WeatherMonitoring.Context.Entities;

namespace WeatherMonitoring.Services.Locations;

public class LocationService : ILocationService
{
    private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
    private readonly IMapper _mapper;

    public LocationService(IDbContextFactory<MainDbContext> dbContextFactory, IMapper mapper)
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }

    public async Task<LocationModel> Create(CreateLocationModel model)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();

        var location = await context.Locations.FirstOrDefaultAsync(loc =>
            loc.Name == model.Name
            && loc.Region == model.Region
            && loc.Country == model.Country);

        location = (location is null) ? _mapper.Map<Location>(model) : location;
        location.Included = true;
        location.Active = model.Active;

        if (location is not null)
        {
            context.Locations.Update(location);
        }
        else
        {
            await context.Locations.AddAsync(location);
        }

        await context.SaveChangesAsync();

        return _mapper.Map<LocationModel>(location);
    }

    public async Task<IEnumerable<LocationModel>> GetAll(bool? included, bool? active, int? limit, int? offest)
    {
        using var context = _dbContextFactory.CreateDbContext();

        var locations = await context.Locations.ToListAsync();

        if (included != null && active != null)
        {
            locations = locations.Where(x => x.Included == included && x.Active == active).ToList();
        }
        else if (included != null)
        {
            locations = locations.Where(x => x.Included == included).ToList();
        }
        else if (active != null)
        {
            locations = locations.Where(x => x.Active == active).ToList();
        }

        if (offest != null) locations = locations.Skip(offest.Value).ToList();
        if (limit != null) locations = locations.Take(limit.Value).ToList();

        var result = _mapper.Map<IEnumerable<LocationModel>>(locations);

        return result;
    }

    public async Task<LocationModel> GetById(Guid id)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();

        var location = await context.Locations.Where(loc => loc.Uid == id).FirstOrDefaultAsync();

        var result = _mapper.Map<LocationModel>(location);

        return result;
    }

    public async Task Update(Guid id, UpdateLocationModel model)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();

        var location = await context.Locations.Where(loc => loc.Uid == id).FirstOrDefaultAsync();

        if (location != null)
        {
            location = _mapper.Map(model, location);

            context.Locations.Update(location);

            await context.SaveChangesAsync();
        }
    }
}
