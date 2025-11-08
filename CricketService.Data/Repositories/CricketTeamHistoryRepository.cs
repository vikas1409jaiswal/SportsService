using AutoMapper;
using CricketService.Data.Contexts;
using CricketService.Data.Entities;
using CricketService.Data.Extensions;
using CricketService.Data.Repositories.Interfaces;
using CricketService.Domain;
using CricketService.Domain.BaseDomains;
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
        private readonly ICricketMatchRepository cricketMatchRepository;
        private readonly CricketServiceContext context;
        private readonly Dictionary<CricketFormat, List<InternationalCricketMatchResponse>> loimResponses = new();
        private readonly List<TestCricketMatchResponse> testCricketMatchResponses = new();

        public CricketTeamHistoryRepository(
            IServiceProvider serviceProvider,
            ILogger<CricketTeamHistoryRepository> logger,
            ICricketTeamRepository cricketTeamRepository,
            ICricketMatchRepository cricketMatchRepository,
            CricketServiceContext context,
            IMapper mapper)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
            this.cricketTeamRepository = cricketTeamRepository;
            this.cricketMatchRepository = cricketMatchRepository;
            this.context = context;
            var t20iResponse = context.LimitedOverInternationalMatchesInfo
                    .Where(x => x.MatchNumber.Contains("T20I"))
                    .OrderBy(x => Convert.ToInt32(x.MatchNumber
                                                   .Replace("WT20I no. ", string.Empty)
                                                   .Replace("T20I no. ", string.Empty)))
                    .Select(x => x.ToDomain(mapper)).ToList();

            loimResponses[CricketFormat.T20I] = t20iResponse;

            var odiResponse = context.LimitedOverInternationalMatchesInfo
                    .Where(x => x.MatchNumber.Contains("ODI"))
                    .OrderBy(x => Convert.ToInt32(x.MatchNumber
                                                   .Replace("WODI no. ", string.Empty)
                                                   .Replace("ODI no. ", string.Empty)))
                    .Select(x => x.ToDomain(mapper)).ToList();

            loimResponses[CricketFormat.ODI] = odiResponse;

            testCricketMatchResponses = context.TestCricketMatchInfo
                    .OrderBy(x => Convert.ToInt32(x.MatchNumber.Replace("Test no. ", string.Empty)))
                    .Select(x => x.ToDomain(mapper)).ToList();
        }

        public async Task<IEnumerable<CricketTeamHistoryDTO>> GetTeamsHistory(CricketFormat format)
        {
           return await context.CricketTeamsHistory
                //.Where(x => x.Format == format.ToString())
                //.OrderBy(x => x.MatchNumber)
                .ToListAsync();
        }

        public async Task SeedCricketTeamHistoryTable(CricketFormat format, bool finalRecordOnly = false)
        {
            logger.LogInformation("Starting cricket team history seeding process");
            var startTime = DateTime.Now;

            try
            {
                logger.LogInformation("Fetching all team UUIDs");
                List<Guid> uuids = cricketTeamRepository.GetAllTeamsUuidByFormat(format).ToList();

                if (!uuids.Any())
                {
                    logger.LogWarning("No team UUIDs found - aborting operation");
                    return;
                }

                if (format == CricketFormat.TestCricket)
                {
                    await ProcessTestCricketTeamHistoryTable(uuids, finalRecordOnly);
                }
                else
                {
                    await ProcessLOICricketTeamHistoryTable(uuids, format, finalRecordOnly);
                }

                var duration = DateTime.Now - startTime;
                logger.LogInformation($"Successfully completed seeding process. Total time: {duration.TotalSeconds} seconds");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Critical error during cricket team history seeding");
                throw;
            }
        }

        public async Task ProcessLOICricketTeamHistoryTable(List<Guid> uuids, CricketFormat format, bool finalRecordOnly = false)
        {
            var counter = 1;

            var cricketTeamHistoriesDto = new List<CricketTeamHistoryDTO>();

            var teamsLoimResponses = loimResponses[format];

            if (finalRecordOnly)
            {
                teamsLoimResponses = new List<InternationalCricketMatchResponse>()
                {
                    teamsLoimResponses.Last(),
                };
            }

            foreach (var loim in teamsLoimResponses.Skip(3835))
            {
                logger.LogInformation($"Processing match {loim.MatchNumber}");

                var matchNumber = Convert.ToInt32(loim.MatchNumber.Replace($"{format} no. ", string.Empty));

                logger.LogDebug($"Checking if match {matchNumber} already exists");
                var existingMatchInfo = await context.CricketTeamsHistory.FindAsync(loim.MatchUuid);

                if (existingMatchInfo is null)
                {
                    List<TeamFormatRecords> tfrDetails = new List<TeamFormatRecords>();

                    foreach (var uuid in uuids)
                    {
                        logger.LogDebug($"Processing team with UUID: {uuid}");

                        var team = context.CricketTeamInfo.AsNoTracking().Single(x => x.Uuid == uuid);
                        logger.LogDebug($"Found team: {team.TeamName}");

                        var teamRecords = cricketTeamRepository.GetTeamStatistics(team: team, format: format, matchUntil: matchNumber);
                        logger.LogDebug($"Retrieved statistics for {team.TeamName}");

                        tfrDetails.Add(new TeamFormatRecords()
                        {
                            TeamUuid = uuid,
                            TeamName = team.TeamName,
                            TeamFormatRecordDetails = format == CricketFormat.ODI
                                                      ? teamRecords.TeamRecordDetails.ODIResults
                                                      : teamRecords.TeamRecordDetails.T20IResults,
                        });
                    }

                    var preparedData = new CricketTeamHistoryDTO()
                    {
                        MatchUuid = loim.MatchUuid,
                        MatchNumber = matchNumber,
                        Format = format.ToString(),
                        InstantTeamsRecords = tfrDetails,
                    };

                    cricketTeamHistoriesDto.Add(preparedData);
                }
                else
                {
                    logger.LogDebug($"Match #{matchNumber} already exists - skipping");
                }

                counter++;

                if (finalRecordOnly ? true : counter % 50 == 0)
                {
                    int startMatchNumber = cricketTeamHistoriesDto.First().MatchNumber;
                    int endMatchNumber = cricketTeamHistoriesDto.Last().MatchNumber;
                    logger.LogInformation($"Creating background job from {startMatchNumber} to {endMatchNumber}");
                    RecurringJob.AddOrUpdate(
                    $"Job for {startMatchNumber}->{endMatchNumber}::{format}",
                    () => SaveCricketTeamHistoryBatch(cricketTeamHistoriesDto),
                    Cron.Never);

                    cricketTeamHistoriesDto = new List<CricketTeamHistoryDTO>();

                    logger.LogInformation($"Processed {counter} matches so far...");
                }
            }
        }

        public async Task ProcessTestCricketTeamHistoryTable(List<Guid> uuids, bool finalRecordOnly = false)
        {
            var counter = 1;

            var cricketTeamHistoriesDto = new List<CricketTeamHistoryDTO>();

            var teamsTcmResponses = testCricketMatchResponses;

            if (finalRecordOnly)
            {
                teamsTcmResponses = new List<TestCricketMatchResponse>()
                {
                    teamsTcmResponses.Last(),
                };
            }

            foreach (var tcm in teamsTcmResponses)
            {
                logger.LogInformation($"Processing match {tcm.MatchNumber}");

                var matchNumber = Convert.ToInt32(tcm.MatchNumber.Replace($"Test no. ", string.Empty));

                logger.LogDebug($"Checking if match {matchNumber} already exists");
                var existingMatchInfo = await context.CricketTeamsHistory.FindAsync(tcm.MatchUuid);

                if (existingMatchInfo is null)
                {
                    List<TeamFormatRecords> tfrDetails = new();

                    foreach (var uuid in uuids)
                    {
                        logger.LogDebug($"Processing team with UUID: {uuid}");

                        var team = context.CricketTeamInfo.AsNoTracking().Single(x => x.Uuid == uuid);
                        logger.LogDebug($"Found team: {team.TeamName}");

                        var teamRecords = cricketTeamRepository.GetTeamStatistics(team: team, format: CricketFormat.TestCricket, matchUntil: matchNumber);
                        logger.LogDebug($"Retrieved statistics for {team.TeamName}");

                        tfrDetails.Add(new TeamFormatRecords()
                        {
                            TeamUuid = uuid,
                            TeamName = team.TeamName,
                            TeamFormatRecordDetails = teamRecords.TeamRecordDetails.TestResults,
                        });
                    }

                    var preparedData = new CricketTeamHistoryDTO()
                    {
                        MatchUuid = tcm.MatchUuid,
                        MatchNumber = matchNumber,
                        Format = CricketFormat.TestCricket.ToString(),
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
                    $"Job for {startMatchNumber}->{endMatchNumber}::{CricketFormat.TestCricket}",
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

        public async Task<IEnumerable<object>> GetTeamsPlayers(CricketFormat format, PlayersCategory playersCategory, string teamName)
        {
            var lastMatchData = await context.CricketTeamsHistory
                 .Where(x => x.Format.Equals(format.ToString()))
                 .OrderByDescending(x => x.MatchNumber)
                 .FirstOrDefaultAsync();

            if (lastMatchData == null)
            {
                throw new KeyNotFoundException($"No match data found for format {format}");
            }

            var teamRecord = lastMatchData.InstantTeamsRecords
                .FirstOrDefault(x => x.TeamName.Equals(teamName, StringComparison.OrdinalIgnoreCase));

            if (teamRecord == null)
            {
                throw new KeyNotFoundException($"Team '{teamName}' not found in the latest match data");
            }

            var playersRepresented = new List<KeyValuePair<CricketPlayer, PlayerRepresentedDetails>>();

            switch (playersCategory)
            {
                case PlayersCategory.All:
                    playersRepresented = teamRecord.TeamFormatRecordDetails.TeamMileStones.PlayersRepresented;
                    break;
                case PlayersCategory.Captains:
                    playersRepresented = teamRecord.TeamFormatRecordDetails.TeamMileStones.CaptainsRepresented;
                    break;
                case PlayersCategory.WicketKeepers:
                    playersRepresented = teamRecord.TeamFormatRecordDetails.TeamMileStones.WicketKeepersRepresented;
                    break;
            }

            return playersRepresented.Select(x => new
            {
                x.Key.Name,
                x.Key.Href,
                x.Value.FirstMatchDate,
                x.Value.FirstMatchUuid,
            });
        }
    }
}
