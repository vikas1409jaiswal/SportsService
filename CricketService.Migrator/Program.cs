using CricketService.Data.Extensions;
using CricketSevice.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CricketService.Migrator;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).UseEnvironment("Development").RunConsoleAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
           .UseContentRoot(AppContext.BaseDirectory)
           .ConfigureServices((hostContext, services) =>
           {
               services.AddLogging(logging =>
               {
                   logging.AddConsole();
                   logging.ClearProviders();
               });
               services.AddCricketServiceDataLayer(hostContext.Configuration);
               services.AddHostedService<MigratorService>();
               services.AddScoped<Migrators>();
           });
    }
}