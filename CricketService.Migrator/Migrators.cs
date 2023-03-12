using CricketService.Data.Contexts;
using CricketService.Hangfire.Postgres.Contracts;
using Microsoft.Extensions.Logging;

namespace CricketService.Migrator;

public class Migrators
{
    private readonly ILogger logger;
    private readonly CricketServiceContext context;
    private readonly IHangfireContext hangfireContext;

    public Migrators(
        CricketServiceContext context,
        IHangfireContext hangfireContext,
        ILogger<Migrators> logger)
    {
        this.context = context;
        this.hangfireContext = hangfireContext;
        this.logger = logger;
    }

    public async Task RunAsync()
    {
        var pendingMigrations = await context.GetPendingMigrationsAsync();
        logger.LogInformation("{PendingMigrationsCount} pending migrations found", pendingMigrations.Count());

        if (pendingMigrations.Any())
        {
            logger.LogInformation("Cricket Service: Start running pending migrations");
            await context.MigrateAsync();
            logger.LogInformation("Cricket Service: Stop running pending migrations");
        }

        logger.LogInformation("HangFire: Start running pending migrations");
        await hangfireContext.MigrateAsync();
        logger.LogInformation("HangFire: Stop running pending migrations");
    }
}
