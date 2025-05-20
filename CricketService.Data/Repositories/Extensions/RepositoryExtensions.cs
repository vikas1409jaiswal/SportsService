using CricketService.Data.Contexts;
using CricketService.Data.Entities;
using CricketService.Domain;
using CricketService.Domain.BaseDomains;
using CricketService.Domain.Common;
using CricketService.Domain.Enums;
using CricketService.Domain.ResponseDomains;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static CricketService.Data.Repositories.Extensions.RepositoryExtensionHelper;

namespace CricketService.Data.Repositories.Extensions
{
    public static class RepositoryExtensions
    {
        #region SaveTeamInformation
        public static async Task<CricketTeamInfoDTO> SaveTeamInfo(
            this CricketTeam cricketTeam,
            CricketServiceContext context,
            CricketFormat format)
        {
            var isExist = context.CricketTeamInfo.Any(x => x.TeamName == cricketTeam.Name);

            if (isExist)
            {
                var teamInfo = context.CricketTeamInfo.Single(x => x.TeamName == cricketTeam.Name);

                if (!teamInfo.Formats.Contains(format.ToString()))
                {
                    teamInfo.Formats = teamInfo.Formats.Concat(new List<string> { format.ToString() }).ToList();
                    context.CricketTeamInfo.Update(teamInfo);
                    await context.SaveChangesAsync();
                }

                return teamInfo;
            }
            else
            {
                var teamInfo = new CricketTeamInfoDTO
                {
                    Uuid = Guid.NewGuid(),
                    TeamName = cricketTeam.Name,
                    Formats = new List<string>() { format.ToString() },
                    FlagUrl = GetCountriesDataFromFile(cricketTeam.Name).Flags.Svg,
                    TestRecords = new TeamFormatRecordDetails(null!, 0, 0, 0, 0, 0, null!),
                    ODIRecords = new TeamFormatRecordDetails(null!, 0, 0, 0, 0, 0, null!),
                    T20IRecords = new TeamFormatRecordDetails(null!, 0, 0, 0, 0, 0, null!),
                };

                context.CricketTeamInfo.Add(teamInfo);

                await context.SaveChangesAsync();

                return teamInfo;
            }
        }
        #endregion

