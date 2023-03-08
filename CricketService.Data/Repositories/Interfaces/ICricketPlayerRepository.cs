using CricketService.Domain;
using CricketService.Domain.Enums;

namespace CricketService.Data.Repositories.Interfaces;

public interface ICricketPlayerRepository
{
    IEnumerable<CricketPlayerInfoResponse> GetPlayerByUuid(Guid playerUuid);

    IEnumerable<string> GetPlayersByTeamName(string teamName, CricketFormat format);

    IEnumerable<PlayerDetails> GetAllPlayers();

    CricketPlayerInfoResponse GetPlayerDetailsByTeamName(string teamName, string playerName, bool? isSingle = false);
}