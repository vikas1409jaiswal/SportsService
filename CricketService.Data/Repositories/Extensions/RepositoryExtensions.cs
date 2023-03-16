using CricketService.Data.Contexts;
using CricketService.Domain;
using CricketService.Domain.Common;
using CricketService.Domain.Enums;
using Newtonsoft.Json;
using static CricketService.Data.Repositories.Extensions.RepositoryExtensionHelper;

namespace CricketService.Data.Repositories.Extensions
{
    public static class RepositoryExtensions
    {
        public static async Task<int> SaveTeamInfo(
            this TeamScoreDetails teamScoreDetails,
            CricketServiceContext context,
            CricketFormat format)
        {
            var isExist = context.CricketTeamInfo.Any(x => x.TeamName == teamScoreDetails.TeamName);

            if (isExist)
            {
                var t = context.CricketTeamInfo.Single(x => x.TeamName == teamScoreDetails.TeamName);

                if (!t.Formats.Contains(format.ToString()))
                {
                    t.Formats = t.Formats.Concat(new List<string> { format.ToString() }).ToList();
                    context.CricketTeamInfo.Update(t);
                    await context.SaveChangesAsync();
                }

                return 0;
            }
            else
            {
                context.CricketTeamInfo.Add(new Entities.CricketTeamInfo
                {
                    TeamName = teamScoreDetails.TeamName,
                    Formats = new List<string>() { format.ToString() },
                    FlagUrl = GetCountriesDataFromFile(teamScoreDetails.TeamName).Flags.Svg,
                    TestRecords = new TestTeamFormatRecordDetails(null!, 0, 0, 0, 0, 0, null!),
                    ODIRecords = new TeamFormatRecordDetails(null!, 0, 0, 0, 0, 0, null!),
                    T20IRecords = new TeamFormatRecordDetails(null!, 0, 0, 0, 0, 0, null!),
                });

                await context.SaveChangesAsync();

                return 1;
            }
        }

        public static async Task<int> SaveTestTeamInfo(
            this TestTeamScoreDetails teamScoreDetails,
            CricketServiceContext context)
        {
            var isExist = context.CricketTeamInfo.Any(x => x.TeamName == teamScoreDetails.TeamName);

            if (isExist)
            {
                var t = context.CricketTeamInfo.Single(x => x.TeamName == teamScoreDetails.TeamName);

                if (!t.Formats.Contains(CricketFormat.TestCricket.ToString()))
                {
                    t.Formats = t.Formats.Concat(new List<string> { CricketFormat.TestCricket.ToString() }).ToList();
                    context.CricketTeamInfo.Update(t);
                    await context.SaveChangesAsync();
                }

                return 0;
            }
            else
            {
                context.CricketTeamInfo.Add(new Entities.CricketTeamInfo
                {
                    TeamName = teamScoreDetails.TeamName,
                    Formats = new List<string>() { CricketFormat.TestCricket.ToString() },
                    FlagUrl = GetCountriesDataFromFile(teamScoreDetails.TeamName).Flags.Svg,
                    TestRecords = new TestTeamFormatRecordDetails(null!, 0, 0, 0, 0, 0, null!),
                    ODIRecords = new TeamFormatRecordDetails(null!, 0, 0, 0, 0, 0, null!),
                    T20IRecords = new TeamFormatRecordDetails(null!, 0, 0, 0, 0, 0, null!),
                });

                await context.SaveChangesAsync();

                return 1;
            }
        }

