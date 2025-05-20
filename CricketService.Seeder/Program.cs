using AutoMapper;
using CricketService.Data.Extensions;
using CricketService.Data.Mappings;
using CricketService.Data.Repositories;
using CricketService.Data.Repositories.Interfaces;
using CricketService.Data.Utils;
using CricketService.Seeder.Options;

namespace CricketService.Seeder
{
    public class Program
    {
        public static async Task Main()
        {
            var services = new ServiceCollection();
            var configs = ConfigureServices(services);

            var service = services.BuildServiceProvider().GetService<Seeder>();

            if (service is not null)
            {
                await service.RunSeeder(configs);
            }
        }

        private static IConfiguration ConfigureServices(IServiceCollection services)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            services.AddSingleton<Seeder>();
            services.AddCricketServiceDataLayer(config);
            services.AddOptions<StaticDataJsonFilePathsOptions>().Bind(config.GetSection(StaticDataJsonFilePathsOptions.SectionName));
            services.AddScoped<ICricketMatchRepository, CricketMatchRepository>();
            services.AddScoped<ICricketTeamRepository, CricketTeamRepository>();
            services.AddScoped<ICricketPlayerRepository, CricketPlayerRepository>();
            services.AddScoped<MatchPDFHandler>();
            services.AddScoped<TeamPDFHandler>();
            services.AddScoped<PlayerPDFHandler>();
            services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
            });

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CricketServiceProfile());
            });

            services.AddSingleton(mapperConfig.CreateMapper());

            return config;
        }
    }
}
