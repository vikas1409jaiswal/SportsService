using System.Threading.Tasks;

namespace CricketService.Hangfire.Postgres.Contracts
{
    public interface IHangfireContext
    {
        Task MigrateAsync();
    }
}