        public static async Task<int> SavePlayersForTeam(
            this TeamScoreDetails teamScoreDetails,
            CricketServiceContext context,
            CricketFormat format)
        {
            int count = 0;

            foreach (var player in teamScoreDetails.Playing11)
            {
                var isExist = context.CricketPlayerInfo.Any(x => x.Href == player.Href);

                if (isExist)
                {
                    var players = context.CricketPlayerInfo.Where(x => x.PlayerName == player.Name && x.Href == player.Href).ToList();

                    foreach (var p in players)
                    {
                        if (!p.Formats.Contains(format.ToString()))
                        {
                            p.Formats = p.Formats.Concat(new List<string> { format.ToString() }).ToList();
                            context.CricketPlayerInfo.Update(p);
                            await context.SaveChangesAsync();
                        }

                        if (!p.InternationalTeamNames.Contains(teamScoreDetails.TeamName))
                        {
                            p.InternationalTeamNames = p.InternationalTeamNames.Concat(new List<string> { teamScoreDetails.TeamName }).ToList();
                            context.CricketPlayerInfo.Update(p);
                            await context.SaveChangesAsync();
                        }
                    }
                }
                else
                {
                    var playerInfo = GetPlayersDataFromFile(player.Href);

                    context.CricketPlayerInfo.Add(new Entities.CricketPlayerInfo
                    {
                        Uuid = playerInfo.PlayerUuid,
                        PlayerName = player.Name.Trim().Replace("(c)", string.Empty).Replace("†", string.Empty),
                        FullName = player.Name,
                        Href = player.Href,
                        DebutDetails = new DebutDetailsInfo(
                            new DebutInfo(playerInfo.DebutDetails.T20IMatches.First, playerInfo.DebutDetails.T20IMatches.Last),
                            new DebutInfo(playerInfo.DebutDetails.ODIMatches.First, playerInfo.DebutDetails.ODIMatches.Last),
                            new DebutInfo(playerInfo.DebutDetails.TestMatches.First, playerInfo.DebutDetails.TestMatches.Last)),
                        InternationalTeamNames = new List<string>() { teamScoreDetails.TeamName },
                        Formats = new List<string>() { format.ToString() },
                        Birth = playerInfo.Birth,
                        Death = playerInfo.Died,
                        TeamNames = string.Join(", ", playerInfo.TeamNames),
                        CareerStatistics = new CareerDetailsInfo(null!, null!, null!),
                        ExtraInfo = new PlayerExtraInfo
                        {
                            BattingStyle = playerInfo.BattingStyle,
                            BowlingStyle = playerInfo.BowlingStyle,
                            PlayingRole = playerInfo.PlayingRole,
                            Height = playerInfo.Height,
                            ImageSrc = playerInfo.ImageSrc,
                        },
                    });

                    count = count + await context.SaveChangesAsync();
                }
            }

            return count;
        }

        public static async Task<int> SavePlayersForTestTeam(
          this TestTeamScoreDetails teamScoreDetails,
          CricketServiceContext context)
        {
            int count = 0;

            foreach (var player in teamScoreDetails.Inning1.Playing11)
            {
                var isExist = context.CricketPlayerInfo.Any(x => x.Href == player.Href);

                if (isExist)
                {
                    var players = context.CricketPlayerInfo.Where(x => x.PlayerName == player.Name && x.Href == player.Href).ToList();

                    foreach (var p in players)
                    {
                        if (!p.Formats.Contains(CricketFormat.TestCricket.ToString()))
                        {
                            p.Formats = p.Formats.Concat(new List<string> { CricketFormat.TestCricket.ToString() }).ToList();
                            context.CricketPlayerInfo.Update(p);
                            await context.SaveChangesAsync();
                        }

                        if (!p.InternationalTeamNames.Contains(teamScoreDetails.TeamName))
                        {
                            p.InternationalTeamNames = p.InternationalTeamNames.Concat(new List<string> { teamScoreDetails.TeamName }).ToList();
                            context.CricketPlayerInfo.Update(p);
                            await context.SaveChangesAsync();
                        }
                    }
                }
                else
                {
                    PlayersFileData playerInfo = new();

                    playerInfo = GetPlayersDataFromFile(player.Href);

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

                    context.CricketPlayerInfo.Add(new Entities.CricketPlayerInfo
                    {
                        Uuid = playerInfo.PlayerUuid,
                        PlayerName = player.Name.Trim().Replace("(c)", string.Empty).Replace("†", string.Empty),
                        FullName = player.Name,
                        Href = player.Href,
                        DebutDetails = new DebutDetailsInfo(
                            new DebutInfo(playerInfo.DebutDetails.T20IMatches.First, playerInfo.DebutDetails.T20IMatches.Last),
                            new DebutInfo(playerInfo.DebutDetails.ODIMatches.First, playerInfo.DebutDetails.ODIMatches.Last),
                            new DebutInfo(playerInfo.DebutDetails.TestMatches.First, playerInfo.DebutDetails.TestMatches.Last)),
                        InternationalTeamNames = new List<string>() { teamScoreDetails.TeamName },
                        Formats = new List<string>() { CricketFormat.TestCricket.ToString() },
                        Birth = playerInfo.Birth,
                        Death = playerInfo.Died,
                        TeamNames = string.Join(", ", playerInfo.TeamNames),
                        CareerStatistics = new CareerDetailsInfo(null!, null!, null!),
                        ExtraInfo = new PlayerExtraInfo
                        {
                            BattingStyle = playerInfo.BattingStyle,
                            BowlingStyle = playerInfo.BowlingStyle,
                            PlayingRole = playerInfo.PlayingRole,
                            Height = playerInfo.Height,
                            ImageSrc = playerInfo.ImageSrc,
                        },
                    });

                    count = count + await context.SaveChangesAsync();
                }
            }

            return count;
        }

