using System.Data.Common;
using AutoMapper;
using CricketService.Data.Contexts;
using CricketService.Data.Entities;
using CricketService.Data.Extensions;
using CricketService.Data.Repositories.Extensions;
using CricketService.Data.Repositories.Interfaces;
using CricketService.Data.Utils;
using CricketService.Domain;
using CricketService.Domain.BaseDomains;
using CricketService.Domain.Common;
using CricketService.Domain.Enums;
using CricketService.Domain.Exceptions;
using CricketService.Domain.RequestDomains;
using CricketService.Domain.ResponseDomains;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CricketService.Data.Repositories
{
    public class CricketMatchRepository : ICricketMatchRepository
    {
        private readonly ILogger<CricketMatchRepository> logger;
        private readonly IMapper mapper;
        private readonly CricketServiceContext context;
        private readonly ICricketPlayerRepository cricketPlayerRepository;
        private readonly MatchPDFHandler matchPdfHandler;

        public CricketMatchRepository(
            ILogger<CricketMatchRepository> logger,
            IMapper mapper,
            CricketServiceContext context,
            ICricketPlayerRepository cricketPlayerRepository,
            MatchPDFHandler matchPdfHandler)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.context = context;
            this.cricketPlayerRepository = cricketPlayerRepository;
            this.matchPdfHandler = matchPdfHandler;
        }

        public IEnumerable<InternationalCricketMatchResponse> GetAllLimitedOverInternationalMatches(MatchesFilters matchesFilters)
        {
            var format = matchesFilters.Format;

            logger.LogInformation($"Fetching details for all {format} matches.");

            IEnumerable<LimitedOverInternationalMatchInfoDTO> matchesList = new List<LimitedOverInternationalMatchInfoDTO>();

            matchesList = context.LimitedOverInternationalMatchesInfo.AsEnumerable()
                .Where(x => x.MatchNumber.Contains(format.ToString()))
                .OrderBy(x => Convert.ToInt32(x.MatchNumber.Replace($"{format} no. ", string.Empty)));

            if (!matchesFilters.TeamUuid.Equals(Guid.Empty))
            {
                if (matchesFilters.OppositionTeamUuid.Equals(Guid.Empty))
                {
                    matchesList = matchesList.Where(x => x.Team1.Team.Uuid.Equals(matchesFilters.TeamUuid) || x.Team2.Team.Uuid.Equals(matchesFilters.TeamUuid));
                }
                else if (!matchesFilters.OppositionTeamUuid.Equals(Guid.Empty))
                {
                    matchesList = matchesList.Where(x =>
                    (x.Team1.Team.Uuid.Equals(matchesFilters.TeamUuid) && x.Team2.Team.Uuid.Equals(matchesFilters.OppositionTeamUuid))
                    || (x.Team2.Team.Uuid.Equals(matchesFilters.TeamUuid) && x.Team1.Team.Uuid.Equals(matchesFilters.OppositionTeamUuid)));
                }
            }

            logger.LogInformation($"{matchesList.Count()} {format} matches found.");

            return matchesList.Select(x => x.ToDomain(mapper));
        }

        public IEnumerable<TestCricketMatchResponse> GetAllMatchesTest(MatchesFilters matchesFilters)
        {
            logger.LogInformation($"Fetching details for all Test matches.");

            var matchesList = context.TestCricketMatchInfo.AsEnumerable()
                .OrderBy(x => Convert.ToInt32(x.MatchNumber.Replace("Test no. ", string.Empty)));

            logger.LogInformation($"{matchesList.Count()} matches found.");

            return matchesList.Select(x => x.ToDomain(mapper));
        }

        public IEnumerable<DomesticCricketMatchResponse> GetAllTwenty20Matches(MatchesFilters matchesFilters)
        {
            logger.LogInformation($"Fetching details for all Twenty 20 matches.");

            var matchesList = context.T20MatchesInfo.AsEnumerable();

            logger.LogInformation($"{matchesList.Count()} matches found.");

            return matchesList.Select(x => x.ToDomain(mapper));
        }

        public async Task<InternationalCricketMatchResponse> GetLimitedOverInternationalMatchByNumber(int matchNumber, CricketFormat format)
        {
            logger.LogInformation($"Fetching details for {format} no. {matchNumber}");

            Entities.LimitedOverInternationalMatchInfoDTO match = null!;

            try
            {
                match = await context.LimitedOverInternationalMatchesInfo.SingleAsync(x => x.MatchNumber == $"{format} no. {matchNumber}");
            }
            catch (JsonReaderException)
            {
                match = new Entities.LimitedOverInternationalMatchInfoDTO();
            }
            catch (JsonSerializationException)
            {
                match = new Entities.LimitedOverInternationalMatchInfoDTO();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return match.ToDomain(mapper);
        }

        public async Task<TestCricketMatchResponse> GetMatchByMNumberTest(int matchNumber)
        {
            logger.LogInformation($"Fetching details for match number {matchNumber}");

            Entities.TestCricketMatchInfoDTO match = null!;

            try
            {
                match = await context.TestCricketMatchInfo.SingleAsync(x => x.MatchNumber == $"Test no. {matchNumber}");
            }
            catch (JsonReaderException)
            {
                match = new Entities.TestCricketMatchInfoDTO();
            }
            catch (JsonSerializationException)
            {
                match = new Entities.TestCricketMatchInfoDTO();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return match.ToDomain(mapper);
        }

        private IEnumerable<InternationalCricketMatchResponse> GetMatchByTeamT20I(string teamName)
        {
            logger.LogInformation($"Fetching details for team {teamName}");

            var matchesList = context.LimitedOverInternationalMatchesInfo.Where(x => ((x.Team1.Team.Name == teamName) || (x.Team2.Team.Name == teamName)));

            logger.LogInformation($"{matchesList.Count()} matches found.");

            return matchesList.Select(x => x.ToDomain(mapper));
        }

        private IEnumerable<InternationalCricketMatchResponse> GetMatchByTeamODI(string teamName)
        {
            logger.LogInformation($"Fetching details for team {teamName}");

            var matchesList = context.LimitedOverInternationalMatchesInfo.Where(x => ((x.Team1.Team.Name == teamName) || (x.Team2.Team.Name == teamName)));

            logger.LogInformation($"{matchesList.Count()} matches found.");

            return matchesList.Select(x => x.ToDomain(mapper));
        }

        private IEnumerable<TestCricketMatchResponse> GetMatchByTeamTest(string teamName)
        {
            logger.LogInformation($"Fetching details for team {teamName}");

            var matchesList = context.TestCricketMatchInfo.Where(x => ((x.Team1.Team.Name == teamName) || (x.Team2.Team.Name == teamName)));

            logger.LogInformation($"{matchesList.Count()} matches found.");

            return matchesList.Select(x => x.ToDomain(mapper));
        }

        public async Task<InternationalCricketMatchResponse> AddLimitedOverInternationalMatch(InternationalCricketMatchRequest match, CricketFormat format)
        {
            if (context.LimitedOverInternationalMatchesInfo.Any(x => x.Uuid.Equals(match.MatchUuid)))
            {
                return null!;
            }
            else
            {
                var matchInfo = mapper.Map<LimitedOverInternationalMatchInfoDTO>(match);

                var response = matchInfo.ToDomain(mapper);

                var team1Info = await response.Team1.Team.SaveTeamInfo(context, format);
                var team2Info = await response.Team2.Team.SaveTeamInfo(context, format);

                matchInfo.Team1.Team = new CricketTeam(team1Info.Uuid, team1Info.TeamName, matchInfo.Team1.Team.LogoUrl);
                matchInfo.Team2.Team = new CricketTeam(team2Info.Uuid, team2Info.TeamName, matchInfo.Team2.Team.LogoUrl);

                context.LimitedOverInternationalMatchesInfo.Add(matchInfo);

                await context.SaveChangesAsync();

                return matchInfo.ToDomain(mapper);
            }
        }

        public async Task<TestCricketMatchResponse> AddMatchTest(TestCricketMatchRequest match)
        {
            var matchInfo = mapper.Map<TestCricketMatchInfoDTO>(match);

            var response = matchInfo.ToDomain(mapper);

            response.Team1.Team = new CricketTeam(Guid.Empty, match.Team1.Team.Name);
            response.Team2.Team = new CricketTeam(Guid.Empty, match.Team2.Team.Name);

            var team1Info = await response.Team1.Team.SaveTeamInfo(context, CricketFormat.TestCricket);
            var team2Info = await response.Team2.Team.SaveTeamInfo(context, CricketFormat.TestCricket);

            matchInfo.Team1.Team = new CricketTeam(team1Info.Uuid, team1Info.TeamName);
            matchInfo.Team2.Team = new CricketTeam(team2Info.Uuid, team2Info.TeamName);

            context.TestCricketMatchInfo.Add(matchInfo);

            await context.SaveChangesAsync();

            if (response.Team1.Inning1.Playing11 is not null)
            {
                await response.Team1.Team.SavePlayersForTeam(response.Team1.Inning1.Playing11, context, CricketFormat.TestCricket);
            }

            if (response.Team2.Inning1.Playing11 is not null)
            {
                await response.Team2.Team.SavePlayersForTeam(response.Team2.Inning1.Playing11, context, CricketFormat.TestCricket);
            }

            return response;
        }

        public IEnumerable<Guid> GetAllPlayersUuid()
        {
            return context.CricketPlayerInfo.Select(x => x.Uuid);
        }

        public async Task UpdatePlayersCareerStatistics()
        {
            var counter = 1;

            var t20iResponse = context.LimitedOverInternationalMatchesInfo
                    .OrderBy(x => Convert.ToInt32(x.MatchNumber.Replace("T20I no. ", string.Empty)))
                    .Select(x => mapper.Map<InternationalCricketMatchResponse>(x));

            var odiResponse = context.LimitedOverInternationalMatchesInfo
                    .OrderBy(x => Convert.ToInt32(x.MatchNumber.Replace("ODI no. ", string.Empty)))
                    .Select(x => mapper.Map<InternationalCricketMatchResponse>(x));

            var testResponse = context.TestCricketMatchInfo
                    .OrderBy(x => Convert.ToInt32(x.MatchNumber.Replace("Test no. ", string.Empty)))
                    .Select(x => mapper.Map<TestCricketMatchResponse>(x));

            List<Guid> result = GetAllPlayersUuid().Where(x => x == new Guid("7c5181e5-08d2-4c8e-844b-6dd57cef00eb")).ToList();

            var startTime = DateTime.Now;

            foreach (var uuid in result)
            {
                Console.WriteLine($"updating career statistics for player no {counter} with uuid {uuid}");

                var player = context.CricketPlayerInfo.Include(p => p.TeamsPlayersInfos).Single(x => x.Uuid == uuid);

                foreach (var teamPlayerInfos in player.TeamsPlayersInfos)
                {
                    CricketTeam cricketTeam = new CricketTeam(teamPlayerInfos.TeamUuid, teamPlayerInfos.TeamName);
                    CricketPlayer cricketPlayer = new CricketPlayer(teamPlayerInfos.PlayerName, player.Href);

                    teamPlayerInfos.CareerStatistics = new CareerDetailsInfo(
                    teamPlayerInfos.TeamName,
                    testResponse.ToList().GetTestPlayerStatistics(cricketTeam, cricketPlayer, true),
                    odiResponse.ToList().GetPlayerStatistics(cricketTeam, cricketPlayer, true),
                    t20iResponse.ToList().GetPlayerStatistics(cricketTeam, cricketPlayer, true));
                }

                context.CricketPlayerInfo.Update(player);

                await context.SaveChangesAsync();
                counter++;

                var stepTime = DateTime.Now;

                Console.WriteLine($"Running watch: {stepTime - startTime}");
            }

            var endTime = DateTime.Now;

            Console.WriteLine($"Total seeding time is {endTime - startTime}");
        }

        public IEnumerable<object> GetMatchesByTeamUuid(Guid teamUuid, CricketFormat format)
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

            IEnumerable<object> matches = new List<object>();

            if (format == CricketFormat.T20I)
            {
                matches = GetMatchByTeamT20I(teamInfo.TeamName);
            }
            else if (format == CricketFormat.ODI)
            {
                matches = GetMatchByTeamODI(teamInfo.TeamName);
            }
            else if (format == CricketFormat.TestCricket)
            {
                matches = GetMatchByTeamTest(teamInfo.TeamName);
            }

            return matches;
        }

        public IEnumerable<object> GetMatchesByTournament(CricketTournament tournament, CricketFormat format)
        {
            IEnumerable<object> matches = new List<object>();

            if (format == CricketFormat.T20I && tournament == CricketTournament.ICCMensT20IWorldCup)
            {
                matches = context.LimitedOverInternationalMatchesInfo
                    .Where(x => x.Series == "ICC Men's T20 World Cup" || x.Series == "ICC World Twenty20")
                    .GroupBy(x => x.Season)
                    .Select(x => new
                    {
                        Year = x.Key,
                        Matches = x.Select(y => new
                        {
                            MatchUuid = y.Uuid,
                            y.MatchNumber,
                            Title = y.MatchTitle,
                            Date = y.MatchDate,
                            y.Venue,
                            y.Result,
                        }),
                    });
            }
            else if (format == CricketFormat.ODI && tournament == CricketTournament.ICCMensODIWorldCup)
            {
                matches = context.LimitedOverInternationalMatchesInfo
                    .Where(x => x.Series == "Prudential World Cup"
                    || x.Series == "Reliance World Cup"
                    || x.Series == "Benson & Hedges World Cup"
                    || x.Series == "Wills World Cup"
                    || x.Series == "ICC World Cup"
                    || x.Series == "ICC Cricket World Cup")
                    .GroupBy(x => x.Season)
                    .Select(x => new
                    {
                        Year = x.Key,
                        Matches = x.Select(y => new
                        {
                            MatchUuid = y.Uuid,
                            y.MatchNumber,
                            Title = y.MatchTitle,
                            Date = y.MatchDate,
                            y.Venue,
                            y.Result,
                        }),
                    });
            }

            return matches;
        }

        public async Task<DomesticCricketMatchResponse> GetT20IMatchesByTitleAndDate(string title, string date)
        {
            logger.LogInformation($"Fetching details for {title} on {date}");

            T20MatchInfoDTO match = null!;

            try
            {
                match = await context.T20MatchesInfo.SingleAsync(x => x.MatchDate.Equals(date) && x.MatchTitle.Equals(title));
            }
            catch (JsonReaderException)
            {
                match = new T20MatchInfoDTO();
            }
            catch (JsonSerializationException)
            {
                match = new T20MatchInfoDTO();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return match.ToDomain(mapper);
        }

        public async Task<DomesticCricketMatchResponse> AddT20Match(DomesticCricketMatchRequest match)
        {
            var matchInfo = mapper.Map<T20MatchInfoDTO>(match);

            var response = matchInfo.ToDomain(mapper);

            await response.Team1.Team.SaveTeamInfo(context, CricketFormat.Twenty20);
            await response.Team2.Team.SaveTeamInfo(context, CricketFormat.Twenty20);

            context.T20MatchesInfo.Add(matchInfo);

            await context.SaveChangesAsync();

            return response;
        }

        public async Task GeneratedPDFForMatches(IEnumerable<Guid> matchUuids, CricketFormat format)
        {
            if (format == CricketFormat.TestCricket)
            {
               var matches = context.TestCricketMatchInfo.Where(x => matchUuids.Contains(x.Uuid));
               await matchPdfHandler.AddPDFTestCricketMatch(matches);
            }
            else if (format == CricketFormat.Twenty20)
            {
                var matches = context.T20MatchesInfo.Where(x => matchUuids.Contains(x.Uuid));
                await matchPdfHandler.AddPDFT20CricketMatch(matches);
            }
            else
            {
                var matches = context.LimitedOverInternationalMatchesInfo.Where(x => matchUuids.Contains(x.Uuid));
                await matchPdfHandler.AddPDFLimitedOverCricketMatch(matches, format);
            }
        }
    }
}
