using CricketService.Data.Contexts;
using CricketService.Data.Extensions;
using CricketService.Data.Repositories.Extensions;
using CricketService.Data.Repositories.Interfaces;
using CricketService.Domain;
using CricketService.Domain.Enums;
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

    public IEnumerable<string> GetPlayersByTeamName(string teamName, CricketFormat format)
    {
        logger.LogInformation($"Fetching names for all {format} players for {teamName}.");

        var allPlayers = context.CricketPlayerInfo.AsEnumerable()
                       .Where(x => x.InternationalTeamNames.Contains(teamName) && x.Formats.Contains(format.ToString()))
                       .Select(x => x.PlayerName.Trim())
                       .OrderBy(x => x);

        return allPlayers;
    }

    public IEnumerable<PlayerDetails> GetAllPlayers()
    {
        logger.LogInformation("Fetching all cricket Players.");

        var allPlayers = context.CricketPlayerInfo
            .Select(x => new PlayerDetails(
                x.Uuid,
                x.PlayerName,
                x.Href,
                x.DateOfBirth,
                x.Formats,
                x.BirthPlace,
                x.BattingStyle,
                x.BowlingStyle,
                x.PlayingRole,
                x.Height,
                x.ImageSrc,
                x.TeamNames));

        return allPlayers;
    }

    public CricketPlayerInfoResponse GetPlayerDetailsByTeamName(string teamName, string playerName, bool? isSingle = false)
    {
        logger.LogInformation($"Fething details for player ${playerName}");

        var cricketMatchInfoTableT20I = context.T20ICricketMatchInfo;
        var cricketMatchInfoTableODI = context.ODICricketMatchInfo;
        var cricketPlayerInfoTable = context.CricketPlayerInfo;

        var allMatchesByTeamT20I = cricketMatchInfoTableT20I
            .Where(x => x.Team1.TeamName == teamName || x.Team2.TeamName == teamName)
            .OrderBy(x => Convert.ToInt32(x.MatchNo.Replace("T20I no. ", string.Empty))).Select(x => x.ToDomain());

        var allMatchesByTeamODI = cricketMatchInfoTableODI
            .Where(x => x.Team1.TeamName == teamName || x.Team2.TeamName == teamName)
            .OrderBy(x => Convert.ToInt32(x.MatchNo.Replace("ODI no. ", string.Empty))).Select(x => x.ToDomain());

        Entities.CricketPlayerInfo playerInfo = null!;

        try
        {
            try
            {
                playerInfo = cricketPlayerInfoTable
                .Single(x => x.PlayerName == playerName);
            }
            catch
            {
                playerInfo = cricketPlayerInfoTable
                .Single(x => x.PlayerName.Contains(playerName));
            }
        }
        catch
        {
            throw new Exception($"{playerName} ({teamName}) doesn't exist.");
        }

        var playerCareerDetails = new CricketPlayerInfoResponse(
            playerInfo.Uuid,
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

    public IEnumerable<CricketPlayerInfoResponse> GetPlayerByUuid(Guid playerUuid)
    {
        var player = context.CricketPlayerInfo.Single(x => x.Uuid == playerUuid);

        var playerD = new List<CricketPlayerInfoResponse>();

        foreach (var team in player.InternationalTeamNames)
        {
            playerD.Add(GetPlayerDetailsByTeamName(team, player.PlayerName, true));
        }

        return playerD;
    }
}
