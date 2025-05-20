using AutoMapper;
using CricketService.Data.Contexts;
using CricketService.Data.Entities;
using CricketService.Data.Extensions;
using CricketService.Data.Repositories.Interfaces;
using CricketService.Domain;
using CricketService.Domain.Enums;
using CricketService.Domain.ResponseDomains;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CricketService.Data.Repositories
{
    public class CricketTeamHistoryH2HRepository : ICricketTeamHistoryH2HRepository
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<CricketTeamHistoryH2HRepository> logger;
        private readonly ICricketTeamRepository cricketTeamRepository;
        private readonly CricketServiceContext context;
        private readonly Dictionary<CricketFormat, List<InternationalCricketMatchResponse>> loimResponses = new();

        public CricketTeamHistoryH2HRepository(
            IServiceProvider serviceProvider,
            ILogger<CricketTeamHistoryH2HRepository> logger,
            ICricketTeamRepository cricketTeamRepository,
            CricketServiceContext context,
            IMapper mapper)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
            this.cricketTeamRepository = cricketTeamRepository;
            this.context = context;
            var t20iResponse = context.LimitedOverInternationalMatchesInfo
                    .Where(x => x.MatchNumber.Contains("T20I"))
                    .OrderBy(x => Convert.ToInt32(x.MatchNumber.Replace("T20I no. ", string.Empty)))
                    .Select(x => x.ToDomain(mapper)).ToList();

            loimResponses[CricketFormat.T20I] = t20iResponse;

            var odiResponse = context.LimitedOverInternationalMatchesInfo
                    .Where(x => x.MatchNumber.Contains("ODI"))
                    .OrderBy(x => Convert.ToInt32(x.MatchNumber.Replace("ODI no. ", string.Empty)))
                    .Select(x => x.ToDomain(mapper)).ToList();

            loimResponses[CricketFormat.ODI] = odiResponse;

            var testResponse = context.TestCricketMatchInfo
                    .OrderBy(x => Convert.ToInt32(x.MatchNumber.Replace("Test no. ", string.Empty)))
                    .Select(x => x.ToDomain(mapper)).ToList();
        }

        public async Task<IEnumerable<CricketTeamHistoryH2hDTO>> GetTeamsHistoryH2H(
            CricketFormat format, string team1Name, string? team2Name = null)
        {
            if (team2Name == null)
            {
              return await context.CricketTeamsHistoryH2H
                .Where(x => x.Format == format.ToString()
                            && (new string[] { x.Team1Name, x.Team2Name }.Contains(team1Name)
                                || new string[] { x.Team1Name, x.Team2Name }.Contains(team2Name)))
                .OrderByDescending(x => x.MatchNumber)
                .ToListAsync();
            }

            return await context.CricketTeamsHistoryH2H
                   .Where(x => x.Format == format.ToString()
                               && new string[] { x.Team1Name, x.Team2Name }.Contains(team1Name)
                               && new string[] { x.Team1Name, x.Team2Name }.Contains(team2Name))
                   .OrderByDescending(x => x.MatchNumber)
                   .ToListAsync();
        }

        public async Task SeedCricketTeamHistoryH2HTable(
            string team1Name,
            string team2Name,
            CricketFormat format)
        {
            logger.LogInformation("Starting cricket team history seeding process");
            var startTime = DateTime.Now;

            try
            {
                logger.LogInformation("Fetching team UUIDs");
                CricketTeamInfoResponse team1 = cricketTeamRepository.GetTeamByName(team1Name);
                CricketTeamInfoResponse team2 = cricketTeamRepository.GetTeamByName(team2Name);
                List<Guid> uuids = new() { team1.TeamUuid, team2.TeamUuid };

                await ProcessCricketTeamHistoryH2HTable(uuids, format);

                var duration = DateTime.Now - startTime;
                logger.LogInformation($"Successfully completed seeding process. Total time: {duration.TotalSeconds} seconds");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Critical error during cricket team history seeding");
                throw;
            }
        }

        public async Task ProcessCricketTeamHistoryH2HTable(List<Guid> uuids, CricketFormat format)
        {
            var counter = 1;

            var cricketTeamHistoriesH2hDto = new List<CricketTeamHistoryH2hDTO>();

            var teamsLoimResponses = loimResponses[format]
                .Where(x => (x.Team1.Team.Uuid.Equals(uuids.ElementAt(0))
                            && x.Team2.Team.Uuid.Equals(uuids.ElementAt(1)))
                            || (x.Team2.Team.Uuid.Equals(uuids.ElementAt(0))
                            && x.Team1.Team.Uuid.Equals(uuids.ElementAt(1))));

            foreach (var loim in teamsLoimResponses)
            {
                logger.LogInformation($"Processing match {loim.MatchNumber} for H2H between {loim.Team1.Team.Name} & {loim.Team2.Team.Name}");

                var matchNumber = Convert.ToInt32(loim.MatchNumber
                    .Replace(format == CricketFormat.ODI ? "ODI no. " : "T20I no. ", string.Empty));

                logger.LogDebug($"Checking if match {matchNumber} already exists");
                var existingMatchInfo = await context.CricketTeamsHistoryH2H.FindAsync(loim.MatchUuid);

                if (existingMatchInfo is null)
                {
                    List<TeamFormatRecords> tfrDetails = new();

                    foreach (var uuid in uuids)
                    {
                        logger.LogDebug($"Processing team with UUID: {uuid}");

                        try
                        {
                            var teams = context.CricketTeamInfo
                                       .AsNoTracking()
                                       .Where(x => uuids.Contains(x.Uuid))
                                       .ToList();

                            var team = teams.Single(x => x.Uuid == uuid);
                            var oppTeam = teams.First(x => x.Uuid != uuid);

                            var teamRecords = cricketTeamRepository.GetTeamStatistics(
                                team: team, format: format, matchUntil: matchNumber, opponentTeam: oppTeam);

                            logger.LogDebug($"Retrieved statistics for {team.TeamName}");

                            tfrDetails.Add(new TeamFormatRecords()
                            {
                                TeamUuid = uuid,
                                TeamName = team.TeamName,
                                TeamFormatRecordDetails = teamRecords.TeamRecordDetails.T20IResults,
                            });
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, $"Error processing team {uuid} for match {loim.MatchNumber}");
                        }
                    }

                    var preparedData = new CricketTeamHistoryH2hDTO()
                    {
                        MatchUuid = loim.MatchUuid,
                        Team1Name = tfrDetails.ElementAt(0).TeamName,
                        Team2Name = tfrDetails.ElementAt(1).TeamName,
                        MatchNumber = matchNumber,
                        Format = format.ToString(),
                        InstantTeamsRecords = tfrDetails,
                    };

                    cricketTeamHistoriesH2hDto.Add(preparedData);
                }
                else
                {
                    logger.LogDebug($"Match #{matchNumber} already exists - skipping");
                }

                counter++;

                if (counter % 1 == 0)
                {
                    int startMatchNumber = cricketTeamHistoriesH2hDto.First().MatchNumber;
                    int endMatchNumber = cricketTeamHistoriesH2hDto.Last().MatchNumber;
                    string firstTeamName = cricketTeamHistoriesH2hDto.First().Team1Name;
                    string secondTeamName = cricketTeamHistoriesH2hDto.First().Team2Name;
                    logger.LogInformation($"Creating background job from {startMatchNumber} to {endMatchNumber} btw {firstTeamName} & {secondTeamName}");
                    RecurringJob.AddOrUpdate(
                    $"{firstTeamName}vs{secondTeamName}::{startMatchNumber}->{endMatchNumber}::{format}",
                    () => SaveCricketTeamHistoryH2HBatch(cricketTeamHistoriesH2hDto),
                    Cron.Never);

                    cricketTeamHistoriesH2hDto = new List<CricketTeamHistoryH2hDTO>();

                    logger.LogInformation($"Processed {counter} matches so far...");
                }
            }
        }

        public async Task SaveCricketTeamHistoryH2HBatch(IEnumerable<CricketTeamHistoryH2hDTO> histories)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<CricketServiceContext>();

                var matchUuids = histories.Select(h => h.MatchUuid).ToList();

                var existingRecords = await context.CricketTeamsHistory
                    .Where(x => matchUuids.Contains(x.MatchUuid))
                    .ToDictionaryAsync(x => x.MatchUuid);

                foreach (var history in histories)
                {
                    if (existingRecords.TryGetValue(history.MatchUuid, out var existingRecord))
                    {
                        context.Entry(existingRecord).CurrentValues.SetValues(history);
                        context.Entry(existingRecord).State = EntityState.Modified;
                    }
                    else
                    {
                        context.CricketTeamsHistoryH2H.Add(history);
                    }
                }

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
