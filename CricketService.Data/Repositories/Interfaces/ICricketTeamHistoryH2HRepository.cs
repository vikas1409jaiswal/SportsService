using CricketService.Data.Entities;
using CricketService.Domain.Enums;

namespace CricketService.Data.Repositories.Interfaces
{
    public interface ICricketTeamHistoryH2HRepository
    {
        Task<IEnumerable<CricketTeamHistoryH2hDTO>> GetTeamsHistoryH2H(
            CricketFormat format, string team1Name, string? team2Name = null);

        Task SeedCricketTeamHistoryH2HTable(string team1Name, string team2Name, CricketFormat format);
    }
}
