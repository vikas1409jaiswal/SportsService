using CricketService.Hangfire.Postgres.Contracts;
using Hangfire.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CricketService.Hangfire.Postgres.Context
{
    public class HangfireContext : DbContext, IHangfireContext
    {
        public HangfireContext(DbContextOptions<HangfireContext> options)
            : base(options)
        {
        }

        public async Task MigrateAsync()
        {
            await Database.MigrateAsync();
        }
    }
}