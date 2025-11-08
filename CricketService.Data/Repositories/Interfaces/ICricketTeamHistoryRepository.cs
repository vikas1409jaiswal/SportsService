using CricketService.Data.Entities;
using CricketService.Domain.Enums;

namespace CricketService.Data.Repositories.Interfaces
{
    public interface ICricketTeamHistoryRepository
    {
        Task<IEnumerable<CricketTeamHistoryDTO>> GetTeamsHistory(CricketFormat format);

        Task<IEnumerable<object>> GetTeamsPlayers(CricketFormat format, PlayersCategory playersCategory, string teamName);

        Task SeedCricketTeamHistoryTable(CricketFormat format, bool finalRecordOnly = false);
    }
}
