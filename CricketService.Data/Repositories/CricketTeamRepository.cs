using AutoMapper;
using CricketService.Data.Contexts;
using CricketService.Data.Extensions;
using CricketService.Data.Repositories.Extensions;
using CricketService.Data.Repositories.Interfaces;
using CricketService.Domain;
using CricketService.Domain.Enums;
using CricketService.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace CricketService.Data.Repositories
{
    public class CricketTeamRepository : ICricketTeamRepository
    {
        private readonly ILogger<CricketPlayerRepository> logger;
        private readonly CricketServiceContext context;
        private readonly IMapper mapper;

        public CricketTeamRepository(
            ILogger<CricketPlayerRepository> logger,
            CricketServiceContext context,
            IMapper mapper)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }

        public IEnumerable<Team> GetAllTeamDetails()
        {
            return context.CricketTeamInfo
                 .Select(x => new Team(
                     x.Uuid,
                     x.TeamName,
                     x.Formats,
                     x.LogoUrl,
                     x.FlagUrl));
        }

        public IEnumerable<string> GetAllTeamNames(CricketFormat format)
        {
            List<string> teams = new();

            if (format == CricketFormat.All)
            {
                teams = context.CricketTeamInfo.AsEnumerable()
                  .Select(x => x.TeamName)
                  .OrderBy(x => x).ToList();
            }
            else
            {
                teams = context.CricketTeamInfo.AsEnumerable()
                 .Where(x => x.Formats.Contains(format.ToString()))
                 .Select(x => x.TeamName)
                 .OrderBy(x => x).ToList();
            }

            return teams;
        }

        public CricketTeamInfoResponse GetTeamRecordsByNamex(string teamName, bool isSingle = false)
        {
            var cricketMatchInfoTableODI = context.ODICricketMatchInfo;
            var cricketMatchInfoTableT20I = context.T20ICricketMatchInfo;
            var cricketMatchInfoTableTest = context.TestCricketMatchInfo;
            var cricketTeamInfoTable = context.CricketTeamInfo;

            var allMatchesByTeamODI = cricketMatchInfoTableODI
                .OrderBy(x => Convert.ToInt32(x.MatchNo.Replace("ODI no. ", string.Empty)))
                .Where(x => x.Team1.TeamName == teamName || x.Team2.TeamName == teamName);

            var allMatchesByTeamT20I = cricketMatchInfoTableT20I
                .OrderBy(x => Convert.ToInt32(x.MatchNo.Replace("T20I no. ", string.Empty)))
                .Where(x => x.Team1.TeamName == teamName || x.Team2.TeamName == teamName);

            var allMatchesByTeamTest = cricketMatchInfoTableTest
                .OrderBy(x => Convert.ToInt32(x.MatchNo.Replace("Test no. ", string.Empty)))
                .Where(x => x.Team1.TeamName == teamName || x.Team2.TeamName == teamName);

            Entities.CricketTeamInfo teamInfo = null!;

            try
            {
                teamInfo = cricketTeamInfoTable.Single(t => t.TeamName == teamName);
            }
            catch
            {
                throw new CricketTeamNotFoundException($"{teamName} does not exists.");
            }

            var teamRecordData = new CricketTeamInfoResponse(
                teamInfo.Uuid,
                teamName,
                new TeamRecordDetails(null!, null!, null!),
                teamInfo.FlagUrl);

            if (allMatchesByTeamT20I.Any())
            {
                DebutDetails debutDetails = null!;
                TeamMileStones teamMileStones = null!;

                if (isSingle)
                {
                    var debutMatchDetails = allMatchesByTeamT20I.First();

                    var debutOpponent = debutMatchDetails.Team1.TeamName == teamName ? debutMatchDetails.Team2.TeamName : debutMatchDetails.Team1.TeamName;

                    var cricketMatchInfoResponses = allMatchesByTeamT20I.Select(x => x.ToDomain(mapper));

                    teamMileStones = cricketMatchInfoResponses.GetMileStones(teamName);

                    debutDetails = new DebutDetails(
                                                   debutMatchDetails.Uuid,
                                                   debutMatchDetails.MatchDate,
                                                   debutOpponent,
                                                   debutMatchDetails.Venue,
                                                   debutMatchDetails.Result,
                                                   debutMatchDetails.MatchNo);
                }

                teamRecordData.TeamRecordDetails.T20IResults = new TeamFormatRecordDetails(
                                           debut: debutDetails,
                                           matches: allMatchesByTeamT20I.Count(),
                                           won: allMatchesByTeamT20I.Count(m => m.Result.Contains($"{teamName} won by")),
                                           lost: allMatchesByTeamT20I.Count(m => (m.Team1.TeamName == teamName && m.Result.Contains(m.Team2.TeamName + " won by"))
                                                 || (m.Team2.TeamName == teamName && m.Result.Contains(m.Team1.TeamName + " won by"))),
                                           tied: allMatchesByTeamT20I.Count(m => m.Result.Contains("Match tied")),
                                           noResult: allMatchesByTeamT20I.Count(m => m.Result.Contains("No result")),
                                           teamMileStones: teamMileStones);
            }

            if (allMatchesByTeamODI.Any())
            {
                DebutDetails debutDetails = null!;
                TeamMileStones teamMileStones = null!;

                if (isSingle)
                {
                    var debutMatchDetails = allMatchesByTeamODI.First();

                    var debutOpponent = debutMatchDetails.Team1.TeamName == teamName ? debutMatchDetails.Team2.TeamName : debutMatchDetails.Team1.TeamName;

                    var cricketMatchInfoResponses = allMatchesByTeamODI.Select(x => x.ToDomain(mapper));

                    teamMileStones = cricketMatchInfoResponses.GetMileStones(teamName);

                    debutDetails = new DebutDetails(
                                                   debutMatchDetails.Uuid,
                                                   debutMatchDetails.MatchDate,
                                                   debutOpponent,
                                                   debutMatchDetails.Venue,
                                                   debutMatchDetails.Result,
                                                   debutMatchDetails.MatchNo);
                }

                teamRecordData.TeamRecordDetails.ODIResults = new TeamFormatRecordDetails(
                                           debut: debutDetails,
                                           matches: allMatchesByTeamODI.Count(),
                                           won: allMatchesByTeamODI.Count(m => m.Result.Contains($"{teamName} won by")),
                                           lost: allMatchesByTeamODI.Count(m => (m.Team1.TeamName == teamName && m.Result.Contains(m.Team2.TeamName + " won by"))
                                                 || (m.Team2.TeamName == teamName && m.Result.Contains(m.Team1.TeamName + " won by"))),
                                           tied: allMatchesByTeamODI.Count(m => m.Result.Contains("Match tied")),
                                           noResult: allMatchesByTeamODI.Count(m => m.Result.Contains("No result")),
                                           teamMileStones: teamMileStones);
            }

            if (allMatchesByTeamTest.Any())
            {
                DebutDetails debutDetails = null!;
                TeamMileStones teamMileStones = null!;

                if (isSingle)
                {
                    var debutMatchDetails = allMatchesByTeamTest.First();

                    var debutOpponent = debutMatchDetails.Team1.TeamName == teamName ? debutMatchDetails.Team2.TeamName : debutMatchDetails.Team1.TeamName;

                    var cricketMatchInfoResponses = allMatchesByTeamTest.Select(x => x.ToDomain(mapper));

                    teamMileStones = cricketMatchInfoResponses.GetTestMileStones(teamName);

                    debutDetails = new DebutDetails(
                                                   debutMatchDetails.Uuid,
                                                   debutMatchDetails.MatchDates,
                                                   debutOpponent,
                                                   debutMatchDetails.Venue,
                                                   debutMatchDetails.Result,
                                                   debutMatchDetails.MatchNo);
                }

                teamRecordData.TeamRecordDetails.TestResults = new TestTeamFormatRecordDetails(
                                   debut: debutDetails,
                                   matches: allMatchesByTeamTest.Count(),
                                   won: allMatchesByTeamTest.Count(m => m.Result.Contains($"{teamName} won by")),
                                   lost: allMatchesByTeamTest.Count(m => (m.Team1.TeamName == teamName && m.Result.Contains(m.Team2.TeamName + " won by"))
                                         || (m.Team2.TeamName == teamName && m.Result.Contains(m.Team1.TeamName + " won by"))),
                                   tied: allMatchesByTeamTest.Count(m => m.Result.Contains("Match tied")),
                                   draw: allMatchesByTeamTest.Count(m => m.Result.Contains("Match drawn")),
                                   teamMileStones: teamMileStones);
            }

            return teamRecordData;
        }

        public object GetAllAgainstRecordsByTeamT20I(Guid teamUuid)
        {
            Entities.CricketTeamInfo teamInfo;

            try
            {
                teamInfo = context.CricketTeamInfo.Single(x => x.Uuid == teamUuid);
            }
            catch
            {
                throw new CricketTeamNotFoundException($"team with uuid {teamUuid} does not exists.");
            }

            var cricketMatchInfoTableT20I = context.T20ICricketMatchInfo;

            var allMatchesByTeamT20I = cricketMatchInfoTableT20I.Where(x => x.Team1.TeamName == teamInfo.TeamName || x.Team2.TeamName == teamInfo.TeamName);

            var recordAgainstTeams = allMatchesByTeamT20I
                 .GroupBy(m => m.Team1.TeamName == teamInfo.TeamName ? m.Team2.TeamName : m.Team1.TeamName);

            return recordAgainstTeams.Select(g => new
            {
                Opponent = g.Key,
                Matches = g.Count(),
                Won = g.Count(x => x.Result.Contains($"{teamInfo.TeamName} won by")),
                Lost = g.Count(m => (m.Team1.TeamName == teamInfo.TeamName && m.Result.Contains(m.Team2.TeamName + " won by"))
                                                 || (m.Team2.TeamName == teamInfo.TeamName && m.Result.Contains(m.Team1.TeamName + " won by"))),
                Tied = g.Count(x => x.Result.Contains("Match tied")),
                NoResult = g.Count(x => x.Result.Contains("No result")),
                MatchesBySeason = g.OrderBy(x => Convert.ToInt32(x.MatchNo.Replace("T20I no. ", string.Empty)))
                .GroupBy(x => x.Season)
                .Select(x => new
                {
                    Season = x.Key,
                    Matches = x.Select(y => new
                    {
                        Title = y.Series,
                        Result = y.Result,
                        MatchUuid = y.Uuid,
                        MatchNo = y.MatchNo,
                        Date = y.MatchDate,
                    }),
                }),
            });
        }

        public object GetAllAgainstRecordsByTeamODI(Guid teamUuid)
        {
            Entities.CricketTeamInfo teamInfo;

            try
            {
                teamInfo = context.CricketTeamInfo.Single(x => x.Uuid == teamUuid);
            }
            catch
            {
                throw new CricketTeamNotFoundException($"team with uuid {teamUuid} does not exists.");
            }

            var cricketMatchInfoTableODI = context.ODICricketMatchInfo;

            var allMatchesByTeamODI = cricketMatchInfoTableODI.Where(x => x.Team1.TeamName == teamInfo.TeamName || x.Team2.TeamName == teamInfo.TeamName);

            var recordAgainstTeams = allMatchesByTeamODI
                 .GroupBy(m => m.Team1.TeamName == teamInfo.TeamName ? m.Team2.TeamName : m.Team1.TeamName);

            return recordAgainstTeams.Select(g => new
            {
                Opponent = g.Key,
                Matches = g.Count(),
                Won = g.Count(x => x.Result.Contains($"{teamInfo.TeamName} won by")),
                Lost = g.Count(m => (m.Team1.TeamName == teamInfo.TeamName && m.Result.Contains(m.Team2.TeamName + " won by"))
                                                 || (m.Team2.TeamName == teamInfo.TeamName && m.Result.Contains(m.Team1.TeamName + " won by"))),
                Tied = g.Count(x => x.Result.Contains("Match tied")),
                NoResult = g.Count(x => x.Result.Contains("No result")),
                MatchesBySeason = g.OrderBy(x => Convert.ToInt32(x.MatchNo.Replace("ODI no. ", string.Empty)))
                .GroupBy(x => x.Season)
                .Select(x => new
                {
                    Season = x.Key,
                    Matches = x.Select(y => new
                    {
                        Title = y.Series,
                        Result = y.Result,
                        MatchUuid = y.Uuid,
                        MatchNo = y.MatchNo,
                        Date = y.MatchDate,
                    }),
                }),
            });
        }

        public object GetAllAgainstRecordsByTeamTest(Guid teamUuid)
        {
            Entities.CricketTeamInfo teamInfo;

            try
            {
                teamInfo = context.CricketTeamInfo.Single(x => x.Uuid == teamUuid);
            }
            catch
            {
                throw new CricketTeamNotFoundException($"team with uuid {teamUuid} does not exists.");
            }

            var cricketMatchInfoTableTest = context.TestCricketMatchInfo
                .OrderBy(x => Convert.ToInt32(x.MatchNo.Replace("Test no. ", string.Empty)));

            var allMatchesByTeamTest = cricketMatchInfoTableTest.Where(x => x.Team1.TeamName == teamInfo.TeamName || x.Team2.TeamName == teamInfo.TeamName);

            var recordAgainstTeams = allMatchesByTeamTest
                 .GroupBy(m => m.Team1.TeamName == teamInfo.TeamName ? m.Team2.TeamName : m.Team1.TeamName);

            return recordAgainstTeams.Select(g => new
            {
                Opponent = g.Key,
                Matches = g.Count(),
                Won = g.Count(x => x.Result.Contains($"{teamInfo.TeamName} won by")),
                Lost = g.Count(m => (m.Team1.TeamName == teamInfo.TeamName && m.Result.Contains(m.Team2.TeamName + " won by"))
                                                 || (m.Team2.TeamName == teamInfo.TeamName && m.Result.Contains(m.Team1.TeamName + " won by"))),
                Tied = g.Count(x => x.Result.Contains("Match tied")),
                Draw = g.Count(x => x.Result.Contains("Match drawn")),
                MatchesBySeason = g.OrderBy(x => Convert.ToInt32(x.MatchNo.Replace("Test no. ", string.Empty)))
                .GroupBy(x => x.Season)
                .Select(x => new
                {
                    Season = x.Key,
                    Matches = x.Select(y => new
                    {
                      Title = y.Series,
                      Result = y.Result,
                      MatchUuid = y.Uuid,
                      MatchNo = y.MatchNo,
                      Date = y.MatchDates,
                    }),
                }),
            });
        }

        public CricketTeamInfoResponse GetTeamByName(string teamName)
        {
            Entities.CricketTeamInfo teamInfo;

            try
            {
                teamInfo = context.CricketTeamInfo.Single(x => x.TeamName == teamName);
            }
            catch
            {
                throw new CricketTeamNotFoundException($"team with name {teamName} does not exists.");
            }

            return new CricketTeamInfoResponse(
                teamInfo.Uuid,
                teamInfo.TeamName,
                new TeamRecordDetails(teamInfo.T20IRecords, teamInfo.ODIRecords, teamInfo.TestRecords),
                teamInfo.FlagUrl);
        }

        public CricketTeamInfoResponse GetTeamByUuid(Guid teamUuid)
        {
            Entities.CricketTeamInfo teamInfo;

            try
            {
                teamInfo = context.CricketTeamInfo.Single(x => x.Uuid == teamUuid);
            }
            catch
            {
                throw new CricketTeamNotFoundException($"team with uuid {teamUuid} does not exists.");
            }

            return new CricketTeamInfoResponse(
                teamInfo.Uuid,
                teamInfo.TeamName,
                new TeamRecordDetails(teamInfo.T20IRecords, teamInfo.ODIRecords, teamInfo.TestRecords),
                teamInfo.FlagUrl);
        }
    }
}