using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WeatherMonitoring.Repository
{
    public static class RepositoryInitializer
    {
        public static void Execute(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetService<IServiceScopeFactory>()?.CreateScope();
            ArgumentNullException.ThrowIfNull(scope);

            var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<RepositoryContext>>();
            using var context = dbContextFactory.CreateDbContext();

            context.Database.EnsureDeleted();
            context.Database.Migrate();
        }
    }
}
