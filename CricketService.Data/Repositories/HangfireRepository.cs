using AutoMapper;
using CricketService.Data.Contexts;
using CricketService.Data.Entities;
using CricketService.Data.Extensions;
using CricketService.Data.Repositories.Extensions;
using CricketService.Data.Repositories.Interfaces;
using CricketService.Domain;
using CricketService.Domain.BaseDomains;
using CricketService.Domain.Enums;
using CricketService.Domain.ResponseDomains;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CricketService.Data.Repositories
{
    public class HangfireRepository : IHangfireRepository
    {
        private readonly ILogger<HangfireRepository> logger;
        private readonly ICricketTeamRepository cricketTeamRepository;
        private readonly CricketServiceContext context;
        private readonly List<InternationalCricketMatchResponse> t20iResponse;
        private readonly List<InternationalCricketMatchResponse> odiResponse;
        private readonly List<TestCricketMatchResponse> testResponse;

        public HangfireRepository(
            ILogger<HangfireRepository> logger,
            ICricketTeamRepository cricketTeamRepository,
            CricketServiceContext context,
            IMapper mapper)
        {
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

        #region Team Related Jobs

        public async Task UpdateTeamRecords(List<Guid>? teamUuids = null)
        {
            logger.LogInformation($"Started updating team records.");

            var counter = 1;

            List<Guid> uuids;

            if (teamUuids is null)
            {
                 uuids = cricketTeamRepository.GetAllTeamsUuid().ToList();
            }
            else
            {
                uuids = teamUuids;
            }

            var startTime = DateTime.Now;

            foreach (var uuid in uuids)
            {
                var team = context.CricketTeamInfo.Single(x => x.Uuid == uuid);

                logger.LogInformation($"updating team statistics for {team.TeamName}");

                var teamRecords = cricketTeamRepository.GetTeamStatistics(team);

                if (teamRecords.TeamRecordDetails.T20IResults is not null)
                {
                    team.T20IRecords = teamRecords.TeamRecordDetails.T20IResults;
                }

                if (teamRecords.TeamRecordDetails.ODIResults is not null)
                {
                    team.ODIRecords = teamRecords.TeamRecordDetails.ODIResults;
                }

                if (teamRecords.TeamRecordDetails.TestResults is not null)
                {
                    team.TestRecords = teamRecords.TeamRecordDetails.TestResults;
                }

                context.CricketTeamInfo.Update(team);

                await context.SaveChangesAsync();
                counter++;

                var stepTime = DateTime.Now;

                logger.LogInformation($"Running watch: {stepTime - startTime}");
            }

            var endTime = DateTime.Now;

            logger.LogInformation($"Total seeding time is {endTime - startTime}");

            logger.LogInformation($"Completed updating team records.");
        }
        #endregion

        #region Players Related Jobs
        public IEnumerable<Guid> GetAllPlayersUuid()
        {
            return context.CricketPlayerInfo.Select(x => x.Uuid);
        }

        public async Task UpdatePlayersCareerStatistics()
        {
            var counter = 1;

            List<Guid> result = GetAllPlayersUuid().Where(x => x == new Guid("30d4ba88-3351-47dc-8f6b-bd9705d0d493")).ToList();

            var startTime = DateTime.Now;

            foreach (var uuid in result)
            {
                logger.LogInformation($"updating career statistics for player no {counter} with uuid {uuid}");

                var player = context.CricketPlayerInfo.Include(p => p.TeamsPlayersInfos).Single(x => x.Uuid == uuid);

                foreach (var teamPlayerInfos in player.TeamsPlayersInfos)
                {
                    CricketTeam cricketTeam = new CricketTeam(teamPlayerInfos.TeamUuid, teamPlayerInfos.TeamName);
                    CricketPlayer cricketPlayer = new CricketPlayer(teamPlayerInfos.PlayerName, player.Href);

                    teamPlayerInfos.CareerStatistics = new CareerDetailsInfo(
                    teamPlayerInfos.TeamName,
                    testResponse.GetTestPlayerStatistics(cricketTeam, cricketPlayer, true),
                    odiResponse.GetPlayerStatistics(cricketTeam, cricketPlayer, true),
                    t20iResponse.GetPlayerStatistics(cricketTeam, cricketPlayer, true));
                }

                context.CricketPlayerInfo.Update(player);

                await context.SaveChangesAsync();
                counter++;

                var stepTime = DateTime.Now;

                logger.LogInformation($"Running watch: {stepTime - startTime}");
            }

            var endTime = DateTime.Now;

            logger.LogInformation($"Total seeding time is {endTime - startTime}");
        }
        #endregion

        public void CleanDatabase()
        {
            context.ChangeTracker
                .Entries().ToList().ForEach(e => e.State = EntityState.Detached);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}
