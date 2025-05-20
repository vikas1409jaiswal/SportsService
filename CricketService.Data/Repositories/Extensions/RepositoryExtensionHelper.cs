using CricketService.Domain;
using CricketService.Domain.BaseDomains;
using CricketService.Domain.Common;
using CricketService.Domain.ResponseDomains;
using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;

namespace CricketService.Data.Repositories.Extensions
{
    public class RepositoryExtensionHelper
    {
        public class TeamMatchesHelper
        {
            protected readonly CricketTeam cricketTeam;

            public TeamMatchesHelper(
                CricketTeam cricketTeam)
            {
                this.cricketTeam = cricketTeam;
            }

            public TeamMileStones GetMileStones(
            IEnumerable<TotalInningInfo> allScoresByTeam,
            IEnumerable<ICollection<BattingScoreboardResponse>> allBattingScoreCardByTeam,
            IEnumerable<ICollection<BowlingScoreboardResponse>> allBowlingScoreCardByTeam,
            IEnumerable<Playing11Info> allPlaying11Members)
            {
                var batsmanArr = allBattingScoreCardByTeam
                    .SelectMany(x => x)
                    .GroupBy(x => x.PlayerName.Href)
                    .Select(x => new
                    {
                        Href = x.Key,
                        Player = x.First().PlayerName.Name?.Replace("(c)", string.Empty).TrimEnd(),
                        Innings = x.Count(),
                        Runs = x.Sum(y => y.RunsScored),
                        Fours = x.Sum(y => y.Fours),
                        Sixes = x.Sum(y => y.Sixes),
                        Ducks = x.Count(y => y.RunsScored == 0 && !y.OutStatus.Contains("not out")),
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
                        Href = x.Key,
                        Player = x.First().PlayerName.Name?.Replace("(c)", string.Empty).TrimEnd(),
                        Wickets = x.Sum(y => y.Wickets),
                        BestBowlingInning = x.OrderByDescending(x => x.Wickets)
                                             .ThenBy(x => x.RunsConceded)
                                             .FirstOrDefault(),
                    });

                var mostInnings = batsmanArr.MaxBy(x => x.Innings);

                var mostRuns = batsmanArr.MaxBy(x => x.Runs);

                var mostFours = batsmanArr.MaxBy(x => x.Fours);

                var mostSixes = batsmanArr.MaxBy(x => x.Sixes);

                var mostDucks = batsmanArr.MaxBy(x => x.Ducks);

                var most50s = batsmanArr.MaxBy(x => x.HalfCenturies);

                var most100s = batsmanArr.MaxBy(x => x.Centuries);

                var most150s = batsmanArr.MaxBy(x => x.OneAndHalfCenturies);

                var most200s = batsmanArr.MaxBy(x => x.DoubleCenturies);

                var hiScore = batsmanArr.MaxBy(x => x.HighehestIndividualScore);

                var mostWickets = bowlersArr.MaxBy(x => x.Wickets);

                var bestBowlingInning = bowlersArr.OrderByDescending(x => x.BestBowlingInning!.Wickets)
                                                  .ThenBy(x => x.BestBowlingInning!.RunsConceded)
                                                  .FirstOrDefault();

                var allBattingMilestones = allBattingScoreCardByTeam.Select(x => new
                {
                    Ducks = x.Count(y => y.RunsScored == 0 && !y.OutStatus.Contains("not out")),
                    HalfCenturies = x.Count(y => y.RunsScored > 49 && y.RunsScored < 100),
                    Centuries = x.Count(y => y.RunsScored > 99 && y.RunsScored < 150),
                    OneAndHalfCenturies = x.Count(y => y.RunsScored > 149 && y.RunsScored < 200),
                    DoubleCentury = x.Count(y => y.RunsScored > 199),
                }).ToList();

                var highestTotalScore = allScoresByTeam.MaxBy(x => x.TotalInningDetails.Runs);

                var lowestTotalScore = allScoresByTeam.Where(x => x.TotalInningDetails.Wickets == 10)
                                                      .MinBy(x => x.TotalInningDetails.Runs);

                if (lowestTotalScore is null)
                {
                    lowestTotalScore = allScoresByTeam.MinBy(x => x.TotalInningDetails.Runs);
                }

                var playersRepresented = new List<KeyValuePair<CricketPlayer, PlayerRepresentedDetails>>();
                var captainsRepresented = new List<KeyValuePair<CricketPlayer, PlayerRepresentedDetails>>();
                var wicketKeepersRepresented = new List<KeyValuePair<CricketPlayer, PlayerRepresentedDetails>>();

                foreach (Playing11Info p11info in allPlaying11Members)
                {
                    var allAvailablePlayersHrefs = playersRepresented.Select(x => x.Key.Href);
                    var allAvailableCaptainsHrefs = captainsRepresented.Select(x => x.Key.Href);
                    var allAvailablewicketKeepersHrefs = wicketKeepersRepresented.Select(x => x.Key.Href);

                    foreach (var member in p11info.Playing11Members)
                    {
                        if (!allAvailablePlayersHrefs.Contains(member.Href))
                        {
                            playersRepresented.Add(
                                new KeyValuePair<CricketPlayer, PlayerRepresentedDetails>(
                                    new CricketPlayer(member.Name.TrimEnd(), member.Href),
                                    new PlayerRepresentedDetails()
                                    {
                                        FirstMatchDate = p11info.MatchDate,
                                        FirstMatchUuid = p11info.MatchUuid,
                                    }));
                        }

                        if (member.Name.Contains("(c)") && !allAvailableCaptainsHrefs.Contains(member.Href))
                        {
                            captainsRepresented.Add(
                                new KeyValuePair<CricketPlayer, PlayerRepresentedDetails>(
                                    new CricketPlayer(member.Name.TrimEnd(), member.Href),
                                    new PlayerRepresentedDetails()
                                    {
                                        FirstMatchDate = p11info.MatchDate,
                                        FirstMatchUuid = p11info.MatchUuid,
                                    }));
                        }

                        if (member.Name.Contains("†") && !allAvailablewicketKeepersHrefs.Contains(member.Href))
                        {
                            wicketKeepersRepresented.Add(
                                new KeyValuePair<CricketPlayer, PlayerRepresentedDetails>(
                                    new CricketPlayer(member.Name.TrimEnd(), member.Href),
                                    new PlayerRepresentedDetails()
                                    {
                                        FirstMatchDate = p11info.MatchDate,
                                        FirstMatchUuid = p11info.MatchUuid,
                                    }));
                        }
                    }
                }

                return new TeamMileStones(
                     careerRuns: allScoresByTeam.Sum(x => x.TotalInningDetails.Runs),
                     careerDucks: allBattingMilestones.Sum(x => x.Ducks),
                     careerCenturies: allBattingMilestones.Sum(x => x.Centuries),
                     careerHalfCenturies: allBattingMilestones.Sum(x => x.HalfCenturies),
                     careerOneAndHalfCenturies: allBattingMilestones.Sum(x => x.OneAndHalfCenturies),
                     careerDoubleCenturies: allBattingMilestones.Sum(x => x.DoubleCentury),
                     inningRecordDetails: new InningRecordDetails(
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
                     mostInnings: new KeyValuePair<string, int>(mostInnings!.Player, mostInnings.Innings),
                     mostRuns: new KeyValuePair<string, int>(mostRuns!.Player, (int)mostRuns.Runs!),
                     mostWickets: new KeyValuePair<string, int>(mostWickets!.Player, mostWickets.Wickets!),
                     mostDucks: new KeyValuePair<string, int>(mostDucks!.Player, mostDucks.Ducks!),
                     bestBowlingMatch: new KeyValuePair<string, string>(bestBowlingInning!.Player, $"{bestBowlingInning.BestBowlingInning!.Wickets}/{bestBowlingInning.BestBowlingInning!.RunsConceded}"),
                     bestBowlingInning: new KeyValuePair<string, string>(bestBowlingInning!.Player, $"{bestBowlingInning.BestBowlingInning!.Wickets}/{bestBowlingInning.BestBowlingInning!.RunsConceded}"),
                     mostSixes: new KeyValuePair<string, int>(mostSixes!.Player, (int)mostSixes.Sixes!),
                     mostFours: new KeyValuePair<string, int>(mostFours!.Player, (int)mostFours.Fours!),
                     most50s: new KeyValuePair<string, int>(most50s!.Player, most50s.HalfCenturies!),
                     most100s: new KeyValuePair<string, int>(most100s!.Player, most100s.Centuries!),
                     most150s: new KeyValuePair<string, int>(most150s!.Player, most150s.OneAndHalfCenturies!),
                     most200s: new KeyValuePair<string, int>(most200s!.Player, most200s.DoubleCenturies!),
                     hiScore: new KeyValuePair<string, int>(hiScore!.Player, (int)hiScore.HighehestIndividualScore!),
                     playersRepresented: playersRepresented,
                     captainsRepresented: captainsRepresented,
                     wicketKeepersRepresented: wicketKeepersRepresented);
            }
        }