        #region SavePlayerInformation
        public static async Task<int> SavePlayersForTeam(
            this CricketTeam cricketTeam,
            CricketPlayer[] playing11,
            CricketServiceContext context,
            CricketFormat format)
        {
            int count = 0;

            foreach (var player in playing11)
            {
                var isExist = context.CricketPlayerInfo.Any(x => x.Href == player.Href);

                var teamUuid = context.CricketTeamInfo.Single(x => x.TeamName == cricketTeam.Name).Uuid;

                var playerInfo2 = await ReadPlayerFile(player.Href);

                if (isExist)
                {
                    var players = context.CricketPlayerInfo.Include(p => p.TeamsPlayersInfos).Where(x => x.PlayerName == player.Name && x.Href == player.Href).ToList();

                    foreach (var p in players)
                    {
                        if (!p.Formats.Contains(format.ToString()))
                        {
                            p.Formats = p.Formats.Concat(new List<string> { format.ToString() }).ToList();
                            context.CricketPlayerInfo.Update(p);
                            await context.SaveChangesAsync();
                        }

                        if (!p.InternationalTeamNames.Contains(cricketTeam.Name))
                        {
                            p.InternationalTeamNames = p.InternationalTeamNames.Concat(new List<string> { cricketTeam.Name }).ToList();
                            p.TeamsPlayersInfos.Add(
                                new CricketTeamPlayerInfos
                                {
                                    TeamUuid = teamUuid,
                                    PlayerUuid = p.Uuid,
                                    TeamName = cricketTeam.Name,
                                    PlayerName = playerInfo2.Player.LongName,
                                    CareerStatistics = new CareerDetailsInfo(cricketTeam.Name, null!, null!, null!),
                                });
                         }

                        context.CricketPlayerInfo.Update(p);
                        await context.SaveChangesAsync();
                        }
                    }
                else
                {
                    var playerInfo = GetPlayersDataFromFile(player.Href);

                    //Summary<BattingStats>[] battingStats = await ReadPlayerMatchesFile<BattingStats>(player.Href);
                    //Summary<BowlingStats>[] bowlingStats = await ReadPlayerMatchesFile<BowlingStats>(player.Href);
                    //Summary<FieldingStats>[] fieldingStats = await ReadPlayerMatchesFile<FieldingStats>(player.Href);

                    //await PlayerPDFHandler.AddPlayerImage(playerInfo2.Player.HeadshotImage is null ? (playerInfo2.Player.ImageUrl ?? string.Empty) : playerInfo2.Player.HeadshotImage.Url, player.Href);

                    if (playerInfo is null)
                    {
                        playerInfo = new PlayersFileData
                        {
                            PlayerUuid = Guid.NewGuid(),
                            DebutDetails = new DebutDetailsInfo(
                            new DebutInfo(string.Empty, string.Empty),
                            new DebutInfo(string.Empty, string.Empty),
                            new DebutInfo(string.Empty, string.Empty)),
                            Birth = string.Empty,
                            Died = string.Empty,
                            TeamNames = new string[] { },
                            BattingStyle = string.Empty,
                            BowlingStyle = string.Empty,
                            PlayingRole = string.Empty,
                            Height = string.Empty,
                            ImageSrc = string.Empty,
                        };
                    }

                    context.CricketPlayerInfo.Add(new CricketPlayerInfoDTO
                    {
                        Uuid = playerInfo.PlayerUuid,
                        PlayerName = playerInfo2.Player.LongName,
                        FullName = playerInfo2.Player.FullName,
                        Href = player.Href,
                        ImageUrl = playerInfo2.Player.HeadshotImage is null ? (playerInfo2.Player.ImageUrl ?? string.Empty) : playerInfo2.Player.HeadshotImage.Url,
                        DebutDetails = new DebutDetailsInfo(
                            new DebutInfo(playerInfo.DebutDetails.T20IMatches.First, playerInfo.DebutDetails.T20IMatches.Last),
                            new DebutInfo(playerInfo.DebutDetails.ODIMatches.First, playerInfo.DebutDetails.ODIMatches.Last),
                            new DebutInfo(playerInfo.DebutDetails.TestMatches.First, playerInfo.DebutDetails.TestMatches.Last)),
                        InternationalTeamNames = new List<string>() { cricketTeam.Name },
                        Formats = new List<string>() { format.ToString() },
                        DateOfBirth = playerInfo2.Player.DateOfBirth,
                        DateOfDeath = playerInfo2.Player.DateOfDeath,
                        TeamNames = string.Join(", ", playerInfo.TeamNames),
                        ExtraInfo = new PlayerExtraInfo
                        {
                            InternationalCareerSpan = playerInfo2.Player.IntlCareerSpan,
                            BattingStyle = string.Join("::", playerInfo2.Player.LongBattingStyles),
                            BowlingStyle = string.Join("::", playerInfo2.Player.LongBowlingStyles),
                            PlayingRole = string.Join("::", playerInfo2.Player.PlayingRoles),
                            Height = playerInfo.Height,
                        },
                        Contents = playerInfo2.Content.Profile is null ? Array.Empty<string>() : playerInfo2.Content.Profile.Items.Select(x => x.Html).ToArray(),
                        TeamsPlayersInfos = GetTeamsPlayerInfo(cricketTeam, teamUuid, playerInfo, playerInfo2),
                    });

                    count += await context.SaveChangesAsync();
                }
            }

            return count;
        }

