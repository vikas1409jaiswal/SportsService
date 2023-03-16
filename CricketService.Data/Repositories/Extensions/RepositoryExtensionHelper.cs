using CricketService.Domain;
using CricketService.Domain.Common;

namespace CricketService.Data.Repositories.Extensions
{
    public class RepositoryExtensionHelper
    {
        public class MatchesStatisticsHelper
        {
            private readonly IQueryable<CricketMatchInfoResponse> cricketMatchInfoResponses;
            private readonly string teamName;

            public MatchesStatisticsHelper(IQueryable<CricketMatchInfoResponse> cricketMatchInfoResponses, string teamName)
            {
                this.cricketMatchInfoResponses = cricketMatchInfoResponses;
                this.teamName = teamName;
            }

            public List<TotalInningInfo> AllScoresByTeam
            {
                get
                {
                    return AllScoreCardsByTeam
                          .Select(x => new TotalInningInfo
                          {
                              MatchUuid= x.MatchUuid,
                              TotalInningDetails = x.ScoreCard.TotalInningDetails,
                          })
                          .ToList();
                 }
            }

            public List<Player[]> AllPlaying11sByTeam
            {
                get
                {
                    return AllScoreCardsByTeam.Select(x => x.ScoreCard.Playing11).ToList();
                }
            }

            public List<MatchInfo> AllScoreCardsByTeam
            {
                get
                {
                    return cricketMatchInfoResponses
                          .Select(x => new MatchInfo
                          {
                              MatchUuid = (Guid)x.MatchUuid!,
                              ScoreCard = x.Team1.TeamName == teamName ? x.Team1 : x.Team2,
                          }).ToList();
                }
            }

            public List<TeamScoreDetails> AllScoreCardsByOpponent
            {
                get
                {
                    return cricketMatchInfoResponses
                          .Select(x => x.Team1.TeamName == teamName ? x.Team2 : x.Team1)
                          .ToList();
                }
            }

            public List<ICollection<BattingScoreCard>> AllBattingScoreCardsByTeam
            {
                get
                {
                    return AllScoreCardsByTeam
                           .Select(x => x.ScoreCard.BattingScoreCard)
                           .ToList();
                }
            }

            public List<ICollection<BattingScoreCard>> AllBattingScoreCardsByOpponent
            {
                get
                {
                    return AllScoreCardsByOpponent
                          .Select(x => x.BattingScoreCard)
                          .ToList();
                }
            }

            public List<ICollection<BowlingScoreCard>> AllBowlingScoreCardsByTeam
            {
                get
                {
                    return AllScoreCardsByOpponent
                          .Select(x => x.BowlingScoreCard)
                          .ToList();
                }
            }

            public TeamMileStones AllMileStones
            {
                get
                {
                    return GetMileStones(AllScoresByTeam, AllBattingScoreCardsByTeam, AllBowlingScoreCardsByTeam);
                }
            }

