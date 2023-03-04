using CricketService.Data.Contexts;
using CricketService.Data.Options.Configs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CricketService.Data.Extensions;

public static class DbServiceCollectionExtensions
{
    public static IServiceCollection AddCricketServiceDataLayer(
       this IServiceCollection serviceCollection,
       IConfiguration configuration,
       string sectionName = CricketServiceContextOptions.SectionName)
    {
        var options = configuration.GetSection(sectionName).Get<CricketServiceContextOptions>();
        serviceCollection.AddDbContext<CricketServiceContext>(
            (provider, optionsBuilder) =>
            {
                optionsBuilder.UseNpgsql(options!.ConnectionString);
                optionsBuilder.UseLoggerFactory(provider.GetRequiredService<ILoggerFactory>());
            });

        return serviceCollection;
    }
}