        private static List<CricketTeamPlayerInfos> GetTeamsPlayerInfo(
            CricketTeam cricketTeam,
            Guid teamUuid,
            PlayersFileData playerInfo,
            ESPNCricketPlayer<BattingStats> playerInfo2,
            Summary<BattingStats>[]? battingStats = null,
            Summary<BowlingStats>[]? bowlingStats = null,
            Summary<FieldingStats>[]? fieldingStats = null)
        {
            var lastPlayedYear = Convert.ToInt32(playerInfo2.Player.IntlCareerSpan.Split(" - ")[1]);

            if (lastPlayedYear < 2024 && battingStats is not null && bowlingStats is not null && fieldingStats is not null)
            {
                List<List<BattingStatistics>> battingStatisticsAll = new();
                List<List<BowlingStatistics>> bowlingStatisticsAll = new();
                List<List<FieldingStatistics>> fieldingStatisticsAll = new();

                Func<string, int, BattingStats[]> getBattingStats = (t, i) => { return battingStats[i].Groups.ToList().Single(g => g.Type.Equals(t)).Stats; };
                Func<string, int, BowlingStats[]> getBowlingStats = (t, i) => { return bowlingStats[i].Groups.ToList().Single(g => g.Type.Equals(t)).Stats; };
                Func<string, int, FieldingStats[]> getFieldingStats = (t, i) => { return fieldingStats[i].Groups.ToList().Single(g => g.Type.Equals(t)).Stats; };

                var types = new List<string>()
                {
                  "CAREER_AVERAGES",
                  "OPPOSITION_TEAM",
                  "HOST_COUNTRY",
                  "YEAR",
                  "CAPTAIN",
                  "KEEPER",
                  "TEAM_INNINGS",
                  "MATCH_INNINGS",
                  "TOURNAMENT_TYPE",
                  "MAJOR_TROPHY",
                  "BATTING_POSITION",
                };

                for (int i = 0; i < 3; i++)
                {
                    List<BattingStatistics> battingStatistics = new();
                    List<BowlingStatistics> bowlingStatistics = new();
                    List<FieldingStatistics> fieldingStatistics = new();

                    foreach (var type in types)
                    {
                        try
                        {
                            getBattingStats.Invoke(type, i).GetBattingStatistics(battingStatistics, type);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        try
                        {
                            getBowlingStats.Invoke(type, i).GetBowlingStatistics(bowlingStatistics, type);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        try
                        {
                            getFieldingStats.Invoke(type, i).GetFieldingStatistics(fieldingStatistics, type);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    battingStatisticsAll.Add(battingStatistics);
                    bowlingStatisticsAll.Add(bowlingStatistics);
                    fieldingStatisticsAll.Add(fieldingStatistics);
                }

                return new List<CricketTeamPlayerInfos>()
                        {
                            new CricketTeamPlayerInfos
                            {
                                TeamUuid = teamUuid,
                                PlayerUuid = playerInfo.PlayerUuid,
                                TeamName = cricketTeam.Name,
                                PlayerName = playerInfo2.Player.LongName,
                                CareerStatistics = (battingStats is not null && bowlingStats is not null && fieldingStats is not null) ? new CareerDetailsInfo(
                                    cricketTeam.Name,
                                    new CareerInfo(
                                        null!,
                                        null!,
                                        battingStatisticsAll.First(),
                                        bowlingStatisticsAll.First(),
                                        fieldingStatisticsAll.First()),
                                    new CareerInfo(
                                        null!,
                                        null!,
                                        battingStatisticsAll.Skip(1).First(),
                                        bowlingStatisticsAll.Skip(1).First(),
                                        fieldingStatisticsAll.Skip(1).First()),
                                    new CareerInfo(
                                        null!,
                                        null!,
                                        battingStatisticsAll.Last(),
                                        bowlingStatisticsAll.Last(),
                                        fieldingStatisticsAll.Last())) : new CareerDetailsInfo(cricketTeam.Name, null!, null!, null!),
                            },
                        };
            }

            return new List<CricketTeamPlayerInfos>()
                        {
                            new CricketTeamPlayerInfos
                            {
                                TeamUuid = teamUuid,
                                PlayerUuid = playerInfo.PlayerUuid,
                                TeamName = cricketTeam.Name,
                                PlayerName = playerInfo2.Player.LongName,
                                CareerStatistics = new CareerDetailsInfo(cricketTeam.Name, null!, null!, null!),
                            },
                        };
        }

        private static void GetBattingStatistics(this BattingStats[] stats, List<BattingStatistics> battingStatistics, string superTitle)
        {
            if (stats.Length > 0)
            {
                foreach (var bs in stats)
                {
                    if (bs.Mt > 0 && bs.In > 0)
                    {
                        battingStatistics.Add(new BattingStatistics(
                                            (int)bs.Mt!,
                                            (int)bs.In!,
                                            (int)bs.No!,
                                            (int)bs.Rn!,
                                            (int)bs.Dk!,
                                            bs.Hs!,
                                            (int)bs.Bf!,
                                            (int)bs.Hn!,
                                            (int)bs.Ft!,
                                            (int)bs.Fo!,
                                            (int)bs.Si!,
                                            bs.Sp!,
                                            superTitle,
                                            bs.Tt!));
                    }
                }
            }
        }

        private static void GetBowlingStatistics(this BowlingStats[] stats, List<BowlingStatistics> bowlingStatistics, string superTitle)
        {
            if (stats.Length > 0)
            {
                foreach (var bs in stats)
                {
                    if (bs.Mt > 0 && bs.In > 0)
                    {
                        bowlingStatistics.Add(new BowlingStatistics(
                                           (int)bs.Mt!,
                                           (int)bs.In!,
                                           (int)bs.Wk!,
                                           new Over((double)bs.Ov!),
                                           bs.Cd ?? -1,
                                           bs.Bbi ?? string.Empty,
                                           bs.Bbm ?? string.Empty,
                                           -1,
                                           (int)bs.Fw!,
                                           (int)bs.Tw!,
                                           -1,
                                           -1,
                                           (int)bs.Md!,
                                           -1,
                                           -1,
                                           -1,
                                           bs.Sp,
                                           bs.Tt!,
                                           superTitle));
                    }
                }
            }
        }

        private static void GetFieldingStatistics(this FieldingStats[] stats, List<FieldingStatistics> fieldingStatistics, string superTitle)
        {
            Console.WriteLine(superTitle);

            if (stats.Length > 0)
            {
                foreach (var fs in stats)
                {
                    if (fs.Mt > 0 && fs.In > 0)
                    {
                        fieldingStatistics.Add(new FieldingStatistics(
                                           (int)fs.Ct!,
                                           (int)fs.St!,
                                           -1,
                                           fs.Sp,
                                           fs.Tt!,
                                           superTitle,
                                           (int)fs.Mt!,
                                           (int)fs.In!,
                                           (int)fs.Ds!,
                                           (int)fs.Ck!,
                                           (int)fs.Cf!));
                    }
                }
            }
        }
        #endregion

        public static IEnumerable<string> GetAllPlayersName(
            this IQueryable<InternationalCricketMatchResponse> cricketMatchInfoResponses,
            string teamName)
        {
            var playersList = cricketMatchInfoResponses
                          .Select(x => x.Team1.Team.Name == teamName ? x.Team1.Playing11 : x.Team2.Playing11)
                          .ToList()
                          .Where(x => x is not null)
                          .SelectMany(x => x)
                          .Select(x => x.Name.Trim().Replace("(c)", string.Empty).Replace("†", string.Empty))
                          .ToList()
                          .Distinct();

            return playersList;
        }

        public static TeamMileStones GetMileStones(
            this List<InternationalCricketMatchResponse> cricketMatchInfoResponses,
            CricketTeam cricketTeam)
        {
            var matchHelper = new TeamMatchesStatisticsHelper(cricketMatchInfoResponses, cricketTeam);

            return matchHelper.AllMileStonesByTeam;
        }

        public static TeamMileStones GetTestMileStones(
           this List<TestCricketMatchResponse> cricketMatchInfoResponses,
           CricketTeam cricketTeam)
        {
            var matchHelper = new TestMatchesStatisticsHelper(cricketMatchInfoResponses, cricketTeam);

            return matchHelper.AllMileStones;
        }

        #region GetPlayerStatistics
        public static CareerInfo GetPlayerStatistics(
            this List<InternationalCricketMatchResponse> cricketMatchInfoResponses,
            CricketTeam cricketTeam,
            CricketPlayer cricketPlayer,
            bool? isSingle = false)
        {
           var matchHelper = new TeamMatchesStatisticsHelper(cricketMatchInfoResponses, cricketTeam);

           var playerBattingDetails = matchHelper.AllBattingScoreCardsByTeam
                .SelectMany(x => x)
                .GroupBy(x => x.PlayerName)
                .Where(x => x.Key.Href.Contains(cricketPlayer.Href))
                .SelectMany(x => x);

           var playerBowlingDetails = matchHelper.AllBowlingScoreCardsByTeam
                .SelectMany(x => x)
                .GroupBy(x => x.PlayerName)
                .Where(x => x.Key.Href.Contains(cricketPlayer.Href))
                .SelectMany(x => x);

           var allPlayersArr = matchHelper.AllPlaying11sByTeam.Where(x => x is not null).SelectMany(x => x.Playing11Members);

           MatchDetails debutDetails = null!;
           MatchDetails lastDetails = null!;

           if ((bool)isSingle! && (playerBattingDetails.Any() || playerBowlingDetails.Any()))
            {
                InternationalCricketMatchResponse debutMatchDetails = null!;
                InternationalCricketMatchResponse lastMatchDetails = null!;

                try
                {
                    var matchesByPlayer = matchHelper.AllMatchesByTeam
                        .Where(x => (x.Team1.Playing11 is not null && x.Team2.Playing11 is not null)
                        && (x.Team1.Team.Uuid.Equals(cricketTeam.Uuid)
                        ? x.Team1.Playing11.Any(y => y.Href.Contains(cricketPlayer.Href))
                        : x.Team2.Playing11.Any(y => y.Href.Contains(cricketPlayer.Href))));
                    debutMatchDetails = matchesByPlayer.First();
                    lastMatchDetails = matchesByPlayer.Last();
                }
                catch
                {
                    Console.WriteLine($"No debut details found for player {cricketPlayer.Href}");
                }

                if (debutMatchDetails is not null)
                {
                    debutDetails = new MatchDetails(
                       (Guid)debutMatchDetails.MatchUuid!,
                       debutMatchDetails.MatchDate,
                       debutMatchDetails.Team1.Team.Uuid.Equals(cricketTeam.Uuid) ? debutMatchDetails.Team2.Team.Name : debutMatchDetails.Team1.Team.Name,
                       debutMatchDetails.Venue,
                       debutMatchDetails.Result,
                       debutMatchDetails.MatchNumber);
                }

                if (lastMatchDetails is not null)
                {
                    lastDetails = new MatchDetails(
                       (Guid)lastMatchDetails.MatchUuid!,
                       lastMatchDetails.MatchDate,
                       lastMatchDetails.Team1.Team.Uuid.Equals(cricketTeam.Uuid) ? lastMatchDetails.Team2.Team.Name : lastMatchDetails.Team1.Team.Name,
                       lastMatchDetails.Venue,
                       lastMatchDetails.Result,
                       lastMatchDetails.MatchNumber);
                }
            }

           var careerInfo = new CareerInfo(debutDetails, lastDetails, null!, null!, null!);

           if (playerBattingDetails.Any())
            {
                careerInfo.BattingStatistics.ToList().Add(new BattingStatistics(
                   allPlayersArr.Count(x => x.Href.Contains(cricketPlayer.Href)),
                   playerBattingDetails.Count(),
                   playerBattingDetails.Count(x => x.OutStatus.Contains("not out")),
                   (int)playerBattingDetails.Sum(x => x.RunsScored)!,
                   playerBattingDetails.Count(x => x.RunsScored == 0),
                   playerBattingDetails.Max(x => x.RunsScored).ToString()!,
                   (int)playerBattingDetails.Sum(x => x.BallsFaced)!,
                   playerBattingDetails.Count(x => x.RunsScored > 99),
                   playerBattingDetails.Count(x => x.RunsScored > 49 && x.RunsScored < 100),
                   (int)playerBattingDetails.Sum(x => x.Fours)!,
                   (int)playerBattingDetails.Sum(x => x.Sixes)!,
                   "0000-0000",
                   "CAREER_AVERAGES",
                   "overview"));
            }

           if (playerBowlingDetails.Any())
            {
                var mostWickets = playerBowlingDetails
                    .OrderByDescending(x => x.Wickets)
                    .ThenBy(x => x.RunsConceded).FirstOrDefault();

                careerInfo.BowlingStatistics.ToList().Add(new BowlingStatistics(
                   allPlayersArr.Count(x => x.Href.Contains(cricketPlayer.Href)),
                   playerBowlingDetails.Count(),
                   playerBowlingDetails.Sum(x => x.Wickets),
                   playerBowlingDetails.Sum(x => new Over(x.OversBowled).Balls).ToOvers(),
                   playerBowlingDetails.Sum(x => x.RunsConceded),
                   $"{mostWickets!.Wickets}/{mostWickets.RunsConceded}",
                   $"{mostWickets!.Wickets}/{mostWickets.RunsConceded}",
                   playerBowlingDetails.Count(x => x.Wickets > 3),
                   playerBowlingDetails.Count(x => x.Wickets > 4),
                   playerBowlingDetails.Count(x => x.Wickets == 10),
                   playerBowlingDetails.Sum(x => x.NoBall),
                   playerBowlingDetails.Sum(x => x.WideBall),
                   playerBowlingDetails.Sum(x => x.Maidens),
                   (int)playerBowlingDetails.Sum(x => x.Dots)!,
                   (int)playerBowlingDetails.Sum(x => x.Sixes)!,
                   (int)playerBowlingDetails.Sum(x => x.Fours)!,
                   "0000-0000",
                   "CAREER_AVERAGES",
                   "overview"));
            }

           var allOutStatusByOpponentTeams = matchHelper.AllBattingScoreCardsByOpponent
                .SelectMany(x => x)
                .Select(x => x.OutStatus);

           var allCatchesByTeam = allOutStatusByOpponentTeams.Where(x => x.StartsWith("c"));

           var allCatchesByPlayer = allCatchesByTeam
                 .Where(x =>
                 x.Contains($"c {cricketPlayer.Name.Split(" ").Last()}")
                 || x.Contains($"c {cricketPlayer.Name}")
                 || x.Contains($"c & b {cricketPlayer.Name}")
                 || x.Contains($"c & b {cricketPlayer.Name.Split(" ").Last()}"));

           if (allCatchesByPlayer.Any())
            {
                careerInfo.FieldingStatistics.ToList().Add(
                    new FieldingStatistics(
                        allCatchesByPlayer.Count(),
                        0,
                        0,
                        "0000-0000",
                        "CAREER_AVERAGES",
                        "overview",
                        0,
                        0,
                        0,
                        0,
                        0));
            }

           return careerInfo;
        }

        public static CareerInfo GetTestPlayerStatistics(
           this List<TestCricketMatchResponse> cricketMatchInfoResponses,
           CricketTeam cricketTeam,
           CricketPlayer cricketPlayer,
           bool? isSingle = false)
        {
            var matchHelper = new TestMatchesStatisticsHelper(cricketMatchInfoResponses, cricketTeam);

            var playerBattingDetails = matchHelper.AllBattingScoreCardsByTeam
                .SelectMany(x => x)
                .GroupBy(x => x.PlayerName)
                .Where(x => x.Key.Href.Contains(cricketPlayer.Href))
                .SelectMany(x => x);

            var playerBowlingDetails = matchHelper.AllBowlingScoreCardsByTeam
                .SelectMany(x => x)
                .GroupBy(x => x.PlayerName)
                .Where(x => x.Key.Href.Contains(cricketPlayer.Href))
                .SelectMany(x => x);

            var allPlayersArr = matchHelper.AllPlaying11sByTeam
                                           .Where(x => x is not null)
                                           .SelectMany(x => x.Playing11Members);

            MatchDetails debutDetails = null!;
            MatchDetails lastDetails = null!;

            if ((bool)isSingle! && (playerBattingDetails.Any() || playerBowlingDetails.Any()))
            {
                TestCricketMatchResponse debutMatchDetails = null!;
                TestCricketMatchResponse lastMatchDetails = null!;

                try
                {
                    var matchesByPlayer = matchHelper.AllMatchesByTeam
                        .Where(x => (x.Team1.Inning1.Playing11 is not null && x.Team2.Inning1.Playing11 is not null)
                        && (x.Team1.Team.Uuid.Equals(cricketTeam.Uuid)
                        ? x.Team1.Inning1.Playing11.Any(y => y.Href.Contains(cricketPlayer.Href))
                        : x.Team2.Inning1.Playing11.Any(y => y.Href.Contains(cricketPlayer.Href))));
                    debutMatchDetails = matchesByPlayer.First();
                    lastMatchDetails = matchesByPlayer.Last();
                }
                catch
                {
                    Console.WriteLine($"No debut details found for player {cricketPlayer.Href}");
                }

                if (debutMatchDetails is not null)
                {
                    debutDetails = new MatchDetails(
                       (Guid)debutMatchDetails.MatchUuid!,
                       debutMatchDetails.MatchDate,
                       debutMatchDetails.Team1.Team.Uuid.Equals(cricketTeam.Uuid) ? debutMatchDetails.Team2.Team.Name : debutMatchDetails.Team1.Team.Name,
                       debutMatchDetails.Venue,
                       debutMatchDetails.Result,
                       debutMatchDetails.MatchNumber);
                }

                if (lastMatchDetails is not null)
                {
                    lastDetails = new MatchDetails(
                       (Guid)lastMatchDetails.MatchUuid!,
                       lastMatchDetails.MatchDate,
                       lastMatchDetails.Team1.Team.Uuid.Equals(cricketTeam.Uuid) ? lastMatchDetails.Team2.Team.Name : lastMatchDetails.Team1.Team.Name,
                       lastMatchDetails.Venue,
                       lastMatchDetails.Result,
                       lastMatchDetails.MatchNumber);
                }
            }

            var careerInfo = new CareerInfo(debutDetails, lastDetails, null!, null!, null!);

            if (playerBattingDetails.Any())
            {
                careerInfo.BattingStatistics.ToList().Add(new BattingStatistics(
                   allPlayersArr.Count(x => x.Href.Contains(cricketPlayer.Href)),
                   playerBattingDetails.Count(),
                   playerBattingDetails.Count(x => x.OutStatus.Contains("not out")),
                   (int)playerBattingDetails.Sum(x => x.RunsScored)!,
                   playerBattingDetails.Count(x => x.RunsScored == 0),
                   playerBattingDetails.Max(x => x.RunsScored).ToString()!,
                   (int)playerBattingDetails.Sum(x => x.BallsFaced)!,
                   playerBattingDetails.Count(x => x.RunsScored > 99),
                   playerBattingDetails.Count(x => x.RunsScored > 49 && x.RunsScored < 100),
                   (int)playerBattingDetails.Sum(x => x.Fours)!,
                   (int)playerBattingDetails.Sum(x => x.Sixes)!,
                   "0000-0000",
                   "CAREER_AVERAGES",
                   "overview"));
            }

            if (playerBowlingDetails.Any())
            {
                var mostWickets = playerBowlingDetails
                    .OrderByDescending(x => x.Wickets)
                    .ThenBy(x => x.RunsConceded).FirstOrDefault();

                careerInfo.BowlingStatistics.ToList().Add(new BowlingStatistics(
                   allPlayersArr.Count(x => x.Href.Contains(cricketPlayer.Href)),
                   playerBowlingDetails.Count(),
                   playerBowlingDetails.Sum(x => x.Wickets),
                   playerBowlingDetails.Sum(x => new Over(x.OversBowled).Balls).ToOvers(),
                   playerBowlingDetails.Sum(x => x.RunsConceded),
                   $"{mostWickets!.Wickets}/{mostWickets.RunsConceded}",
                   $"{mostWickets!.Wickets}/{mostWickets.RunsConceded}",
                   playerBowlingDetails.Count(x => x.Wickets > 3),
                   playerBowlingDetails.Count(x => x.Wickets > 4),
                   playerBowlingDetails.Count(x => x.Wickets == 10),
                   playerBowlingDetails.Sum(x => x.NoBall),
                   playerBowlingDetails.Sum(x => x.WideBall),
                   playerBowlingDetails.Sum(x => x.Maidens),
                   (int)playerBowlingDetails.Sum(x => x.Dots)!,
                   (int)playerBowlingDetails.Sum(x => x.Sixes)!,
                   (int)playerBowlingDetails.Sum(x => x.Fours)!,
                   "0000-0000",
                   "CAREER_AVERAGES",
                   "overview"));
            }

            var allOutStatusByOpponentTeams = matchHelper.AllBattingScoreCardsByOpponent
               .SelectMany(x => x)
               .Select(x => x.OutStatus);

            var allCatchesByTeam = allOutStatusByOpponentTeams.Where(x => x.StartsWith("c"));

            var allCatchesByPlayer = allCatchesByTeam
                .Where(x =>
                x.Contains($"c {cricketPlayer.Name.Split(" ").Last()}")
                || x.Contains($"c {cricketPlayer.Name}")
                || x.Contains($"c & b {cricketPlayer.Name}")
                || x.Contains($"c & b {cricketPlayer.Name.Split(" ").Last()}"));

            if (allCatchesByPlayer.Any())
            {
                careerInfo.FieldingStatistics.ToList().Add(
                    new FieldingStatistics(
                        allCatchesByPlayer.Count(),
                        0,
                        0,
                        "0000-0000",
                        "CAREER_AVERAGES",
                        "overview",
                        0,
                        0,
                        0,
                        0,
                        0));
            }

            return careerInfo;
        }
        #endregion

        public static CountriesData GetCountriesDataFromFile(string teamName)
        {
            List<CountriesData> countriesData = new();

            StreamReader r = new StreamReader(@"D:\MyYoutubeRepos\repo\CricketService\CricketService.Data\StaticData\CountriesData.json");
            countriesData = JsonConvert.DeserializeObject<List<CountriesData>>(r.ReadToEnd())!;

            return countriesData.FirstOrDefault(x => x.Name!.Common == teamName, new CountriesData(new Name(teamName), new Flags("url_not_found", "url_not_found")));
        }

        public static PlayersFileData GetPlayersDataFromFile(string href)
        {
            List<PlayersFileData> playersData = new();

            StreamReader r = new StreamReader(@"D:\MyYoutubeRepos\repo\CricketService\CricketService.Data\StaticData\Player_Data_BackUp\CricketPlayers.json");
            playersData = JsonConvert.DeserializeObject<List<PlayersFileData>>(r.ReadToEnd())!;

            return playersData.FirstOrDefault(x => x.Href == href)!;
        }

        public static async Task<ESPNCricketPlayer<BattingStats>> ReadPlayerFile(string href)
        {
            var filePath = $"D:/MyYoutubeRepos/repo/CricketService/CricketService.Data/StaticData/Player_Data_BackUp/Players/{href.Split("/").Last()}.json";

            if (!File.Exists(filePath))
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://hs-consumer-api.espncricinfo.com/v1/pages/player/home?playerId={href.Split("-").Last()}"),
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();

                    var responseObj = JsonConvert.DeserializeObject(body);

                    var jsonStr = JsonConvert.SerializeObject(responseObj, Formatting.Indented);

                    File.WriteAllText(filePath, jsonStr);
                }
            }

            StreamReader sr = new StreamReader(filePath);
            return JsonConvert.DeserializeObject<ESPNCricketPlayer<BattingStats>>(sr.ReadToEnd())!;
        }

        public static async Task<Summary<T>[]> ReadPlayerMatchesFile<T>(string href)
        {
            var directoryPath = $"D:/MyYoutubeRepos/repo/CricketService/CricketService.Data/StaticData/Player_Data_BackUp/PlayerMatches/{href.Split("/").Last()}";

            Console.WriteLine(href);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);

                var client = new HttpClient();
                List<HttpRequestMessage> requests = new List<HttpRequestMessage>();

                string[][] queryParams = new string[][]{
                    new string[] { "1", "BATTING" },
                    new string[] { "1", "BOWLING" },
                    new string[] { "1", "FIELDING" },
                    new string[] { "1", "ALLROUND" },
                    new string[] { "2", "BATTING" },
                    new string[] { "2", "BOWLING" },
                    new string[] { "2", "FIELDING" },
                    new string[] { "2", "ALLROUND" },
                    new string[] { "3", "BATTING" },
                    new string[] { "3", "BOWLING" },
                    new string[] { "3", "FIELDING" },
                    new string[] { "3", "ALLROUND" },
                };

                foreach (var queryParam in queryParams)
                {
                    requests.Add(new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"https://hs-consumer-api.espncricinfo.com/v1/pages/player/stats/summary?playerId={href.Split("-").Last()}&recordClassId={queryParam[0]}&type={queryParam[1]}"),
                    });
                }

