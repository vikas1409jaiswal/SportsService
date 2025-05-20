using CricketService.Domain;
using CricketService.Domain.Common;
using CricketService.Domain.Enums;

namespace CricketService.Data.Repositories.Interfaces;

public interface ICricketPlayerRepository
{
    PlayerDetails GetPlayerByUuid(Guid playerUuid);

    IEnumerable<PlayerDetails> GetAllPlayers(PlayersFilters filters);

    IEnumerable<string> GetPlayersByTeamName(string teamName, CricketFormat format);

    IEnumerable<object> GetAllPlayersUuidAndHref(CricketFormat format);

    CricketPlayerInfoResponse GetPlayerDetailsByTeamName(string teamName, string playerName, bool? isSingle = false);

    Task GeneratedPDFForPlayers();
}