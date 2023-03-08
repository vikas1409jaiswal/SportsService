using CricketService.Data.Contexts;
using CricketService.Data.Extensions;
using CricketService.Data.Repositories.Extensions;
using CricketService.Data.Repositories.Interfaces;
using CricketService.Domain;
using CricketService.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CricketService.Data.Repositories
{
    public class CricketMatchRepository : ICricketMatchRepository
    {
        private ILogger<CricketMatchRepository> logger;
        private CricketServiceContext context;

        public CricketMatchRepository(ILogger<CricketMatchRepository> logger, CricketServiceContext cricketServiceContext)
        {
            this.logger = logger;
            context = cricketServiceContext;
        }

        public IEnumerable<CricketMatchInfoResponse> GetAllMatchesT20I()
        {
            logger.LogInformation($"Fetching details for all matches.");

            var matchesList = context.T20ICricketMatchInfo.AsEnumerable();

            logger.LogInformation($"{matchesList.Count()} matches found.");

            return matchesList.Select(x => x.ToDomain());
        }

        public IEnumerable<CricketMatchInfoResponse> GetAllMatchesODI()
        {
            logger.LogInformation($"Fetching details for all matches.");

            var matchesList = context.ODICricketMatchInfo.AsEnumerable();

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
    }
}