                foreach (var request in requests)
                {
                    using (var response = await client.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();
                        var body = await response.Content.ReadAsStringAsync();

                        if (request.RequestUri!.Query.Contains("BATTING"))
                        {
                            var responseObj = JsonConvert.DeserializeObject<Content<BattingStats>>(body);

                            var jsonStr = JsonConvert.SerializeObject(responseObj!.Summary.Groups, Formatting.Indented);

                            File.WriteAllText($"{directoryPath}/{responseObj!.Summary.RecordClassId}-batting-records.json", jsonStr);
                        }

                        if (request.RequestUri!.Query.Contains("BOWLING"))
                        {
                            var responseObj = JsonConvert.DeserializeObject<Content<BowlingStats>>(body);

                            var jsonStr = JsonConvert.SerializeObject(responseObj!.Summary.Groups, Formatting.Indented);

                            File.WriteAllText($"{directoryPath}/{responseObj!.Summary.RecordClassId}-bowling-records.json", jsonStr);
                        }

                        if (request.RequestUri!.Query.Contains("FIELDING"))
                        {
                            var responseObj = JsonConvert.DeserializeObject<Content<FieldingStats>>(body);

                            var jsonStr = JsonConvert.SerializeObject(responseObj!.Summary.Groups, Formatting.Indented);

                            File.WriteAllText($"{directoryPath}/{responseObj!.Summary.RecordClassId}-fielding-records.json", jsonStr);
                        }

                        if (request.RequestUri!.Query.Contains("ALLROUND"))
                        {
                            var responseObj = JsonConvert.DeserializeObject<Content<AllRoundStats>>(body);

                            var jsonStr = JsonConvert.SerializeObject(responseObj!.Summary.Groups, Formatting.Indented);

                            File.WriteAllText($"{directoryPath}/{responseObj!.Summary.RecordClassId}-allround-records.json", jsonStr);
                        }
                    }
                }
            }

