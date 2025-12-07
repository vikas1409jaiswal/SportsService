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

            IEnumerable<BattingInningsStat> odiBattingStats = Enumerable.Empty<BattingInningsStat>();
            IEnumerable<BattingInningsStat> t20iBattingStats = Enumerable.Empty<BattingInningsStat>();
            int odiMatchesCount = 0;
            int t20iMatchesCount = 0;
            try
            {
                var (odiStats, odiMatches) = GetPlayerBattingStatisticsByFormat(player.Href, "ODI");
                var (t20iStats, t20iMatches) = GetPlayerBattingStatisticsByFormat(player.Href, "T20I");
                odiBattingStats = odiStats;
                t20iBattingStats = t20iStats;
                odiMatchesCount = odiMatches;
                t20iMatchesCount = t20iMatches;
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
               new DateOnly(
                   player.DateOfBirth != null ? player.DateOfBirth.Year : 1,
                   player.DateOfBirth != null ? player.DateOfBirth.Month ?? 1 : 1,
                   player.DateOfBirth != null ? player.DateOfBirth.Date ?? 1 : 1).ToString("dd-MMM-yyyy"),
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
                       BattingInningsStats = odiBattingStats,
                       BattingOverallStats = CalculateBattingOverallStats(odiBattingStats, odiMatchesCount),
                   },
                   PlayerT20IStats = new PlayerFormatStats()
                   {
                       BattingInningsStats = t20iBattingStats,
                       BattingOverallStats = CalculateBattingOverallStats(t20iBattingStats, t20iMatchesCount),
                   },
               });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Unhandled error in GetPlayerByUuid for uuid {playerUuid}");
            throw;
        }
    }

    public (IEnumerable<BattingInningsStat> BattingStats, int MatchesCount) GetPlayerBattingStatisticsByFormat(string playerHref, string format)
    {
        logger.LogInformation($"Fetching {format} batting statistics for player with href: {playerHref}");
        try
        {
            var matches = GetMatchesByFormat(format);

            var innings = GetInningsFromMatches(matches);

            var battingRecords = GetBattingRecordsFromInnings(innings);

            var filteredBatting = battingRecords
                .Where(batting => batting.PlayerHref == playerHref);

            var orderedBatting = filteredBatting
                .OrderBy(batting => batting.MatchNumber)
                .ThenBy(batting => batting.Inning)
                .ThenBy(batting => batting.TeamName)
                .ThenBy(batting => batting.BattingPosition);

            // Convert to list to calculate milestones
            var battingList = orderedBatting.ToList();
            var milestonesList = CalculateMilestonesListByFormat(battingList, format);
            var results = new List<BattingInningsStat>();
            
            // Get player's DateOfEvent for DOB
            var player = context.CricketPlayerInfo.FirstOrDefault(x => x.Href == playerHref);
            DateOfEvent? dob = player?.DateOfBirth;
            
            for (int i = 0; i < battingList.Count; i++)
            {
                var batting = battingList[i];
                var milestones = milestonesList[i];
                int runsScored = (int)(batting.RunsScored ?? 0);
                Age age = dob != null ? Age.CalculateAge(dob, batting.MatchDate) : new Age(0, 0);
                
                var battingStat = new BattingInningsStat(
                    batting.Uuid,
                    batting.MatchNumber,
                    batting.MatchDate,
                    batting.Inning,
                    batting.BattingPosition,
                    batting.PlayerHref,
                    batting.PlayerName,
                    runsScored,
                    (int)(batting.BallsFaced ?? 0),
                    (int)(batting.Sixes ?? 0),
                    (int)(batting.Fours ?? 0),
                    batting.OutStatus,
                    batting.TeamName,
                    batting.OppTeamName,
                    milestones,
                    age);
                results.Add(battingStat);
            }

            // Calculate matches count from Playing11 arrays
            var matchesCount = matches
                .Where(match =>
                    ((object)match.Team1.Playing11 != null && ((IEnumerable<dynamic>)match.Team1.Playing11).Any(p => (string)p.Href == playerHref)) ||
                    ((object)match.Team2.Playing11 != null && ((IEnumerable<dynamic>)match.Team2.Playing11).Any(p => (string)p.Href == playerHref)))
                .Count();

            return (results, matchesCount);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error in GetPlayerBattingStatisticsByFormat for href: {playerHref}, format: {format}");
            throw new Exception($"Critical error in GetPlayerBattingStatisticsByFormat for href: {playerHref}, format: {format}", ex);
        }
    }

    public (IEnumerable<BattingInningsStat> BattingStats, int MatchesCount) GetPlayerBattingStatistics(string playerHref)
    {
        // Backward compatibility - default to ODI
        return GetPlayerBattingStatisticsByFormat(playerHref, "ODI");
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

    private List<dynamic> GetMatchesByFormat(string format)
    {
        return format switch
        {
            "ODI" => context.LimitedOverInternationalMatchesInfo
                .Where(match => match.MatchNumber.StartsWith("ODI no. "))
                .ToList()
                .Cast<dynamic>()
                .ToList(),
            "T20I" => context.LimitedOverInternationalMatchesInfo
                .Where(match => match.MatchNumber.StartsWith("T20I no. "))
                .ToList()
                .Cast<dynamic>()
                .ToList(),
            _ => throw new ArgumentException($"Unsupported format: {format}"),
        };
    }

    private IEnumerable<dynamic> GetInningsFromMatches(List<dynamic> matches)
    {
        return matches
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
    }

    private List<dynamic> GetBattingRecordsFromInnings(IEnumerable<dynamic> innings)
    {
        return innings
            .SelectMany(match => ((IEnumerable<dynamic>)match.BattingData).Select((batting, index) => (dynamic)new
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
            .ToList(); // Materialize to avoid multiple context calls
    }

    private static List<List<string>> CalculateMilestonesListByFormat<T>(List<T> battingList, string format)
        where T : class
    {
        if (battingList.Count == 0)
        {
            return new List<List<string>>();
        }

        // Cache property info for performance
        var runsProperty = typeof(T).GetProperty("RunsScored");
        var outStatusProperty = typeof(T).GetProperty("OutStatus");
        
        var milestonesList = new List<List<string>>(battingList.Count);
        
        // Milestone counters
        int fiftiesCount = 0;
        int hundredsCount = 0;
        int doubleCenturiesCount = 0;
        int oneFiftiesCount = 0;
        int ducksCount = 0;
        int totalRuns = 0;
        int thousandsCount = 0;

        foreach (var item in battingList)
        {
            var milestones = new List<string>();
            
            // Extract runs scored
            int runsScored = (runsProperty?.GetValue(item) as int?) ?? 0;
            
            // Extract out status
            string outStatus = outStatusProperty?.GetValue(item)?.ToString() ?? string.Empty;

            // Duck milestone
            if (runsScored == 0 && !string.IsNullOrWhiteSpace(outStatus) &&
                !outStatus.Equals("not out", StringComparison.OrdinalIgnoreCase))
            {
                ducksCount++;
                milestones.Add($"{GetOrdinalNumber(ducksCount)} {format} Duck");
            }

            // Career runs milestones
            totalRuns += runsScored;
            int newThousands = totalRuns / 1000;
            if (newThousands > thousandsCount)
            {
                for (int i = thousandsCount + 1; i <= newThousands; i++)
                {
                    milestones.Add($"{i * 1000} {format} Career Runs Completed");
                }

                thousandsCount = newThousands;
            }

            // Individual innings milestones
            if (runsScored >= 200)
            {
                doubleCenturiesCount++;
                milestones.Add($"{GetOrdinalNumber(doubleCenturiesCount)} {format} Double Hundred");
            }

            if (runsScored >= 150)
            {
                oneFiftiesCount++;
                milestones.Add($"{GetOrdinalNumber(oneFiftiesCount)} {format} 150+ Score");
            }

            if (runsScored >= 100)
            {
                hundredsCount++;
                milestones.Add($"{GetOrdinalNumber(hundredsCount)} {format} Hundred");
            }
            else if (runsScored >= 50)
            {
                fiftiesCount++;
                milestones.Add($"{GetOrdinalNumber(fiftiesCount)} {format} Fifty");
            }

            milestonesList.Add(milestones);
        }
        
        return milestonesList;
    }

    private static List<List<string>> CalculateMilestonesList<T>(List<T> battingList)
        where T : class
    {
        // Backward compatibility - default to ODI
        return CalculateMilestonesListByFormat(battingList, "ODI");
    }

    private static string GetOrdinalNumber(int number)
    {
        if (number <= 0)
        {
            return number.ToString();
        }

        return (number % 100) switch
        {
            11 or 12 or 13 => $"{number}th",
            _ => (number % 10) switch
            {
                1 => $"{number}st",
                2 => $"{number}nd",
                3 => $"{number}rd",
                _ => $"{number}th",
            },
        };
    }

    private BattingStatistics CalculateBattingOverallStats(IEnumerable<BattingInningsStat> battingStats, int matchesCount)
    {
        var span = string.Empty;
        if (battingStats.Any())
        {
            var firstMatchDate = battingStats.Min(bs => bs.MatchDate);
            var lastMatchDate = battingStats.Max(bs => bs.MatchDate);
            span = $"{firstMatchDate:yyyy}-{lastMatchDate:yyyy}";
        }

        var maxScore = battingStats.Any() ? battingStats.Max(bs => bs.RunScored ?? 0) : 0;
        var maxScoreInning = battingStats.FirstOrDefault(bs => bs.RunScored == maxScore);
        var highestScoreNotOut = maxScoreInning?.OutStatus?.Contains("not out", StringComparison.OrdinalIgnoreCase) == true ? "*" : string.Empty;

        return new BattingStatistics(
           matches: matchesCount,
           innings: battingStats.Count(),
           notOut: battingStats.Count(bs => bs.OutStatus.Contains("not out", StringComparison.OrdinalIgnoreCase)),
           runs: (int)battingStats.Sum(bs => bs.RunScored ?? 0),
           ducks: battingStats.Count(bs => (bs.RunScored ?? 0) == 0 && !bs.OutStatus.Contains("not out", StringComparison.OrdinalIgnoreCase)),
           highestScore: battingStats.Any() ? $"{maxScore}{highestScoreNotOut}" : "0",
           ballsFaced: (int)battingStats.Sum(bs => bs.BallsFaced ?? 0),
           centuries: battingStats.Count(bs => (bs.RunScored ?? 0) >= 100),
           halfCenturies: battingStats.Count(bs => (bs.RunScored ?? 0) >= 50 && (bs.RunScored ?? 0) < 100),
           fours: (int)battingStats.Sum(bs => bs.FoursInInning ?? 0),
           sixes: (int)battingStats.Sum(bs => bs.SixesInInning ?? 0),
           span: span,
           title: string.Empty,
           subTitle: string.Empty);
    }
}
