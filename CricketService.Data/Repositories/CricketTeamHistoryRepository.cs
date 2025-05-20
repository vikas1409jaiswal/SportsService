using AutoMapper;
using CricketService.Data.Contexts;
using CricketService.Data.Entities;
using CricketService.Data.Extensions;
using CricketService.Data.Repositories.Interfaces;
using CricketService.Domain.Enums;
using CricketService.Domain.ResponseDomains;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CricketService.Data.Repositories
{
    public class CricketTeamHistoryRepository : ICricketTeamHistoryRepository
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<CricketTeamHistoryRepository> logger;
        private readonly ICricketTeamRepository cricketTeamRepository;
        private readonly CricketServiceContext context;
        private readonly List<InternationalCricketMatchResponse> t20iResponse;
        private readonly List<InternationalCricketMatchResponse> odiResponse;
        private readonly List<TestCricketMatchResponse> testResponse;

        public CricketTeamHistoryRepository(
            IServiceProvider serviceProvider,
            ILogger<CricketTeamHistoryRepository> logger,
            ICricketTeamRepository cricketTeamRepository,
            CricketServiceContext context,
            IMapper mapper)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
            this.cricketTeamRepository = cricketTeamRepository;
            this.context = context;
            t20iResponse = context.LimitedOverInternationalMatchesInfo
                    .Where(x => x.MatchNumber.Contains("T20I"))
                    .OrderBy(x => Convert.ToInt32(x.MatchNumber.Replace("T20I no. ", string.Empty)))
                    .Select(x => x.ToDomain(mapper)).ToList();

            odiResponse = context.LimitedOverInternationalMatchesInfo
                    .Where(x => x.MatchNumber.Contains("ODI"))
                    .OrderBy(x => Convert.ToInt32(x.MatchNumber.Replace("ODI no. ", string.Empty)))
                    .Select(x => x.ToDomain(mapper)).ToList();

            testResponse = context.TestCricketMatchInfo
                    .OrderBy(x => Convert.ToInt32(x.MatchNumber.Replace("Test no. ", string.Empty)))
                    .Select(x => x.ToDomain(mapper)).ToList();
        }

        public async Task<IEnumerable<CricketTeamHistoryDTO>> GetTeamsHistory(CricketFormat format)
        {
           return await context.CricketTeamsHistory
                .Where(x => x.Format == CricketFormat.ODI.ToString())
                .OrderBy(x => x.MatchNumber)
                .ToListAsync();
        }

        public async Task SeedCricketTeamHistoryTable()
        {
            logger.LogInformation("Starting cricket team history seeding process");
            var startTime = DateTime.Now;

            try
            {
                logger.LogInformation("Fetching all team UUIDs");
                List<Guid> uuids = cricketTeamRepository.GetAllTeamsUuid().ToList();

                if (!uuids.Any())
                {
                    logger.LogWarning("No team UUIDs found - aborting operation");
                    return;
                }

                await ProcessCricketTeamHistoryTable(uuids);

                var duration = DateTime.Now - startTime;
                logger.LogInformation($"Successfully completed seeding process. Total time: {duration.TotalSeconds} seconds");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Critical error during cricket team history seeding");
                throw;
            }
        }

        public async Task ProcessCricketTeamHistoryTable(List<Guid> uuids)
        {
            var counter = 1;

            var cricketTeamHistoriesDto = new List<CricketTeamHistoryDTO>();

            foreach (var odi in odiResponse)
            {
                logger.LogInformation($"Processing match {odi.MatchNumber}");

                var matchNumber = Convert.ToInt32(odi.MatchNumber.Replace("ODI no. ", string.Empty));

                logger.LogDebug($"Checking if match {matchNumber} already exists");
                var existingMatchInfo = await context.CricketTeamsHistory.FindAsync(odi.MatchUuid);

                if (existingMatchInfo is null)
                {
                    List<TeamFormatRecords> tfrDetails = new List<TeamFormatRecords>();

                    foreach (var uuid in uuids)
                    {
                        logger.LogDebug($"Processing team with UUID: {uuid}");

                        try
                        {
                            var team = context.CricketTeamInfo.AsNoTracking().Single(x => x.Uuid == uuid);
                            logger.LogDebug($"Found team: {team.TeamName}");

                            var teamRecords = cricketTeamRepository.GetTeamStatistics(team, CricketFormat.ODI, matchNumber);
                            logger.LogDebug($"Retrieved statistics for {team.TeamName}");

                            tfrDetails.Add(new TeamFormatRecords()
                            {
                                TeamUuid = uuid,
                                TeamName = team.TeamName,
                                TeamFormatRecordDetails = teamRecords.TeamRecordDetails.ODIResults,
                            });
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, $"Error processing team {uuid} for match {odi.MatchNumber}");
                        }
                    }

                    var preparedData = new CricketTeamHistoryDTO()
                    {
                        MatchUuid = odi.MatchUuid,
                        MatchNumber = matchNumber,
                        Format = CricketFormat.ODI.ToString(),
                        InstantTeamsRecords = tfrDetails,
                    };

                    cricketTeamHistoriesDto.Add(preparedData);
                }
                else
                {
                    logger.LogDebug($"Match #{matchNumber} already exists - skipping");
                }

                counter++;

                if (counter % 50 == 0)
                {
                    int startMatchNumber = cricketTeamHistoriesDto.First().MatchNumber;
                    int endMatchNumber = cricketTeamHistoriesDto.Last().MatchNumber;
                    logger.LogInformation($"Creating background job from {startMatchNumber} to {endMatchNumber}");
                    RecurringJob.AddOrUpdate(
                    $"Job for {startMatchNumber} -> {endMatchNumber} ",
                    () => SaveCricketTeamHistoryBatch(cricketTeamHistoriesDto),
                    Cron.Never);

                    cricketTeamHistoriesDto = new List<CricketTeamHistoryDTO>();

                    logger.LogInformation($"Processed {counter} matches so far...");
                }
            }
        }

        public async Task SaveCricketTeamHistoryBatch(IEnumerable<CricketTeamHistoryDTO> histories)
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
                        context.CricketTeamsHistory.Add(history);
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