        public class TeamMatchesStatisticsHelper : TeamMatchesHelper
        {
            private readonly List<InternationalCricketMatchResponse> cricketMatchInfoResponses;

            public TeamMatchesStatisticsHelper(
                List<InternationalCricketMatchResponse> cricketMatchInfoResponses,
                CricketTeam cricketTeam)
                : base(cricketTeam)
            {
                this.cricketMatchInfoResponses = cricketMatchInfoResponses;
            }

            public List<InternationalCricketMatchResponse> AllMatchesByTeam
            {
                get
                {
                    return cricketMatchInfoResponses.AsEnumerable()
                    .Where(x => x.Team1.Team.Uuid.Equals(cricketTeam.Uuid)
                                || x.Team2.Team.Uuid.Equals(cricketTeam.Uuid))
                    .ToList();
                }
            }

            public List<TotalInningInfo> AllScoresByTeam
            {
                get
                {
                    //foreach (var allScoreCardByTeam in AllScoreCardsByTeam)
                    //{
                    //    allScoreCardByTeam.ScoreCard.SetTotalInningScore();
                    //}

                    return AllScoreCardsByTeam
                          .Select(x => new TotalInningInfo
                          {
                              MatchUuid = x.MatchUuid,
                              MatchDate = x.MatchDate,
                              TotalInningDetails = x.ScoreCard.TotalInningDetails,
                          })
                          .ToList();
                 }
            }

