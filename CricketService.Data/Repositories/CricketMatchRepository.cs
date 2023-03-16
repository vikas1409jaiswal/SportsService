using System.Collections;
using CricketService.Data.Contexts;
using CricketService.Data.Entities;
using CricketService.Data.Extensions;
using CricketService.Data.Repositories.Extensions;
using CricketService.Data.Repositories.Interfaces;
using CricketService.Domain;
using CricketService.Domain.Enums;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CricketService.Data.Repositories
{
    public class CricketMatchRepository : ICricketMatchRepository
    {
        private ILogger<CricketMatchRepository> logger;
        private CricketServiceContext context;
        private readonly ICricketPlayerRepository cricketPlayerRepository;

        public CricketMatchRepository(ILogger<CricketMatchRepository> logger, CricketServiceContext cricketServiceContext, ICricketPlayerRepository cricketPlayerRepository)
        {
            this.logger = logger;
            context = cricketServiceContext;
            this.cricketPlayerRepository = cricketPlayerRepository;
        }

        public IEnumerable<CricketMatchInfoResponse> GetAllMatchesT20I()
        {
            logger.LogInformation($"Fetching details for all T20I matches.");

            var matchesList = context.T20ICricketMatchInfo.AsEnumerable()
                .OrderBy(x => Convert.ToInt32(x.MatchNo.Replace("T20I no. ", string.Empty)));

            logger.LogInformation($"{matchesList.Count()} matches found.");

            return matchesList.Select(x => x.ToDomain());
        }

        public IEnumerable<CricketMatchInfoResponse> GetAllMatchesODI()
        {
            logger.LogInformation($"Fetching details for all ODI matches.");

            var matchesList = context.ODICricketMatchInfo.AsEnumerable()
                .OrderBy(x => Convert.ToInt32(x.MatchNo.Replace("ODI no. ", string.Empty)));

            logger.LogInformation($"{matchesList.Count()} matches found.");

            return matchesList.Select(x => x.ToDomain());
        }

        public async Task<CricketMatchInfoResponse> GetMatchByMNumberT20I(int matchNumber)
        {
            logger.LogInformation($"Fetching details for match number {matchNumber}");

            Entities.T20ICricketMatchInfo match = null!;

            try
            {
                match = await context.T20ICricketMatchInfo.SingleAsync(x => x.MatchNo == $"T20I no. {matchNumber}");
            }
            catch (JsonReaderException)
            {
                match = new Entities.T20ICricketMatchInfo();
            }
            catch (JsonSerializationException)
            {
                match = new Entities.T20ICricketMatchInfo();
            }
            catch (Exception ex)
            {
                Console.WriteLine("xxxxxxxxxxx", ex.Message);
            }

            return match?.ToDomain()!;
        }

        public async Task<CricketMatchInfoResponse> GetMatchByMNumberODI(int matchNumber)
        {
            logger.LogInformation($"Fetching details for match number {matchNumber}");

            Entities.ODICricketMatchInfo match = null!;

            try
            {
                match = await context.ODICricketMatchInfo.SingleAsync(x => x.MatchNo == $"ODI no. {matchNumber}");
            }
            catch (JsonReaderException)
            {
                match = new Entities.ODICricketMatchInfo();
            }
            catch (JsonSerializationException)
            {
                match = new Entities.ODICricketMatchInfo();
            }
            catch (Exception ex)
            {
                Console.WriteLine("xxxxxxxxxxx", ex.Message);
            }

            return match?.ToDomain()!;
        }

        public async Task<TestCricketMatchInfoResponse> GetMatchByMNumberTest(int matchNumber)
        {
            logger.LogInformation($"Fetching details for match number {matchNumber}");

            Entities.TestCricketMatchInfo match = null!;

            try
            {
                match = await context.TestCricketMatchInfo.SingleAsync(x => x.MatchNo == $"Test no. {matchNumber}");
            }
            catch (JsonReaderException)
            {
                match = new Entities.TestCricketMatchInfo();
            }
            catch (JsonSerializationException)
            {
                match = new Entities.TestCricketMatchInfo();
            }
            catch (Exception ex)
            {
                Console.WriteLine("xxxxxxxxxxx", ex.Message);
            }

            return match?.ToDomain()!;
        }

        public IEnumerable<CricketMatchInfoResponse> GetMatchByTeamT20I(string teamName)
        {
            logger.LogInformation($"Fetching details for team {teamName}");

            var matchesList = context.T20ICricketMatchInfo.Where(x => ((x.Team1.TeamName == teamName) || (x.Team2.TeamName == teamName)));

            logger.LogInformation($"{matchesList.Count()} matches found.");

            return matchesList.Select(x => x.ToDomain());
        }

        public IEnumerable<CricketMatchInfoResponse> GetMatchByTeamODI(string teamName)
        {
            logger.LogInformation($"Fetching details for team {teamName}");

            var matchesList = context.ODICricketMatchInfo.Where(x => ((x.Team1.TeamName == teamName) || (x.Team2.TeamName == teamName)));

            logger.LogInformation($"{matchesList.Count()} matches found.");

            return matchesList.Select(x => x.ToDomain());
        }

        public async Task<CricketMatchInfoResponse> AddMatchT20I(CricketMatchInfoRequest match)
        {
            var result = context.T20ICricketMatchInfo.Add(match.ToEntityT20I());

            await context.SaveChangesAsync();

            var response = result.Entity.ToDomain();

            if (response is not null)
            {
                await response.Team1.SaveTeamInfo(context, CricketFormat.T20I);
                await response.Team2.SaveTeamInfo(context, CricketFormat.T20I);

                if (response.Team1.Playing11 is not null)
                {
                    await response.Team1.SavePlayersForTeam(context, CricketFormat.T20I);
                }

                if (response.Team2.Playing11 is not null)
                {
                    await response.Team2.SavePlayersForTeam(context, CricketFormat.T20I);
                }
            }

            return response!;
        }

        public async Task<CricketMatchInfoResponse> AddMatchODI(CricketMatchInfoRequest match)
        {
            var result = context.ODICricketMatchInfo.Add(match.ToEntityODI());

            await context.SaveChangesAsync();

            var response = result.Entity.ToDomain();

            if (response is not null)
            {
                await response.Team1.SaveTeamInfo(context, CricketFormat.ODI);
                await response.Team2.SaveTeamInfo(context, CricketFormat.ODI);

                if (response.Team1.Playing11 is not null)
                {
                    await response.Team1.SavePlayersForTeam(context, CricketFormat.ODI);
                }

                if (response.Team2.Playing11 is not null)
                {
                    await response.Team2.SavePlayersForTeam(context, CricketFormat.ODI);
                }
            }

            return response!;
        }

        public async Task<TestCricketMatchInfoResponse> AddMatchTest(TestCricketMatchInfoRequest match)
        {
            var result = context.TestCricketMatchInfo.Add(match.ToEntityTest());

            await context.SaveChangesAsync();

            var response = result.Entity.ToDomain();

            if (response is not null)
            {
                await response.Team1.SaveTestTeamInfo(context);
                await response.Team2.SaveTestTeamInfo(context);

                if (response.Team1.Inning1.Playing11 is not null)
                {
                    await response.Team1.SavePlayersForTestTeam(context);
                }

                if (response.Team2.Inning1.Playing11 is not null)
                {
                    await response.Team2.SavePlayersForTestTeam(context);
                }
            }

            return response!;
        }

        public IEnumerable<Guid> GetAllPlayersUuid()
        {
            return context.CricketPlayerInfo.Select(x => x.Uuid);
        }

        public async Task UpdatePlayersCareerStatistics()
        {
            var counter = 1;

            var t20iResponse = context.T20ICricketMatchInfo
                    .OrderBy(x => Convert.ToInt32(x.MatchNo.Replace("T20I no. ", string.Empty)))
                    .Select(x => x.ToDomain());

            var odiResponse = context.ODICricketMatchInfo
                    .OrderBy(x => Convert.ToInt32(x.MatchNo.Replace("ODI no. ", string.Empty)))
                    .Select(x => x.ToDomain());

            var testResponse = context.TestCricketMatchInfo
                    .OrderBy(x => Convert.ToInt32(x.MatchNo.Replace("Test no. ", string.Empty)))
                    .Select(x => x.ToDomain());

            List<Guid> result = GetAllPlayersUuid().Where(x => x == new Guid("7c5181e5-08d2-4c8e-844b-6dd57cef00eb")).ToList();

            var startTime = DateTime.Now;

            foreach (var uuid in result)
            {
                Console.WriteLine($"updating career statistics for player no {counter} with uuid {uuid}");

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

                Console.WriteLine($"Running watch: {stepTime - startTime}");
            }

            var endTime = DateTime.Now;

            Console.WriteLine($"Total seeding time is {endTime - startTime}");
        }
    }
}
