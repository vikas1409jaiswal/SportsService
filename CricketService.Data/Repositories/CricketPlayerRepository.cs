using CricketService.Data.Contexts;
using CricketService.Data.Extensions;
using CricketService.Data.Repositories.Extensions;
using CricketService.Data.Repositories.Interfaces;
using CricketService.Domain;
using Microsoft.Extensions.Logging;

namespace CricketService.Data.Repositories;

public class CricketPlayerRepository : ICricketPlayerRepository
{
    private ILogger<CricketPlayerRepository> logger;
    private CricketServiceContext context;

    public CricketPlayerRepository(ILogger<CricketPlayerRepository> logger, CricketServiceContext context)
    {
        this.logger = logger;
        this.context = context;
    }

    public IEnumerable<string> GetPlayersByTeamNameT20I(string teamName)
    {
        logger.LogInformation($"Fetching names for all t20I players for {teamName}.");

        var allPlayers = context.T20ICricketMatchInfo
                         .Where(x => x.Team1.TeamName == teamName || x.Team2.TeamName == teamName)
                         .Select(x => x.ToDomain())
                         .GetAllPlayersName(teamName);

        return allPlayers;
    }

    public IEnumerable<string> GetPlayersByTeamNameODI(string teamName)
    {
        logger.LogInformation($"Fetching names for all ODI players for {teamName}.");

        var allPlayers = context.ODICricketMatchInfo
                         .Where(x => x.Team1.TeamName == teamName || x.Team2.TeamName == teamName)
                         .Select(x => x.ToDomain())
                         .GetAllPlayersName(teamName);

        return allPlayers;
    }

    public CricketPlayerInfoResponse GetPlayerDetailsByTeamName(string teamName, string playerName, bool? isSingle = false)
    {
        logger.LogInformation($"Fething details for player ${playerName}");

        var cricketMatchInfoTableT20I = context.T20ICricketMatchInfo;
        var cricketMatchInfoTableODI = context.ODICricketMatchInfo;

        var allMatchesByTeamT20I = cricketMatchInfoTableT20I
            .Where(x => x.Team1.TeamName == teamName || x.Team2.TeamName == teamName)
            .OrderBy(x => Convert.ToInt32(x.MatchNo.Replace("T20I no. ", string.Empty))).Select(x => x.ToDomain());

        var allMatchesByTeamODI = cricketMatchInfoTableODI
            .Where(x => x.Team1.TeamName == teamName || x.Team2.TeamName == teamName)
            .OrderBy(x => Convert.ToInt32(x.MatchNo.Replace("ODI no. ", string.Empty))).Select(x => x.ToDomain());

        var playerCareerDetails = new CricketPlayerInfoResponse(
            playerName,
            default(DateTime),
            teamName,
            "Chennai",
            new CareerDetailsInfo(null!, null!, null!),
            "https://upload.wikimedia.org/wikipedia/commons/thumb/5/57/Shri_Virat_Kohli_for_Cricket%2C_in_a_glittering_ceremony%2C_at_Rashtrapati_Bhavan%2C_in_New_Delhi_on_September_25%2C_2018_%28cropped%29.JPG/330px-Shri_Virat_Kohli_for_Cricket%2C_in_a_glittering_ceremony%2C_at_Rashtrapati_Bhavan%2C_in_New_Delhi_on_September_25%2C_2018_%28cropped%29.JPG");

        playerCareerDetails.CareerDetails.T20Career = allMatchesByTeamT20I.GetPlayerStats(teamName, playerName.Trim(), isSingle);

        playerCareerDetails.CareerDetails.ODICareer = allMatchesByTeamODI.GetPlayerStats(teamName, playerName.Trim(), isSingle);

        return playerCareerDetails;
    }
}
