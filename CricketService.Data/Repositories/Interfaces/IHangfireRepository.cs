namespace CricketService.Data.Repositories.Interfaces
{
    public interface IHangfireRepository
    {
        Task UpdatePlayersCareerStatistics();

        Task UpdateTeamRecords(List<Guid>? teamUuids = null);

        void CleanDatabase();
    }
}