            public static TeamMileStones GetMileStones(
            IEnumerable<TotalInningInfo> allScoresByTeam,
            IEnumerable<ICollection<BattingScoreCard>> allBattingScoreCardByTeam,
            IEnumerable<ICollection<BowlingScoreCard>> allBowlingScoreCardByTeam)
            {
                var batsmanArr = allBattingScoreCardByTeam
                    .SelectMany(x => x)
                    .GroupBy(x => x.PlayerName.Href).Select(x => new
                    {
                        Player = x.First().PlayerName.Name,
                        Innings = x.Count(),
                        Runs = x.Sum(y => y.RunsScored),
                        Fours = x.Sum(y => y.Fours),
                        Sixes = x.Sum(y => y.Sixes),
                        HalfCenturies = x.Count(y => y.RunsScored > 49 && y.RunsScored < 100),
                        Centuries = x.Count(y => y.RunsScored > 99),
                        OneAndHalfCenturies = x.Count(y => y.RunsScored > 149 && y.RunsScored < 200),
                        DoubleCenturies = x.Count(y => y.RunsScored > 199),
                        HighehestIndividualScore = x.Max(x => x.RunsScored),
                    });

                var bowlersArr = allBowlingScoreCardByTeam
                    .SelectMany(x => x)
                    .GroupBy(x => x.PlayerName.Href).Select(x => new
                    {
                        Player = x.First().PlayerName.Name,
                        Wickets = x.Sum(y => y.Wickets),
                        BestBowlingInning = x.OrderByDescending(x => x.Wickets).ThenBy(x => x.RunsConceded).FirstOrDefault(),
                    });

                var mostInnings = batsmanArr.MaxBy(x => x.Innings);

                var mostRuns = batsmanArr.MaxBy(x => x.Runs);

                var mostFours = batsmanArr.MaxBy(x => x.Fours);

                var mostSixes = batsmanArr.MaxBy(x => x.Sixes);

                var most50s = batsmanArr.MaxBy(x => x.HalfCenturies);

                var most100s = batsmanArr.MaxBy(x => x.Centuries);

                var most150s = batsmanArr.MaxBy(x => x.OneAndHalfCenturies);

                var most200s = batsmanArr.MaxBy(x => x.DoubleCenturies);

                var hiScore = batsmanArr.MaxBy(x => x.HighehestIndividualScore);

                var mostWickets = bowlersArr.MaxBy(x => x.Wickets);

                var bestBowlingInning = bowlersArr.OrderByDescending(x => x.BestBowlingInning!.Wickets)
                        .ThenBy(x => x.BestBowlingInning!.RunsConceded).FirstOrDefault();

                var allBattingMilestones = allBattingScoreCardByTeam.Select(x => new
                {
                    HalfCenturies = x.Count(y => y.RunsScored > 49 && y.RunsScored < 100),
                    Centuries = x.Count(y => y.RunsScored > 99 && y.RunsScored < 150),
                    OneAndHalfCenturies = x.Count(y => y.RunsScored > 149 && y.RunsScored < 200),
                    DoubleCentury = x.Count(y => y.RunsScored > 199),
                }).ToList();

                var highestTotalScore = allScoresByTeam.MaxBy(x => x.TotalInningDetails.Runs);
                var lowestTotalScore = allScoresByTeam.Where(x => x.TotalInningDetails.Wickets == 10).MinBy(x => x.TotalInningDetails.Runs);

                if (lowestTotalScore is null)
                {
                    lowestTotalScore = allScoresByTeam.MinBy(x => x.TotalInningDetails.Runs);
                }

                return new TeamMileStones(
                     allScoresByTeam.Sum(x => x.TotalInningDetails.Runs),
                     allBattingMilestones.Sum(x => x.Centuries),
                     allBattingMilestones.Sum(x => x.HalfCenturies),
                     allBattingMilestones.Sum(x => x.OneAndHalfCenturies),
                     allBattingMilestones.Sum(x => x.DoubleCentury),
                     new InningRecordDetails(
                         new InningTotalDetails(
                             highestTotalScore!.MatchUuid,
                             highestTotalScore!.TotalInningDetails.Runs,
                             highestTotalScore.TotalInningDetails.Wickets,
                             new Over(highestTotalScore.TotalInningDetails.Overs.Overs)),
                         new InningTotalDetails(
                             lowestTotalScore!.MatchUuid,
                             lowestTotalScore!.TotalInningDetails.Runs,
                             lowestTotalScore.TotalInningDetails.Wickets,
                             new Over(lowestTotalScore.TotalInningDetails.Overs.Overs))),
                     new KeyValuePair<string, int>(mostInnings!.Player, mostInnings.Innings),
                     new KeyValuePair<string, int>(mostRuns!.Player, (int)mostRuns.Runs!),
                     new KeyValuePair<string, int>(mostWickets!.Player, (int)mostWickets.Wickets!),
                     new KeyValuePair<string, string>(bestBowlingInning!.Player, $"{bestBowlingInning.BestBowlingInning!.Wickets}/{bestBowlingInning.BestBowlingInning!.RunsConceded}"),
                     new KeyValuePair<string, string>(bestBowlingInning!.Player, $"{bestBowlingInning.BestBowlingInning!.Wickets}/{bestBowlingInning.BestBowlingInning!.RunsConceded}"),
                     new KeyValuePair<string, int>(mostSixes!.Player, (int)mostSixes.Sixes!),
                     new KeyValuePair<string, int>(mostFours!.Player, (int)mostFours.Fours!),
                     new KeyValuePair<string, int>(most50s!.Player, (int)most50s.HalfCenturies!),
                     new KeyValuePair<string, int>(most100s!.Player, (int)most100s.Centuries!),
                     new KeyValuePair<string, int>(most150s!.Player, (int)most150s.OneAndHalfCenturies!),
                     new KeyValuePair<string, int>(most200s!.Player, (int)most200s.DoubleCenturies!),
                     new KeyValuePair<string, int>(hiScore!.Player, (int)hiScore.HighehestIndividualScore!));
            }
        }

