using CricketService.Hangfire.Configs;
using CricketService.Hangfire.Extensions;
using CricketService.Hangfire.Postgres.Context;
using CricketService.Hangfire.Postgres.Contracts;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CricketService.Hangfire.Postgres.Extensions
{
    public static class HangfirePostgresServiceCollection
    {
        public static IServiceCollection AddHangfirePostgres(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var hangfireOptions = new HangfireOptions();
            configuration.GetSection(HangfireOptions.DefaultSectionName).Bind(hangfireOptions);

            serviceCollection.AddHangfireAttributes();

            serviceCollection.AddDbContext<IHangfireContext, HangfireContext>(
                optionsBuilder =>
                {
                    optionsBuilder.UseNpgsql(hangfireOptions.DatabaseConnectionString);
                });

            var options = new PostgreSqlStorageOptions
            {
                SchemaName = "hangfire",
                PrepareSchemaIfNecessary = true,
            };

            serviceCollection.AddHangfire(configuration =>
            {
                configuration.UsePostgreSqlStorage(hangfireOptions.DatabaseConnectionString, options);
            });

            serviceCollection.AddHangfireServer();

            return serviceCollection;
        }
    }
}