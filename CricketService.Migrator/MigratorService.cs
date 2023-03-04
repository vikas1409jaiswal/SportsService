using CricketService.Migrator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CricketSevice.Migrations
{
    public class MigratorService : IHostedService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger logger;
        private readonly IHostApplicationLifetime hostApplicationLifetime;

        public MigratorService(
           IServiceScopeFactory scopeFactory,
           ILogger<MigratorService> logger,
           IHostApplicationLifetime hostApplicationLifetime)
        {
            this.scopeFactory = scopeFactory;
            this.logger = logger;
            this.hostApplicationLifetime = hostApplicationLifetime;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                Migrators migrator = null!;

                try
                {
                    migrator = scope.ServiceProvider.GetRequiredService<Migrators>();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to locate migrator scoped service.");
                }
                finally
                {
                    hostApplicationLifetime.StopApplication();
                }

                await migrator.RunAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
