using CricketService.Data.Contexts;
using CricketService.Domain;
using CricketService.Domain.Common;
using CricketService.Domain.Enums;

namespace CricketService.Data.Repositories.Extensions
{
    public static class RepositoryExtensions
    {
        public static async Task<int> SavePlayersForTeam(this TeamScoreDetails teamScoreDetails, CricketServiceContext context, CricketFormat format)
        {
            int count = 0;

            foreach (var player in teamScoreDetails.Playing11)
            {
                var isExist = context.CricketPlayerInfo.Any(x => x.TeamName == teamScoreDetails.TeamName && player.Name == x.FullName);

                if (isExist)
                {
                   var p = context.CricketPlayerInfo.Single(x => x.TeamName == teamScoreDetails.TeamName && player.Name == x.FullName);

                   if (!p.Formats.Contains(format))
                    {
                        p.Formats = p.Formats.Concat(new List<CricketFormat> { format });
                        context.CricketPlayerInfo.Update(p);
                        await context.SaveChangesAsync();
                    }
                }
                else
                {
                    context.CricketPlayerInfo.Add(new Entities.CricketPlayerInfo
                    {
                        FullName = player.Name,
                        Href = player.Href,
                        TeamName = teamScoreDetails.TeamName,
                        Formats = new List<CricketFormat>() { format },
                    });

                    count = count + await context.SaveChangesAsync();
                }
            }

            return count;
        }

        public static IEnumerable<string> GetAllPlayersName(this IQueryable<CricketMatchInfoResponse> cricketMatchInfoResponses, string teamName)
        {
            var playersList = cricketMatchInfoResponses
                          .Select(x => x.Team1.TeamName == teamName ? x.Team1.Playing11 : x.Team2.Playing11)
                          .ToList()
                          .Where(x => x is not null)
                          .SelectMany(x => x)
                          .Select(x => x.Name.Trim())
                          .ToList()
                          .Distinct();

            return playersList;
        }

        public static TeamMileStones GetMileStones(this IQueryable<CricketMatchInfoResponse> cricketMatchInfoResponses, string teamName)
        {
            var allScoresByTeam = cricketMatchInfoResponses
                   .Select(x => (x.Team1.TeamName == teamName) ? x.Team1.TotalInningDetails : x.Team2.TotalInningDetails).ToList();

            var allBattingScoreCardByTeam = cricketMatchInfoResponses
                .Select(x => x.Team1.TeamName == teamName ? x.Team1.BattingScoreCard : x.Team2.BattingScoreCard).ToList();

            var allBowlingScoreCardByTeam = cricketMatchInfoResponses
               .Select(x => x.Team1.TeamName == teamName ? x.Team2.BowlingScoreCard : x.Team1.BowlingScoreCard).ToList();

            var batsmanArr = allBattingScoreCardByTeam
                .SelectMany(x => x)
                .GroupBy(x => x.PlayerName).Select(x => new
                {
                    Player = x.Key,
                    Runs = x.Sum(y => y.RunsScored),
                    Fours = x.Sum(y => y.Fours),
                    Sixes = x.Sum(y => y.Sixes),
                    HalfCenturies = x.Count(y => y.RunsScored > 49 && y.RunsScored < 100),
                    Centuries = x.Count(y => y.RunsScored > 99 && y.RunsScored < 150),
                    OneAndHalfCenturies = x.Count(y => y.RunsScored > 149 && y.RunsScored < 200),
                    DoubleCenturies = x.Count(y => y.RunsScored > 199),
                    HighehestIndividualScore = x.Max(x => x.RunsScored),
                });

            var bowlersArr = allBowlingScoreCardByTeam
                .SelectMany(x => x)
                .GroupBy(x => x.PlayerName).Select(x => new
                {
                    Player = x.Key,
                    Wickets = x.Sum(y => y.Wickets),
                });

            var mostRuns = batsmanArr.MaxBy(x => x.Runs);

            var mostFours = batsmanArr.MaxBy(x => x.Fours);

            var mostSixes = batsmanArr.MaxBy(x => x.Sixes);

            var most50s = batsmanArr.MaxBy(x => x.HalfCenturies);

            var most100s = batsmanArr.MaxBy(x => x.Centuries);

            var most150s = batsmanArr.MaxBy(x => x.OneAndHalfCenturies);

            var most200s = batsmanArr.MaxBy(x => x.DoubleCenturies);

            var hiScore = batsmanArr.MaxBy(x => x.HighehestIndividualScore);

            var mostWickets = bowlersArr.MaxBy(x => x.Wickets);

            var allBattingMilestones = allBattingScoreCardByTeam.Select(x => new
            {
                HalfCenturies = x.Count(y => y.RunsScored > 49 && y.RunsScored < 100),
                Centuries = x.Count(y => y.RunsScored > 99 && y.RunsScored < 150),
                OneAndHalfCenturies = x.Count(y => y.RunsScored > 149 && y.RunsScored < 200),
                DoubleCentury = x.Count(y => y.RunsScored > 199),
            }).ToList();

            var highestTotalScore = allScoresByTeam.MaxBy(x => x.Runs);
            var lowestTotalScore = allScoresByTeam.Where(x => x.Wickets == 10).MinBy(x => x.Runs);

            if (lowestTotalScore is null)
            {
                lowestTotalScore = allScoresByTeam.MinBy(x => x.Runs);
            }

            return new TeamMileStones(
                 allScoresByTeam.Sum(x => x.Runs),
                 allBattingMilestones.Sum(x => x.Centuries),
                 allBattingMilestones.Sum(x => x.HalfCenturies),
                 allBattingMilestones.Sum(x => x.OneAndHalfCenturies),
                 allBattingMilestones.Sum(x => x.DoubleCentury),
                 new InningRecordDetails(
                     new InningTotal(highestTotalScore!.Runs, highestTotalScore.Wickets, new Over(highestTotalScore.Overs.Overs)),
                     new InningTotal(lowestTotalScore!.Runs, lowestTotalScore.Wickets, new Over(lowestTotalScore.Overs.Overs))),
                 new KeyValuePair<string, int>(mostRuns!.Player.Name, (int)mostRuns.Runs!),
                 new KeyValuePair<string, int>(mostWickets!.Player.Name, (int)mostWickets.Wickets!),
                 new KeyValuePair<string, int>(mostSixes!.Player.Name, (int)mostSixes.Sixes!),
                 new KeyValuePair<string, int>(mostFours!.Player.Name, (int)mostFours.Fours!),
                 new KeyValuePair<string, int>(most50s!.Player.Name, (int)most50s.HalfCenturies!),
                 new KeyValuePair<string, int>(most100s!.Player.Name, (int)most100s.Centuries!),
                 new KeyValuePair<string, int>(most150s!.Player.Name, (int)most150s.OneAndHalfCenturies!),
                 new KeyValuePair<string, int>(most200s!.Player.Name, (int)most200s.DoubleCenturies!),
                 new KeyValuePair<string, int>(hiScore!.Player.Name, (int)hiScore.HighehestIndividualScore!));
        }