            StreamReader sr1 = new StreamReader($"{directoryPath}/1-batting-records.json");
            StreamReader sr2 = new StreamReader($"{directoryPath}/2-batting-records.json");
            StreamReader sr3 = new StreamReader($"{directoryPath}/3-batting-records.json");

            if (typeof(T).Equals(typeof(BattingStats)))
            {
                sr1 = new StreamReader($"{directoryPath}/1-batting-records.json");
                sr2 = new StreamReader($"{directoryPath}/2-batting-records.json");
                sr3 = new StreamReader($"{directoryPath}/3-batting-records.json");

            }

            if (typeof(T).Equals(typeof(BowlingStats)))
            {
                sr1 = new StreamReader($"{directoryPath}/1-bowling-records.json");
                sr2 = new StreamReader($"{directoryPath}/2-bowling-records.json");
                sr3 = new StreamReader($"{directoryPath}/3-bowling-records.json");
            }

            if (typeof(T).Equals(typeof(FieldingStats)))
            {
                sr1 = new StreamReader($"{directoryPath}/1-fielding-records.json");
                sr2 = new StreamReader($"{directoryPath}/2-fielding-records.json");
                sr3 = new StreamReader($"{directoryPath}/3-fielding-records.json");

            }

            Summary<T>[] summary = new Summary<T>[]
            {
                new Summary<T>
                {
                  RecordClassId = 1,
                  Type = typeof(T).Name,
                  Groups = JsonConvert.DeserializeObject<Group<T>[]>(sr1.ReadToEnd())!,
                },
                new Summary<T>
                {
                  RecordClassId = 2,
                  Type = typeof(T).Name,
                  Groups = JsonConvert.DeserializeObject<Group<T>[]>(sr2.ReadToEnd())!,
                },
                new Summary<T>
                {
                  RecordClassId = 3,
                  Type = typeof(T).Name,
                  Groups = JsonConvert.DeserializeObject<Group<T>[]>(sr3.ReadToEnd())!,
                },
            };

            return summary;
        }
    }
}
