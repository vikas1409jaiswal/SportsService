using CricketService.Domain;

namespace CricketService.Data.Repositories.Interfaces;

    public interface ICricketPlayerRepository
    {
        IEnumerable<string> GetPlayersByTeamNameT20I(string teamName);

        IEnumerable<string> GetPlayersByTeamNameODI(string teamName);

        CricketPlayerInfoResponse GetPlayerDetailsByTeamName(string teamName, string playerName, bool? isSingle = false);
    }