        public static CareerInfo GetPlayerStats(this IQueryable<CricketMatchInfoResponse> cricketMatchInfoResponses, string teamName, string playerName, bool? isSingle = false)
        {
            var allScoreCardByTeam = cricketMatchInfoResponses
                .Select(x => new
                {
                    MatchUuid = x.MatchUuid,
                    ScoreCard = x.Team1.TeamName == teamName ? x.Team1 : x.Team2,
                }).ToList();

            var allOpponentsScoreCardByTeam = cricketMatchInfoResponses
                .Select(x => x.Team1.TeamName == teamName ? x.Team2 : x.Team1).ToList();

            var allBattingScoreCardByTeam = allScoreCardByTeam
                .Select(x => x.ScoreCard.BattingScoreCard).ToList();

            var allBowlingScoreCardByTeam = allOpponentsScoreCardByTeam
                .Select(x => x.BowlingScoreCard).ToList();

            var playerBattingDetails = allBattingScoreCardByTeam
                .SelectMany(x => x)
                .GroupBy(x => x.PlayerName)
                .Where(x => x.Key.Name.Contains(playerName))
                .SelectMany(x => x);

            var playerBowlingDetails = allBowlingScoreCardByTeam
                .SelectMany(x => x)
                .GroupBy(x => x.PlayerName)
                .Where(x => x.Key.Name.Contains(playerName))
                .SelectMany(x => x);

            var allPlaying11sByTeam = allScoreCardByTeam.Select(x => x.ScoreCard.Playing11);

            var allPlayersArr = allPlaying11sByTeam.Where(x => x is not null).SelectMany(x => x);

            DebutDetails debutDetails = null!;

            if ((bool)isSingle! && (playerBattingDetails.Any() || playerBowlingDetails.Any()))
            {
                var allMatchScoreCardByPlayer = allScoreCardByTeam
               .Where(x => x.ScoreCard.Playing11 is null ? false : x.ScoreCard.Playing11.Any(x => x.Name.Contains(playerName))).ToList();

                var debutMatchDetails = cricketMatchInfoResponses.ToList().Single(x => x.MatchUuid == allMatchScoreCardByPlayer.FirstOrDefault()!.MatchUuid);

                debutDetails = new DebutDetails(
                        (Guid)debutMatchDetails.MatchUuid!,
                        Convert.ToDateTime(debutMatchDetails.MatchDate),
                        debutMatchDetails.Team1.TeamName == teamName ? debutMatchDetails.Team2.TeamName : debutMatchDetails.Team1.TeamName,
                        debutMatchDetails.Venue,
                        debutMatchDetails.Result,
                        debutMatchDetails.MatchNo);
            }

            return new CareerInfo(
                debutDetails,
                playerBattingDetails.Any() ? new BattingStatistics(
                   allPlayersArr.Count(x => x.Name.Contains(playerName)),
                   playerBattingDetails.Count(),
                   (int)playerBattingDetails.Sum(x => x.RunsScored)!,
                   (int)playerBattingDetails.Sum(x => x.BallsFaced)!,
                   playerBattingDetails.Count(x => x.RunsScored > 99),
                   playerBattingDetails.Count(x => x.RunsScored > 49 && x.RunsScored < 100),
                   (int)playerBattingDetails.Max(x => x.RunsScored)!,
                   (int)playerBattingDetails.Sum(x => x.Fours)!,
                   (int)playerBattingDetails.Sum(x => x.Sixes)!) : null!,
                playerBowlingDetails.Any() ? new BowlingStatistics(
                   allPlayersArr.Count(x => x.Name.Contains(playerName)),
                   playerBowlingDetails.Count(),
                   playerBowlingDetails.Sum(x => x.Wickets),
                   playerBowlingDetails.Sum(x => new Over(x.OversBowled).Balls).ToOvers(),
                   playerBowlingDetails.Sum(x => x.RunsConceded),
                   playerBowlingDetails.Sum(x => x.NoBall),
                   playerBowlingDetails.Sum(x => x.WideBall),
                   playerBowlingDetails.Sum(x => x.Maidens),
                   (int)playerBowlingDetails.Sum(x => x.Dots)!,
                   (int)playerBowlingDetails.Sum(x => x.Sixes)!,
                   (int)playerBowlingDetails.Sum(x => x.Fours)!) : null!);
        }
    }
}
