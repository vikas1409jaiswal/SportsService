namespace CricketService.Data.Repositories.Interfaces
{
    public interface IHangfireRepository
    {
        Task UpdatePlayersCareerStatistics();

        Task UpdateTeamRecords();
    }
}
