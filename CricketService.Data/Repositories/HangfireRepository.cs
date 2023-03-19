using AutoMapper;
using CricketService.Data.Contexts;
using CricketService.Data.Extensions;
using CricketService.Data.Repositories.Extensions;
using CricketService.Data.Repositories.Interfaces;
using CricketService.Domain;
using Microsoft.Extensions.Logging;

namespace CricketService.Data.Repositories
{
    public class HangfireRepository : IHangfireRepository
    {
        private readonly ILogger<HangfireRepository> logger;
        private readonly ICricketTeamRepository cricketTeamRepository;
        private readonly CricketServiceContext context;
        private readonly IMapper mapper;

        public HangfireRepository(
            ILogger<HangfireRepository> logger,
            ICricketTeamRepository cricketTeamRepository,
            CricketServiceContext context,
            IMapper mapper)
        {
            this.logger = logger;
            this.cricketTeamRepository = cricketTeamRepository;
            this.context = context;
            this.mapper = mapper;
        }

        public IEnumerable<Guid> GetAllPlayersUuid()
        {
            return context.CricketPlayerInfo.Select(x => x.Uuid);
        }

        public IEnumerable<Guid> GetAllTeamssUuid()
        {
            return context.CricketTeamInfo.Select(x => x.Uuid);
        }

        public async Task UpdatePlayersCareerStatistics()
        {
            var counter = 1;

            var t20iResponse = context.T20ICricketMatchInfo
                    .OrderBy(x => Convert.ToInt32(x.MatchNo.Replace("T20I no. ", string.Empty)))
                    .Select(x => x.ToDomain(mapper));

            var odiResponse = context.ODICricketMatchInfo
                    .OrderBy(x => Convert.ToInt32(x.MatchNo.Replace("ODI no. ", string.Empty)))
                    .Select(x => x.ToDomain(mapper));

            var testResponse = context.TestCricketMatchInfo
                    .OrderBy(x => Convert.ToInt32(x.MatchNo.Replace("Test no. ", string.Empty)))
                    .Select(x => x.ToDomain(mapper));

            List<Guid> result = GetAllPlayersUuid().Where(x => x == new Guid("7c5181e5-08d2-4c8e-844b-6dd57cef00eb")).ToList();

            var startTime = DateTime.Now;

            foreach (var uuid in result)
            {
                logger.LogInformation($"updating career statistics for player no {counter} with uuid {uuid}");

                var player = context.CricketPlayerInfo.Single(x => x.Uuid == uuid);
                var t20iStats = t20iResponse.GetPlayerStats(player.InternationalTeamNames.FirstOrDefault()!, player.PlayerName.Trim(), true);
                var odiStats = odiResponse.GetPlayerStats(player.InternationalTeamNames.FirstOrDefault()!, player.PlayerName.Trim(), true);
                var testStats = testResponse.GetTestPlayerStats(player.InternationalTeamNames.FirstOrDefault()!, player.PlayerName.Trim(), true);

                player.CareerStatistics = new CareerDetailsInfo(
                    testStats,
                    odiStats,
                    t20iStats
                );

                context.CricketPlayerInfo.Update(player);

                await context.SaveChangesAsync();
                counter++;

                var stepTime = DateTime.Now;

                logger.LogInformation($"Running watch: {stepTime - startTime}");
            }

            var endTime = DateTime.Now;

            logger.LogInformation($"Total seeding time is {endTime - startTime}");
        }

        public async Task UpdateTeamRecords()
        {
            var counter = 1;

            var t20iResponse = context.T20ICricketMatchInfo
                    .OrderBy(x => Convert.ToInt32(x.MatchNo.Replace("T20I no. ", string.Empty)))
                    .Select(x => mapper.Map<CricketMatchInfoResponse>(x));

            var odiResponse = context.ODICricketMatchInfo
                    .OrderBy(x => Convert.ToInt32(x.MatchNo.Replace("ODI no. ", string.Empty)))
                    .Select(x => mapper.Map<CricketMatchInfoResponse>(x));

            var testResponse = context.TestCricketMatchInfo
                    .OrderBy(x => Convert.ToInt32(x.MatchNo.Replace("Test no. ", string.Empty)))
                    .Select(x => mapper.Map<TestCricketMatchInfoResponse>(x));

            List<Guid> uuids = GetAllTeamssUuid().ToList();

            var startTime = DateTime.Now;

            foreach (var uuid in uuids)
            {
                logger.LogInformation($"updating team statistics for team with uuid {uuid}");

                var team = context.CricketTeamInfo.Single(x => x.Uuid == uuid);
                var teamRecords = cricketTeamRepository.GetTeamByName(team.TeamName);

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
        }

    }
}