            public List<Playing11Info> AllPlaying11sByTeam
            {
                get
                {
                    return AllScoreCardsByTeam
                        .Select(x => new Playing11Info()
                        {
                          Playing11Members = x.ScoreCard.Playing11 is not null
                                             ? x.ScoreCard.Playing11.ToList()
                                             : new List<CricketPlayer>(),
                          MatchUuid = x.MatchUuid,
                          MatchDate = x.MatchDate,
                        }).ToList();
                }
            }

            public List<MatchInfo> AllScoreCardsByTeam
            {
                get
                {
                    return AllMatchesByTeam
                          .Select(x => new MatchInfo
                          {
                              MatchUuid = x.MatchUuid,
                              MatchDate = DateOnly.FromDateTime(DateTime.ParseExact(x.MatchDate, "MMM dd yyyy", CultureInfo.InvariantCulture)),
                              ScoreCard = x.Team1.Team.Uuid.Equals(cricketTeam.Uuid) ? x.Team1 : x.Team2,
                          }).ToList();
                }
            }

            public List<MatchInfo> AllScoreCardsByOpponent
            {
                get
                {
                    return AllMatchesByTeam
                          .Select(x => new MatchInfo
                          {
                              MatchUuid = x.MatchUuid,
                              MatchDate = DateOnly.FromDateTime(DateTime.ParseExact(x.MatchDate, "MMM dd yyyy", CultureInfo.InvariantCulture)),
                              ScoreCard = x.Team1.Team.Uuid.Equals(cricketTeam.Uuid) ? x.Team2 : x.Team1,
                          }).ToList();
                }
            }

            public List<ICollection<BattingScoreboardResponse>> AllBattingScoreCardsByTeam
            {
                get
                {
                    return AllScoreCardsByTeam
                           .Select(x => x.ScoreCard.BattingScoreCard)
                           .ToList();
                }
            }

            public List<ICollection<BattingScoreboardResponse>> AllBattingScoreCardsByOpponent
            {
                get
                {
                    return AllScoreCardsByOpponent
                          .Select(x => x.ScoreCard.BattingScoreCard)
                          .ToList();
                }
            }

            public List<ICollection<BowlingScoreboardResponse>> AllBowlingScoreCardsByTeam
            {
                get
                {
                    return AllScoreCardsByOpponent
                          .Select(x => x.ScoreCard.BowlingScoreCard)
                          .ToList();
                }
            }

            public TeamMileStones AllMileStonesByTeam
            {
                get
                {
                    return GetMileStones(
                        AllScoresByTeam,
                        AllBattingScoreCardsByTeam,
                        AllBowlingScoreCardsByTeam,
                        AllPlaying11sByTeam);
                }
            }
        }

        public class TestMatchesStatisticsHelper : TeamMatchesHelper
        {
            private readonly List<TestCricketMatchResponse> cricketMatchInfoResponses;

            public TestMatchesStatisticsHelper(
                List<TestCricketMatchResponse> cricketMatchInfoResponses,
                CricketTeam cricketTeam)
                : base(cricketTeam)
            {
                this.cricketMatchInfoResponses = cricketMatchInfoResponses;
            }

            public List<TestCricketMatchResponse> AllMatchesByTeam
            {
                get
                {
                    return cricketMatchInfoResponses.AsEnumerable()
                    .Where(x => x.Team1.Team.Uuid.Equals(cricketTeam.Uuid) || x.Team2.Team.Uuid.Equals(cricketTeam.Uuid)).ToList();
                }
            }