        public static IEnumerable<string> GetAllPlayersName(
            this IQueryable<CricketMatchInfoResponse> cricketMatchInfoResponses,
            string teamName)
        {
            var playersList = cricketMatchInfoResponses
                          .Select(x => x.Team1.TeamName == teamName ? x.Team1.Playing11 : x.Team2.Playing11)
                          .ToList()
                          .Where(x => x is not null)
                          .SelectMany(x => x)
                          .Select(x => x.Name.Trim().Replace("(c)", string.Empty).Replace("†", string.Empty))
                          .ToList()
                          .Distinct();

            return playersList;
        }

        public static TeamMileStones GetMileStones(
            this IQueryable<CricketMatchInfoResponse> cricketMatchInfoResponses,
            string teamName)
        {
            var matchHelper = new MatchesStatisticsHelper(cricketMatchInfoResponses, teamName);

            return matchHelper.AllMileStones;
        }

        public static TeamMileStones GetTestMileStones(
           this IQueryable<TestCricketMatchInfoResponse> cricketMatchInfoResponses,
           string teamName)
        {
            var matchHelper = new TestMatchesStatisticsHelper(cricketMatchInfoResponses, teamName);

            return matchHelper.AllMileStones;
        }

        public static CareerInfo GetPlayerStats(
            this IQueryable<CricketMatchInfoResponse> cricketMatchInfoResponses,
            string teamName,
            string playerName,
            bool? isSingle = false)
        {
           var matchHelper = new MatchesStatisticsHelper(cricketMatchInfoResponses, teamName);

           var playerBattingDetails = matchHelper.AllBattingScoreCardsByTeam
                .SelectMany(x => x)
                .GroupBy(x => x.PlayerName)
                .Where(x => x.Key.Name.Contains(playerName))
                .SelectMany(x => x);

           var playerBowlingDetails = matchHelper.AllBowlingScoreCardsByTeam
                .SelectMany(x => x)
                .GroupBy(x => x.PlayerName)
                .Where(x => x.Key.Name.Contains(playerName))
                .SelectMany(x => x);

           var allPlayersArr = matchHelper.AllPlaying11sByTeam.Where(x => x is not null).SelectMany(x => x);

           DebutDetails debutDetails = null!;

           if ((bool)isSingle! && (playerBattingDetails.Any() || playerBowlingDetails.Any()))
            {
                CricketMatchInfoResponse debutMatchDetails = null!;

                try
                {
                    debutMatchDetails = cricketMatchInfoResponses.ToList().Single(x => x.InternationalDebut.Any(y => y.Contains(playerName)));
                }
                catch
                {
                    Console.WriteLine($"No debut details found for player {playerName}");
                }

                if (debutMatchDetails is not null)
                {
                    debutDetails = new DebutDetails(
                       (Guid)debutMatchDetails.MatchUuid!,
                       debutMatchDetails.MatchDate,
                       debutMatchDetails.Team1.TeamName == teamName ? debutMatchDetails.Team2.TeamName : debutMatchDetails.Team1.TeamName,
                       debutMatchDetails.Venue,
                       debutMatchDetails.Result,
                       debutMatchDetails.MatchNo);
                }
            }

           var careerInfo = new CareerInfo(debutDetails, null!, null!, null!);

           if (playerBattingDetails.Any())
            {
                careerInfo.BattingStatistics = new BattingStatistics(
                   allPlayersArr.Count(x => x.Name.Contains(playerName)),
                   playerBattingDetails.Count(),
                   playerBattingDetails.Count(x => x.OutStatus.Contains("not out")),
                   (int)playerBattingDetails.Sum(x => x.RunsScored)!,
                   (int)playerBattingDetails.Max(x => x.RunsScored)!,
                   (int)playerBattingDetails.Sum(x => x.BallsFaced)!,
                   playerBattingDetails.Count(x => x.RunsScored > 99),
                   playerBattingDetails.Count(x => x.RunsScored > 49 && x.RunsScored < 100),
                   (int)playerBattingDetails.Sum(x => x.Fours)!,
                   (int)playerBattingDetails.Sum(x => x.Sixes)!);
            }

           if (playerBowlingDetails.Any())
            {
                var mostWickets = playerBowlingDetails
                    .OrderByDescending(x => x.Wickets)
                    .ThenBy(x => x.RunsConceded).FirstOrDefault();

                careerInfo.BowlingStatistics = new BowlingStatistics(
                   allPlayersArr.Count(x => x.Name.Contains(playerName)),
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
                   (int)playerBowlingDetails.Sum(x => x.Fours)!);
            }

           var allOutStatusByOpponentTeams = matchHelper.AllBattingScoreCardsByOpponent
                .SelectMany(x => x)
                .Select(x => x.OutStatus);

           var allCatchesByTeam = allOutStatusByOpponentTeams.Where(x => x.StartsWith("c"));

           var allCatchesByPlayer = allCatchesByTeam
                .Where(x =>
                x.Contains($"c {playerName.Split(" ").Last()}")
                || x.Contains($"c {playerName}")
                || x.Contains($"c & b {playerName}")
                || x.Contains($"c & b {playerName.Split(" ").Last()}"));

           if (allCatchesByPlayer.Any())
            {
                careerInfo.FieldingStatistics = new FieldingStatistics(allCatchesByPlayer.Count(), 0, 0);
            }

           return careerInfo;
        }

