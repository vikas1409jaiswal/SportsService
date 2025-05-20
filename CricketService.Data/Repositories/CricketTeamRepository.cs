using AutoMapper;
using CricketService.Data.Contexts;
using CricketService.Data.Entities;
using CricketService.Data.Extensions;
using CricketService.Data.Repositories.Extensions;
using CricketService.Data.Repositories.Interfaces;
using CricketService.Data.Utils;
using CricketService.Domain;
using CricketService.Domain.BaseDomains;
using CricketService.Domain.Enums;
using CricketService.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CricketService.Data.Repositories
{
    public class CricketTeamRepository : ICricketTeamRepository
    {
        private readonly ILogger<CricketPlayerRepository> logger;
        private readonly CricketServiceContext context;
        private readonly IMapper mapper;
        private readonly TeamPDFHandler teamPDFHandler;

        public CricketTeamRepository(
            ILogger<CricketPlayerRepository> logger,
            CricketServiceContext context,
            IMapper mapper,
            TeamPDFHandler teamPDFHandler)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
            this.teamPDFHandler = teamPDFHandler;
        }

        public IEnumerable<Guid> GetAllTeamsUuid()
        {
            return context.CricketTeamInfo.Select(x => x.Uuid);
        }

        public IEnumerable<PlayerTeam> GetAllTeamDetails()
        {
            return context.CricketTeamInfo
                 .Select(x => new PlayerTeam(
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

        public IEnumerable<RecordAgainstTeam> GetAllAgainstRecordsByTeamT20I(Guid teamUuid)
        {
            Entities.CricketTeamInfoDTO teamInfo;

            try
            {
                teamInfo = context.CricketTeamInfo.Single(x => x.Uuid == teamUuid);
            }
            catch
            {
                throw new CricketTeamNotFoundException($"team with uuid {teamUuid} does not exists.");
            }

            var cricketMatchInfoTableT20I = context.LimitedOverInternationalMatchesInfo;

            var allMatchesByTeamT20I = cricketMatchInfoTableT20I.Where(x => x.MatchNumber.Contains("T20I no.") && ( x.Team1.Team.Name == teamInfo.TeamName || x.Team2.Team.Name == teamInfo.TeamName));

            var recordAgainstTeams = allMatchesByTeamT20I
                 .GroupBy(m => m.Team1.Team.Name == teamInfo.TeamName ? m.Team2.Team.Name : m.Team1.Team.Name);

            return recordAgainstTeams.Select(g => new RecordAgainstTeam(
                g.Key,
                g.Count(),
                g.Count(x => x.Result.Contains($"{teamInfo.TeamName} won by")),
                g.Count(m => (m.Team1.Team.Name == teamInfo.TeamName && m.Result.Contains(m.Team2.Team.Name + " won by"))
                                                 || (m.Team2.Team.Name == teamInfo.TeamName && m.Result.Contains(m.Team1.Team.Name + " won by"))),
                g.Count(x => x.Result.Contains("Match tied")),
                g.Count(x => x.Result.Contains("No result")),
                g.OrderBy(x => Convert.ToInt32(x.MatchNumber.Replace("T20I no. ", string.Empty)))
                .GroupBy(x => x.Season)
                .Select(x => new MatchesBySeason(x.Key, x.Select(y => new MatchDetail(y.Uuid, y.Series, y.Result, y.MatchNumber, y.MatchDate))))));
        }

        public IEnumerable<RecordAgainstTeam> GetAllAgainstRecordsByTeamODI(Guid teamUuid)
        {
            Entities.CricketTeamInfoDTO teamInfo;

            try
            {
                teamInfo = context.CricketTeamInfo.Single(x => x.Uuid == teamUuid);
            }
            catch
            {
                throw new CricketTeamNotFoundException($"team with uuid {teamUuid} does not exists.");
            }

            var cricketMatchInfoTableODI = context.LimitedOverInternationalMatchesInfo;

            var allMatchesByTeamODI = cricketMatchInfoTableODI.Where(x => x.MatchNumber.Contains("ODI no.") && (x.Team1.Team.Name == teamInfo.TeamName || x.Team2.Team.Name == teamInfo.TeamName));

            var recordAgainstTeams = allMatchesByTeamODI
                 .GroupBy(m => m.Team1.Team.Name == teamInfo.TeamName ? m.Team2.Team.Name : m.Team1.Team.Name);

            return recordAgainstTeams.Select(g => new RecordAgainstTeam(
                 g.Key,
                 g.Count(),
                 g.Count(x => x.Result.Contains($"{teamInfo.TeamName} won by")),
                 g.Count(m => (m.Team1.Team.Name == teamInfo.TeamName && m.Result.Contains(m.Team2.Team.Name + " won by"))
                                                  || (m.Team2.Team.Name == teamInfo.TeamName && m.Result.Contains(m.Team1.Team.Name + " won by"))),
                 g.Count(x => x.Result.Contains("Match tied")),
                 g.Count(x => x.Result.Contains("No result")),
                 g.OrderBy(x => Convert.ToInt32(x.MatchNumber.Replace("T20I no. ", string.Empty)))
                 .GroupBy(x => x.Season)
                 .Select(x => new MatchesBySeason(x.Key, x.Select(y => new MatchDetail(y.Uuid, y.Series, y.Result, y.MatchNumber, y.MatchDate))))));
        }

        public IEnumerable<RecordAgainstTeam> GetAllAgainstRecordsByTeamTest(Guid teamUuid)
        {
            Entities.CricketTeamInfoDTO teamInfo;

            try
            {
                teamInfo = context.CricketTeamInfo.Single(x => x.Uuid == teamUuid);
            }
            catch
            {
                throw new CricketTeamNotFoundException($"team with uuid {teamUuid} does not exists.");
            }

            var cricketMatchInfoTableTest = context.TestCricketMatchInfo
                .OrderBy(x => Convert.ToInt32(x.MatchNumber.Replace("Test no. ", string.Empty)));

            var allMatchesByTeamTest = cricketMatchInfoTableTest.Where(x => x.Team1.Team.Name == teamInfo.TeamName || x.Team2.Team.Name == teamInfo.TeamName);

            var recordAgainstTeams = allMatchesByTeamTest
                 .GroupBy(m => m.Team1.Team.Name == teamInfo.TeamName ? m.Team2.Team.Name : m.Team1.Team.Name);

            return recordAgainstTeams.Select(g => new RecordAgainstTeam(
                 g.Key,
                 g.Count(),
                 g.Count(x => x.Result.Contains($"{teamInfo.TeamName} won by")),
                 g.Count(m => (m.Team1.Team.Name == teamInfo.TeamName && m.Result.Contains(m.Team2.Team.Name + " won by"))
                                                 || (m.Team2.Team.Name == teamInfo.TeamName && m.Result.Contains(m.Team1.Team.Name + " won by"))),
                 g.Count(x => x.Result.Contains("Match tied")),
                 g.Count(x => x.Result.Contains("Match drawn")),
                 g.OrderBy(x => Convert.ToInt32(x.MatchNumber.Replace("Test no. ", string.Empty)))
                .GroupBy(x => x.Season)
                .Select(x => new MatchesBySeason(x.Key, x.Select(y => new MatchDetail(y.Uuid, y.Series, y.Result, y.MatchNumber, y.MatchType))))));
        }

        public CricketTeamInfoResponse GetTeamByName(string teamName)
        {
            CricketTeamInfoDTO teamInfo;

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
                null!,
                teamInfo.FlagUrl);
        }

        public CricketTeamInfoResponse GetTeamByUuid(Guid teamUuid)
        {
            Entities.CricketTeamInfoDTO teamInfo;

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

        public CricketTeamInfoResponse GetTeamStatistics(
            CricketTeamInfoDTO teamInfo,
            CricketFormat? format = null,
            int? matchUntil = null,
            CricketTeamInfoDTO? opponentTeamInfo = null)
        {
            var teamRecordData = new CricketTeamInfoResponse(
                teamInfo.Uuid,
                teamInfo.TeamName,
                new TeamRecordDetails(null!, null!, null!),
                teamInfo.FlagUrl);

            var cricketTeam = new CricketTeam(teamInfo.Uuid, teamInfo.TeamName);

            CricketTeam oppCricketTeam = null;

            if (opponentTeamInfo != null)
            {
                oppCricketTeam = new CricketTeam(opponentTeamInfo.Uuid, opponentTeamInfo.TeamName);
            }

            if (format == null || format == CricketFormat.T20I)
            {
                AddTeamStatisticsT20I(cricketTeam, teamRecordData, matchUntil, oppCricketTeam);
            }

            if (format == null || format == CricketFormat.ODI)
            {
                AddTeamStatisticsODI(cricketTeam, teamRecordData, matchUntil, oppCricketTeam);
            }

            if (format == null || format == CricketFormat.TestCricket)
            {
                AddTeamStatisticsTest(cricketTeam, teamRecordData, matchUntil, oppCricketTeam);
            }

            return teamRecordData;
        }

        private void AddTeamStatisticsT20I(
            CricketTeam cricketTeam,
            CricketTeamInfoResponse teamRecordData,
            int? matchUntil = null,
            CricketTeam? opponentTeam = null)
        {
            var allMatchesT20I = context.LimitedOverInternationalMatchesInfo
               .Where(x => x.MatchNumber.Contains("T20I"));

            var allMatchesByTeamT20I = allMatchesT20I
               .OrderBy(x => Convert.ToInt32(x.MatchNumber.Replace("T20I no. ", string.Empty)))
               .Take(matchUntil ?? allMatchesT20I.Count())
               .Where(x => x.Team1.Team.Uuid.Equals(cricketTeam.Uuid) || x.Team2.Team.Uuid.Equals(cricketTeam.Uuid));

            if (opponentTeam != null)
            {
                allMatchesByTeamT20I = allMatchesByTeamT20I
                            .Where(x => x.Team1.Team.Uuid.Equals(opponentTeam.Uuid)
                            || x.Team2.Team.Uuid.Equals(opponentTeam.Uuid));
            }

            if (allMatchesByTeamT20I.Any())
            {
                MatchDetails debutDetails = null!;
                TeamMileStones teamMileStones = null!;

                var debutMatchDetails = allMatchesByTeamT20I.First();

                var debutOpponent = debutMatchDetails.Team1.Team.Uuid.Equals(cricketTeam.Uuid) ? debutMatchDetails.Team2.Team.Name : debutMatchDetails.Team1.Team.Name;

                var cricketMatchInfoResponses = allMatchesByTeamT20I.Select(x => x.ToDomain(mapper));
                teamMileStones = cricketMatchInfoResponses.ToList().GetMileStones(cricketTeam);
                debutDetails = new MatchDetails(
                                  debutMatchDetails.Uuid,
                                  debutMatchDetails.MatchDate,
                                  debutOpponent,
                                  debutMatchDetails.Venue,
                                  debutMatchDetails.Result,
                                  debutMatchDetails.MatchNumber);

                teamRecordData.TeamRecordDetails.T20IResults = new TeamFormatRecordDetails(
                                           debut: debutDetails,
                                           matches: allMatchesByTeamT20I.Count(),
                                           won: allMatchesByTeamT20I.Count(m => m.Result.Contains($"{cricketTeam.Name} won by")),
                                           lost: allMatchesByTeamT20I.Count(m => (m.Team1.Team.Uuid.Equals(cricketTeam.Uuid) && m.Result.Contains(m.Team2.Team.Name + " won by"))
                                                 || (m.Team2.Team.Uuid.Equals(cricketTeam.Uuid) && m.Result.Contains(m.Team1.Team.Name + " won by"))),
                                           tied: allMatchesByTeamT20I.Count(m => m.Result.Contains("Match tied")),
                                           nRorDraw: allMatchesByTeamT20I.Count(m => m.Result.Contains("No result")),
                                           teamMileStones: teamMileStones);
            }
        }

        private void AddTeamStatisticsODI(
            CricketTeam cricketTeam,
            CricketTeamInfoResponse teamRecordData,
            int? matchUntil = null,
            CricketTeam? opponentTeam = null)
        {
            var allMatchesODI = context.LimitedOverInternationalMatchesInfo
                .Where(x => x.MatchNumber.Contains("ODI"));

            var allMatchesByTeamODI = allMatchesODI
                .OrderBy(x => Convert.ToInt32(x.MatchNumber.Replace("ODI no. ", string.Empty)))
                .Take(matchUntil ?? allMatchesODI.Count())
                .Where(x => x.Team1.Team.Uuid.Equals(cricketTeam.Uuid)
                            || x.Team2.Team.Uuid.Equals(cricketTeam.Uuid));

            if (opponentTeam != null)
            {
                allMatchesByTeamODI = allMatchesByTeamODI
                            .Where(x => x.Team1.Team.Uuid.Equals(opponentTeam.Uuid)
                            || x.Team2.Team.Uuid.Equals(opponentTeam.Uuid));
            }

            if (allMatchesByTeamODI.Any())
            {
                MatchDetails debutDetails = null!;
                TeamMileStones teamMileStones = null!;

                var debutMatchDetails = allMatchesByTeamODI.First();

                var debutOpponent = debutMatchDetails.Team1.Team.Uuid.Equals(cricketTeam.Uuid) ? debutMatchDetails.Team2.Team.Name : debutMatchDetails.Team1.Team.Name;

                var cricketMatchInfoResponses = allMatchesByTeamODI.Select(x => x.ToDomain(mapper));

                teamMileStones = cricketMatchInfoResponses.ToList().GetMileStones(cricketTeam);

                debutDetails = new MatchDetails(
                                  debutMatchDetails.Uuid,
                                  debutMatchDetails.MatchDate,
                                  debutOpponent,
                                  debutMatchDetails.Venue,
                                  debutMatchDetails.Result,
                                  debutMatchDetails.MatchNumber);

                teamRecordData.TeamRecordDetails.ODIResults = new TeamFormatRecordDetails(
                                           debut: debutDetails,
                                           matches: allMatchesByTeamODI.Count(),
                                           won: allMatchesByTeamODI.Count(m => m.Result.Contains($"{cricketTeam.Name} won by")),
                                           lost: allMatchesByTeamODI.Count(m => (m.Team1.Team.Uuid.Equals(cricketTeam.Uuid) && m.Result.Contains(m.Team2.Team.Name + " won by"))
                                                 || (m.Team2.Team.Uuid.Equals(cricketTeam.Uuid) && m.Result.Contains(m.Team1.Team.Name + " won by"))),
                                           tied: allMatchesByTeamODI.Count(m => m.Result.Contains("Match tied")),
                                           nRorDraw: allMatchesByTeamODI.Count(m => m.Result.Contains("No result")),
                                           teamMileStones: teamMileStones);
            }
        }

        private void AddTeamStatisticsTest(
            CricketTeam cricketTeam,
            CricketTeamInfoResponse teamRecordData,
            int? matchUntil = null,
            CricketTeam? opponentTeam = null)
        {
            var allMatchesByTeamTest = context.TestCricketMatchInfo
                .OrderBy(x => Convert.ToInt32(x.MatchNumber.Replace("Test no. ", string.Empty)))
                .Take(matchUntil ?? context.TestCricketMatchInfo.Count())
                .Where(x => x.Team1.Team.Uuid.Equals(cricketTeam.Uuid) || x.Team2.Team.Uuid.Equals(cricketTeam.Uuid));

            if (opponentTeam != null)
            {
                allMatchesByTeamTest = allMatchesByTeamTest
                            .Where(x => x.Team1.Team.Uuid.Equals(opponentTeam.Uuid)
                            || x.Team2.Team.Uuid.Equals(opponentTeam.Uuid));
            }

            if (allMatchesByTeamTest.Any())
            {
                MatchDetails debutDetails = null!;
                TeamMileStones teamMileStones = null!;

                var debutMatchDetails = allMatchesByTeamTest.First();

                var debutOpponent = debutMatchDetails.Team1.Team.Uuid.Equals(cricketTeam.Uuid) ? debutMatchDetails.Team2.Team.Name : debutMatchDetails.Team1.Team.Name;

                var cricketMatchInfoResponses = allMatchesByTeamTest.Select(x => x.ToDomain(mapper));

                teamMileStones = cricketMatchInfoResponses.ToList().GetTestMileStones(cricketTeam);

                debutDetails = new MatchDetails(
                                    debutMatchDetails.Uuid,
                                    debutMatchDetails.MatchType,
                                    debutOpponent,
                                    debutMatchDetails.Venue,
                                    debutMatchDetails.Result,
                                    debutMatchDetails.MatchNumber);

                teamRecordData.TeamRecordDetails.TestResults = new TeamFormatRecordDetails(
                                   debut: debutDetails,
                                   matches: allMatchesByTeamTest.Count(),
                                   won: allMatchesByTeamTest.Count(m => m.Result.Contains($"{cricketTeam.Name} won by")),
                                   lost: allMatchesByTeamTest.Count(m => (m.Team1.Team.Uuid.Equals(cricketTeam.Uuid) && m.Result.Contains(m.Team2.Team.Name + " won by"))
                                         || (m.Team2.Team.Uuid.Equals(cricketTeam.Uuid) && m.Result.Contains(m.Team1.Team.Name + " won by"))),
                                   tied: allMatchesByTeamTest.Count(m => m.Result.Contains("Match tied")),
                                   nRorDraw: allMatchesByTeamTest.Count(m => m.Result.Contains("Match drawn")),
                                   teamMileStones: teamMileStones);
            }
        }

        public async Task GeneratedPDFForTeams()
        {
            var allTeamsPlayersInfo = context.CricketTeamPlayerInfos.Include(x => x.TeamInfo).Include(x => x.PlayerInfo);

            await teamPDFHandler.AddPDFCricketTeamRecords(allTeamsPlayersInfo);
        }
    }
}