            public List<TotalInningInfo> AllScoresByTeam
            {
                get
                {
                    return AllScoreCardsByTeam
                            .Select(x => new TotalInningInfo
                            {
                                MatchUuid = x.MatchUuid,
                                MatchDate = x.MatchDate,
                                TotalInningDetails = x.TestScoreCard.Inning1.TotalInningDetails,
                            })
                            .ToList()
                            .Concat(
                                AllScoreCardsByTeam
                                .Select(x => new TotalInningInfo
                                {
                                    MatchUuid = x.MatchUuid,
                                    MatchDate = x.MatchDate,
                                    TotalInningDetails = x.TestScoreCard.Inning2.TotalInningDetails,
                                }))
                                .ToList();
                }
            }

            public List<Playing11Info> AllPlaying11sByTeam
            {
                get
                {
                    return AllScoreCardsByTeam.Select(x => new Playing11Info()
                    {
                        Playing11Members = x.TestScoreCard.Inning1.Playing11 is not null
                                           ? x.TestScoreCard.Inning1.Playing11.ToList()
                                           : new List<CricketPlayer>(),
                        MatchUuid = x.MatchUuid,
                        MatchDate = x.MatchDate,
                    }).ToList(); ;
                }
            }

            public List<TestMatchInfo> AllScoreCardsByTeam
            {
                get
                {
                    return AllMatchesByTeam
                             .Select(x => new TestMatchInfo
                                 {
                                     MatchUuid = (Guid)x.MatchUuid!,
                                     MatchDate = DateOnly.FromDateTime(DateTime.ParseExact(x.MatchDate, "MMM dd yyyy", CultureInfo.InvariantCulture)),
                                     TestScoreCard = x.Team1.Team.Uuid.Equals(cricketTeam.Uuid) ? x.Team1 : x.Team2,
                                 }).ToList();
                }
            }

            public List<TestMatchInfo> AllScoreCardsByOpponent
            {
                get
                {
                    return AllMatchesByTeam
                             .Select(x => new TestMatchInfo
                             {
                                 MatchUuid = x.MatchUuid,
                                 MatchDate = DateOnly.FromDateTime(DateTime.ParseExact(x.MatchDate, "MMM dd yyyy", CultureInfo.InvariantCulture)),
                                 TestScoreCard = x.Team1.Team.Uuid.Equals(cricketTeam.Uuid) ? x.Team2 : x.Team1,
                             }).ToList();
                }
            }

            public List<ICollection<BattingScoreboardResponse>> AllBattingScoreCardsByTeam
            {
                get
                {
                    return AllScoreCardsByTeam
                            .Select(x => x.TestScoreCard.Inning1.BattingScorecboard)
                            .ToList()
                            .Concat(
                                AllScoreCardsByTeam
                                .Select(x => x.TestScoreCard.Inning2.BattingScorecboard))
                                .ToList();
                }
            }

            public List<ICollection<BattingScoreboardResponse>> AllBattingScoreCardsByOpponent
            {
                get
                {
                    return AllScoreCardsByOpponent
                            .Select(x => x.TestScoreCard.Inning1.BattingScorecboard)
                            .ToList()
                            .Concat(
                               AllScoreCardsByOpponent
                              .Select(x => x.TestScoreCard.Inning2.BattingScorecboard))
                              .ToList();
                }
            }

            public List<ICollection<BowlingScoreboardResponse>> AllBowlingScoreCardsByTeam
            {
                get
                {
                    return AllScoreCardsByOpponent
                             .Select(x => x.TestScoreCard.Inning1.BowlingScoreboard)
                             .ToList()
                             .Concat(
                                 AllScoreCardsByOpponent
                                .Select(x => x.TestScoreCard.Inning2.BowlingScoreboard))
                                .ToList();
                }
            }

            public TeamMileStones AllMileStones
            {
                get
                {
                    return GetMileStones(
                        AllScoresByTeam,
                        AllBattingScoreCardsByTeam,
                        AllBowlingScoreCardsByTeam,
                        AllPlaying11sByTeam);
                }
            }
        }

        #region Domain Classes
        public class MatchInfo
        {
            public Guid MatchUuid { get; set; }

            public DateOnly MatchDate { get; set; }

            public SingleInningTeamScoreboardResponse ScoreCard { get; set; } = null!;
        }

        public class TestMatchInfo
        {
            public Guid MatchUuid { get; set; }

            public DateOnly MatchDate { get; set; }

            public DoubleInningTeamScoreboardResponse TestScoreCard { get; set; } = null!;
        }

        public class TotalInningInfo
        {
            public Guid MatchUuid { get; set; }

            public DateOnly MatchDate { get; set; }

            public TotalInningScore TotalInningDetails { get; set; } = null!;
        }

        public class Playing11Info
        {
            public Guid MatchUuid { get; set; }

            public DateOnly MatchDate { get; set; }

            public List<CricketPlayer> Playing11Members { get; set; } = new();
        }
        #endregion
    }
}