        public static CareerInfo GetTestPlayerStats(
           this IQueryable<TestCricketMatchInfoResponse> cricketMatchInfoResponses,
           string teamName,
           string playerName,
           bool? isSingle = false)
        {
            var matchHelper = new TestMatchesStatisticsHelper(cricketMatchInfoResponses, teamName);

            var playerBattingDetails = matchHelper.AllBattingScoreCardsByTeam
                .SelectMany(x => x)
                .GroupBy(x => x.PlayerName)
                .Where(x => x.Key.Name.Contains(playerName))
                .SelectMany(x => x);

            var playerBowlingDetails = matchHelper.AllBowlingScoreCardsByTeam
                .SelectMany(x => x)
                .GroupBy(x => x.PlayerName)
                .Where(x => x.Key.Name.Contains(playerName))
                .SelectMany(x => x);

            var allPlayersArr = matchHelper.AllPlaying11sByTeam.Where(x => x is not null).SelectMany(x => x);

            DebutDetails debutDetails = null!;

            if ((bool)isSingle! && (playerBattingDetails.Any() || playerBowlingDetails.Any()))
            {
                TestCricketMatchInfoResponse debutMatchDetails = null!;

                try
                {
                    debutMatchDetails = cricketMatchInfoResponses.ToList().Single(x => x.InternationalDebut.Any(y => y.Contains(playerName)));
                }
                catch
                {
                    Console.WriteLine($"No debut details found for player {playerName}");
                }

                if (debutMatchDetails is not null)
                {
                    debutDetails = new DebutDetails(
                       (Guid)debutMatchDetails.MatchUuid!,
                       debutMatchDetails.MatchDate,
                       debutMatchDetails.Team1.TeamName == teamName ? debutMatchDetails.Team2.TeamName : debutMatchDetails.Team1.TeamName,
                       debutMatchDetails.Venue,
                       debutMatchDetails.Result,
                       debutMatchDetails.MatchNo);
                }
            }

            var careerInfo = new CareerInfo(debutDetails, null!, null!, null!);

            if (playerBattingDetails.Any())
            {
                careerInfo.BattingStatistics = new BattingStatistics(
                   allPlayersArr.Count(x => x.Name.Contains(playerName)),
                   playerBattingDetails.Count(),
                   playerBattingDetails.Count(x => x.OutStatus.Contains("not out")),
                   (int)playerBattingDetails.Sum(x => x.RunsScored)!,
                   (int)playerBattingDetails.Max(x => x.RunsScored)!,
                   (int)playerBattingDetails.Sum(x => x.BallsFaced)!,
                   playerBattingDetails.Count(x => x.RunsScored > 99),
                   playerBattingDetails.Count(x => x.RunsScored > 49 && x.RunsScored < 100),
                   (int)playerBattingDetails.Sum(x => x.Fours)!,
                   (int)playerBattingDetails.Sum(x => x.Sixes)!);
            }

            if (playerBowlingDetails.Any())
            {
                var mostWickets = playerBowlingDetails
                    .OrderByDescending(x => x.Wickets)
                    .ThenBy(x => x.RunsConceded).FirstOrDefault();

                careerInfo.BowlingStatistics = new BowlingStatistics(
                   allPlayersArr.Count(x => x.Name.Contains(playerName)),
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
                   (int)playerBowlingDetails.Sum(x => x.Fours)!);
            }

            var allOutStatusByOpponentTeams = matchHelper.AllBattingScoreCardsByOpponent
               .SelectMany(x => x)
               .Select(x => x.OutStatus);

            var allCatchesByTeam = allOutStatusByOpponentTeams.Where(x => x.StartsWith("c"));

            var allCatchesByPlayer = allCatchesByTeam
                .Where(x =>
                x.Contains($"c {playerName.Split(" ").Last()}")
                || x.Contains($"c {playerName}")
                || x.Contains($"c & b {playerName}")
                || x.Contains($"c & b {playerName.Split(" ").Last()}"));

            if (allCatchesByPlayer.Any())
            {
                careerInfo.FieldingStatistics = new FieldingStatistics(allCatchesByPlayer.Count(), 0, 0);
            }

            return careerInfo;
        }

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
    }
}