        public class TestMatchesStatisticsHelper
        {
            private readonly IQueryable<TestCricketMatchInfoResponse> cricketMatchInfoResponses;
            private readonly string teamName;

            public TestMatchesStatisticsHelper(IQueryable<TestCricketMatchInfoResponse> cricketMatchInfoResponses, string teamName)
            {
                this.cricketMatchInfoResponses = cricketMatchInfoResponses;
                this.teamName = teamName;
            }

            public List<TotalInningInfo> AllScoresByTeam
            {
                get
                {
                    return AllScoreCardsByTeam
                            .Select(x => new TotalInningInfo
                            {
                                MatchUuid = x.MatchUuid,
                                TotalInningDetails = x.TestScoreCard.Inning1.TotalInningDetails,
                            })
                            .ToList()
                            .Concat(
                                AllScoreCardsByTeam
                                .Select(x => new TotalInningInfo
                                {
                                    MatchUuid = x.MatchUuid,
                                    TotalInningDetails = x.TestScoreCard.Inning2.TotalInningDetails,
                                }))
                                .ToList();
                }
            }

            public List<Player[]> AllPlaying11sByTeam
            {
                get
                {
                    return AllScoreCardsByTeam.Select(x => x.TestScoreCard.Inning1.Playing11).ToList();
                }
            }

            public List<TestMatchInfo> AllScoreCardsByTeam
            {
                get
                {
                    return cricketMatchInfoResponses
                             .Select(x => new TestMatchInfo
                                 {
                                     MatchUuid = (Guid)x.MatchUuid!,
                                     TestScoreCard = x.Team1.TeamName == teamName ? x.Team1 : x.Team2,
                                 }).ToList();
                }
            }

            public List<TestTeamScoreDetails> AllScoreCardsByOpponent
            {
                get
                {
                    return cricketMatchInfoResponses
                              .Select(x => x.Team1.TeamName == teamName ? x.Team2 : x.Team1)
                              .ToList();
                }
            }

            public List<ICollection<BattingScoreCard>> AllBattingScoreCardsByTeam
            {
                get
                {
                    return AllScoreCardsByTeam
                            .Select(x => x.TestScoreCard.Inning1.BattingScoreCard)
                            .ToList()
                            .Concat(
                                AllScoreCardsByTeam
                                .Select(x => x.TestScoreCard.Inning2.BattingScoreCard))
                                .ToList();
                }
            }

            public List<ICollection<BattingScoreCard>> AllBattingScoreCardsByOpponent
            {
                get
                {
                    return AllScoreCardsByOpponent
                            .Select(x => x.Inning1.BattingScoreCard)
                            .ToList()
                            .Concat(
                               AllScoreCardsByOpponent
                              .Select(x => x.Inning2.BattingScoreCard))
                              .ToList();
                }
            }

            public List<ICollection<BowlingScoreCard>> AllBowlingScoreCardsByTeam
            {
                get
                {
                    return AllScoreCardsByOpponent
                             .Select(x => x.Inning1.BowlingScoreCard)
                             .ToList()
                             .Concat(
                                 AllScoreCardsByOpponent
                                .Select(x => x.Inning2.BowlingScoreCard))
                                .ToList();
                }
            }

            public TeamMileStones AllMileStones
            {
                get
                {
                    return MatchesStatisticsHelper.GetMileStones(AllScoresByTeam, AllBattingScoreCardsByTeam, AllBowlingScoreCardsByTeam);
                }
            }
        }

        public class MatchInfo
        {
            public Guid MatchUuid { get; set; }

            public TeamScoreDetails ScoreCard { get; set; } = null!;
        }

        public class TestMatchInfo
        {
            public Guid MatchUuid { get; set; }

            public TestTeamScoreDetails TestScoreCard { get; set; } = null!;
        }

        public class TotalInningInfo
        {
            public Guid MatchUuid { get; set; }

            public TotalInningScore TotalInningDetails { get; set; } = null!;
        }
    }
}
