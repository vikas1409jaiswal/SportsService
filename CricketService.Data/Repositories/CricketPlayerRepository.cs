using AutoMapper;
using CricketService.Data.Contexts;
using CricketService.Data.Extensions;
using CricketService.Data.Repositories.Extensions;
using CricketService.Data.Repositories.Interfaces;
using CricketService.Data.Utils;
using CricketService.Domain;
using CricketService.Domain.BaseDomains;
using CricketService.Domain.Common;
using CricketService.Domain.Enums;
using CricketService.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CricketService.Data.Repositories;

public class CricketPlayerRepository : ICricketPlayerRepository
{
    private readonly ILogger<CricketPlayerRepository> logger;
    private readonly CricketServiceContext context;
    private readonly IMapper mapper;
    private readonly PlayerPDFHandler playerPDFHandler;

    public CricketPlayerRepository(
        ILogger<CricketPlayerRepository> logger,
        CricketServiceContext context,
        IMapper mapper,
        PlayerPDFHandler playerPDFHandler)
    {
        this.logger = logger;
        this.context = context;
        this.mapper = mapper;
        this.playerPDFHandler = playerPDFHandler;
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

    public IEnumerable<object> GetAllPlayersUuidAndHref(CricketFormat format)
    {
        if (format == CricketFormat.All)
        {
            logger.LogInformation($"Fetching all cricket Players.");

            return context.CricketPlayerInfo
            .Select(x => new
            {
                x.Uuid,
                x.PlayerName,
                x.Href,
                Teams = x.TeamsPlayersInfos.Where(y => y.PlayerUuid.Equals(x.Uuid)).Select(z => new CricketTeam(z.TeamUuid, z.TeamName, z.TeamInfo.FlagUrl)),
                ImageUrl = x.ImageUrl.Contains("/db/PICTURES") ? $"https://img1.hscicdn.com/image/upload/f_auto,t_ds_square_w_640,q_50/lsci{x.ImageUrl}" : x.ImageUrl,
            }).OrderBy(x => x.PlayerName);
        }

        logger.LogInformation($"Fetching all cricket Players for ${format}.");

        var allPlayers = context.CricketPlayerInfo
            .Include(p => p.TeamsPlayersInfos).AsEnumerable()
            .Where(x => x.Formats.Contains(format.ToString()))
            .Select(x => new
            {
                x.Uuid,
                x.PlayerName,
                x.Href,
                Teams = x.TeamsPlayersInfos.Where(y => y.PlayerUuid.Equals(x.Uuid)).Select(z => new CricketTeam(z.TeamUuid, z.TeamName, string.Empty)),
                ImageUrl = x.ImageUrl.Contains("/db/PICTURES") ? $"https://img1.hscicdn.com/image/upload/f_auto,t_ds_square_w_640,q_50/lsci{x.ImageUrl}" : x.ImageUrl,
            }).OrderBy(x => x.PlayerName);

        return allPlayers;
    }

    public IEnumerable<PlayerDetails> GetAllPlayers(PlayersFilters filters)
    {
        logger.LogInformation("Fetching all cricket Players.");

        IEnumerable<PlayerDetails> playerDetails;

        playerDetails = context.CricketPlayerInfo
            .Select(x => new PlayerDetails(
                x.Uuid,
                x.PlayerName,
                x.Href,
                x.TeamsPlayersInfos.Select(x => new CricketTeam(x.TeamUuid, x.TeamName, x.TeamInfo.FlagUrl)).ToArray(),
                string.Empty,
                string.Empty,
                null!,
                x.TeamsPlayersInfos.Select(x => x.CareerStatistics).ToArray(),
                string.Empty,
                x.Formats,
                x.TeamNames,
                null!,
                Array.Empty<string>(),
                null!));

        if (filters.Format != CricketFormat.All)
        {
            playerDetails = playerDetails.AsEnumerable().Where(x => x.InternationalFormats.Contains(filters.Format.ToString()!));
        }

        if (filters.TeamName is not null)
        {
            playerDetails = playerDetails.Where(x => x.Teams.Any(y => y.Name == filters.TeamName));
        }

        if (filters.NameStartsWith is not null)
        {
            playerDetails = playerDetails.Where(x => x.FullName.StartsWith(filters.NameStartsWith!));
        }

        if (filters.PlayingRole is not null)
        {
            playerDetails = playerDetails.Where(x => x.ExtraInfo.PlayingRole.Contains(filters.PlayingRole));
        }

        if (filters.DateOfBirth is not null)
        {
            playerDetails = playerDetails.Where(x => x.DateOfBirth.Split(", ")[0] == filters.DateOfBirth);
        }

        if (filters.BirthYear is not null)
        {
            playerDetails = playerDetails.Where(x => x.DateOfBirth.Length > 0 && x.DateOfBirth.Split(", ")[1] == filters.BirthYear.ToString());
        }

        if (filters.IsExpired is not null)
        {
            playerDetails = playerDetails.Where(x => x.DateOfDeath.Length > 0);
        }

        return playerDetails;
    }

    public PlayerDetails GetPlayerByUuid(Guid playerUuid)
    {
        try
        {
            var player = context.CricketPlayerInfo
                .Include(p => p.TeamsPlayersInfos)
                .SingleOrDefault(x => x.Uuid == playerUuid);
            if (player == null)
            {
                logger.LogError($"Player with uuid {playerUuid} not found.");
                throw new CricketPlayerNotFoundException($"player with uuid {playerUuid} doesn't exist.");
            }

            IEnumerable<BattingInningsStat> battingStats = Enumerable.Empty<BattingInningsStat>();
            try
            {
                battingStats = GetPlayerBattingStatistics(player.Href);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error getting batting statistics for player {playerUuid} (href: {player.Href})");
            }

            return new PlayerDetails(
                player.Uuid,
                player.PlayerName,
                player.Href,
                player.TeamsPlayersInfos.Select(x => new CricketTeam(x.TeamUuid, x.TeamName)).ToArray(),
                string.Empty,
                string.Empty,
                null!,
                player.TeamsPlayersInfos.Select(x => x.CareerStatistics).ToArray(),
                string.Empty,
                player.Formats,
                player.TeamNames,
                null!,
                Array.Empty<string>(),
                new PlayerOverallStats()
                {
                    PlayerODIStats = new PlayerFormatStats()
                    {
                        BattingInningsStats = battingStats,
                        BattingOverallStats = new BattingStatistics(
                                matches: 0,
                                innings: battingStats.Count(),
                                notOut: 0,
                                runs: (int)battingStats.Sum(bs => bs.RunScored),
                                ducks: 0,
                                highestScore: battingStats.Any() ? battingStats.Max(bs => bs.RunScored).ToString() : "0",
                                ballsFaced: (int)battingStats.Sum(bs => bs.BallsFaced),
                                centuries: battingStats.Count(bs => bs.RunScored >= 100),
                                halfCenturies: battingStats.Count(bs => bs.RunScored >= 50 && bs.RunScored < 100),
                                fours: (int)battingStats.Sum(bs => bs.FoursInInning),
                                sixes: (int)battingStats.Sum(bs => bs.SixesInInning),
                                span: string.Empty,
                                title: string.Empty,
                                subTitle: string.Empty),
                    },
                    PlayerT20IStats = new PlayerFormatStats()
                    {
                        BattingInningsStats = Enumerable.Empty<BattingInningsStat>(),
                    },
                });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Unhandled error in GetPlayerByUuid for uuid {playerUuid}");
            throw;
        }
    }

    public IEnumerable<BattingInningsStat> GetPlayerBattingStatistics(string playerHref)
    {
        logger.LogInformation($"Fetching batting statistics for player with href: {playerHref}");
        try
        {
            var odiMatches = context.LimitedOverInternationalMatchesInfo
                .Where(match => match.MatchNumber.StartsWith("ODI no. "))
                .ToList();

            var innings = odiMatches
                .SelectMany(match => new[]
                {
                    new
                    {
                        match.Uuid,
                        match.MatchNumber,
                        MatchDate = DateTime.Parse(match.MatchDate),
                        Inning = 1,
                        BattingData = match.Team1.BattingScoreCard,
                        TeamName = match.Team1.Team.Name,
                        OppTeamName = match.Team2.Team.Name,
                    },
                    new
                    {
                        match.Uuid,
                        match.MatchNumber,
                        MatchDate = DateTime.Parse(match.MatchDate),
                        Inning = 2,
                        BattingData = match.Team2.BattingScoreCard,
                        TeamName = match.Team2.Team.Name,
                        OppTeamName = match.Team1.Team.Name,
                    },
                });

            var battingRecords = innings
                .SelectMany(match => match.BattingData.Select((batting, index) => new
                {
                    match.Uuid,
                    match.MatchNumber,
                    match.MatchDate,
                    match.Inning,
                    BattingPosition = index + 1,
                    PlayerHref = batting.PlayerName.Href,
                    PlayerName = batting.PlayerName.Name,
                    RunsScored = batting.RunsScored,
                    BallsFaced = batting.BallsFaced,
                    Sixes = batting.Sixes,
                    Fours = batting.Fours,
                    OutStatus = batting.OutStatus,
                    match.TeamName,
                    match.OppTeamName,
                }))
                .ToList(); // Fix: Materialize the query to avoid multiple context calls

            var filteredBatting = battingRecords
                .Where(batting => batting.PlayerHref == playerHref);

            var orderedBatting = filteredBatting
                .OrderBy(batting => batting.MatchNumber)
                .ThenBy(batting => batting.Inning)
                .ThenBy(batting => batting.TeamName)
                .ThenBy(batting => batting.BattingPosition);

            var results = orderedBatting
                .Select(batting => new BattingInningsStat(
                    batting.Uuid,
                    batting.MatchNumber,
                    batting.MatchDate,
                    batting.Inning,
                    batting.BattingPosition,
                    batting.PlayerHref,
                    batting.PlayerName,
                    (int)(batting.RunsScored ?? 0),
                    (int)(batting.BallsFaced ?? 0),
                    (int)(batting.Sixes ?? 0),
                    (int)(batting.Fours ?? 0),
                    batting.OutStatus,
                    batting.TeamName,
                    batting.OppTeamName))
                .ToList();
            return results;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error in GetPlayerBattingStatistics for href: {playerHref}");
            throw new Exception($"Critical error in GetPlayerBattingStatistics for href: {playerHref}", ex);
        }
    }

    public CricketPlayerInfoResponse GetPlayerDetailsByTeamName(string teamName, string playerName, bool? isSingle = false)
    {
        logger.LogInformation($"Fething details for player {playerName} for {teamName}");

        var cricketMatchInfoTableT20I = context.LimitedOverInternationalMatchesInfo;
        var cricketMatchInfoTableODI = context.LimitedOverInternationalMatchesInfo;
        var cricketMatchInfoTableTest = context.TestCricketMatchInfo;
        var cricketTeamPlayerInfoTable = context.CricketTeamPlayerInfos.Include(tp => tp.PlayerInfo);

        var allMatchesByTeamT20I = cricketMatchInfoTableT20I
            .Where(x => x.MatchNumber.Contains("T20I"))
            .OrderBy(x => Convert.ToInt32(x.MatchNumber.Replace("T20I no. ", string.Empty))).Select(x => x.ToDomain(mapper));

        var allMatchesByTeamODI = cricketMatchInfoTableODI
            .Where(x => x.MatchNumber.Contains("ODI"))
            .OrderBy(x => Convert.ToInt32(x.MatchNumber.Replace("ODI no. ", string.Empty))).Select(x => x.ToDomain(mapper));

        var allMatchesByTeamTest = cricketMatchInfoTableTest
            .OrderBy(x => Convert.ToInt32(x.MatchNumber.Replace("Test no. ", string.Empty))).Select(x => x.ToDomain(mapper));

        Entities.CricketTeamPlayerInfos playerInfo = null!;

        try
        {
            try
            {
                playerInfo = cricketTeamPlayerInfoTable
                .Single(x => x.PlayerName == playerName && x.TeamName == teamName);
            }
            catch
            {
                playerInfo = cricketTeamPlayerInfoTable
                .Single(x => x.PlayerName.Contains(playerName) && x.TeamName == teamName);
            }
        }
        catch
        {
            throw new Exception($"{playerName} ({teamName}) doesn't exist.");
        }

        var playerCareerDetails = new CricketPlayerInfoResponse(
            playerInfo.PlayerInfo.Uuid,
            playerName,
            string.Empty,
            teamName,
            string.Empty,
            new CareerDetailsInfo(playerInfo.TeamName, null!, null!, null!),
            "https://upload.wikimedia.org/wikipedia/commons/thumb/5/57/Shri_Virat_Kohli_for_Cricket%2C_in_a_glittering_ceremony%2C_at_Rashtrapati_Bhavan%2C_in_New_Delhi_on_September_25%2C_2018_%28cropped%29.JPG/330px-Shri_Virat_Kohli_for_Cricket%2C_in_a_glittering_ceremony%2C_at_Rashtrapati_Bhavan%2C_in_New_Delhi_on_September_25%2C_2018_%28cropped%29.JPG");

        CricketTeam cricketTeam = new CricketTeam(playerInfo.TeamUuid, teamName);

        CricketPlayer cricketPlayer = new CricketPlayer(playerInfo.PlayerName, playerInfo.PlayerInfo.Href);

        playerCareerDetails.CareerDetails.T20Career = allMatchesByTeamT20I.ToList().GetPlayerStatistics(cricketTeam, cricketPlayer, isSingle);

        playerCareerDetails.CareerDetails.ODICareer = allMatchesByTeamODI.ToList().GetPlayerStatistics(cricketTeam, cricketPlayer, isSingle);

        playerCareerDetails.CareerDetails.TestCareer = allMatchesByTeamTest.ToList().GetTestPlayerStatistics(cricketTeam, cricketPlayer, isSingle);

        return playerCareerDetails;
    }

    public async Task GeneratedPDFForPlayers()
    {
        var allTeamsPlayersInfo = context.CricketTeamPlayerInfos.Include(x => x.TeamInfo).Include(x => x.PlayerInfo);

        await playerPDFHandler.AddPDFCricketPlayerRecords(allTeamsPlayersInfo);
    }
}
