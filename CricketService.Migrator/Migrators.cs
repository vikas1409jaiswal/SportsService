using CricketService.Data.Contexts;
using Microsoft.Extensions.Logging;

namespace CricketService.Migrator;

public class Migrators
{
    private readonly ILogger logger;
    private readonly CricketServiceContext context;

    public Migrators(CricketServiceContext context, ILogger<Migrators> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task RunAsync()
    {
       var pendingMigrations = await context.GetPendingMigrationsAsync();
       logger.LogInformation("{PendingMigrationsCount} pending migrations found", pendingMigrations.Count());

       if (pendingMigrations.Any())
        {
            logger.LogInformation("Start running pending migrations");
            await context.MigrateAsync();
            logger.LogInformation("Stop running pending migrations");
        }
    }
}
