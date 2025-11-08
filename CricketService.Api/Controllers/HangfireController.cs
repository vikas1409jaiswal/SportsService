using System.ComponentModel.DataAnnotations;
using CricketService.Data.Repositories.Interfaces;
using CricketService.Domain.Enums;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace CricketService.Api.Controllers
{
    [ApiController]
    [Route("[controller]/jobs")]
    public class HangfireController : Controller
    {
        private readonly IHangfireRepository hangfireRepository;
        private readonly ICricketTeamHistoryRepository cricketTeamHistoryRepository;
        private readonly ICricketTeamHistoryH2HRepository cricketTeamHistoryH2HRepository;

        public HangfireController(
            IHangfireRepository hangfireRepository,
            ICricketTeamHistoryRepository cricketTeamHistoryRepository,
            ICricketTeamHistoryH2HRepository cricketTeamHistoryH2HRepository)
        {
            this.hangfireRepository = hangfireRepository;
            this.cricketTeamHistoryRepository = cricketTeamHistoryRepository;
            this.cricketTeamHistoryH2HRepository = cricketTeamHistoryH2HRepository;
        }

        [HttpGet]
        public IActionResult CreateHangfireJob(
            [FromQuery, Required] CricketJob jobName,
            [FromQuery] string team1Name,
            [FromQuery] string team2Name,
            [FromQuery] CricketFormat format)
        {
            switch (jobName)
            {
                case CricketJob.CreateSeedCricketTeamHistoryTableJob:
                    RecurringJob.AddOrUpdate(
                         $"{CricketJob.CreateSeedCricketTeamHistoryTableJob}:{format}",
                         () => cricketTeamHistoryRepository.SeedCricketTeamHistoryTable(format, false),
                         Cron.Never);
                    break;

                case CricketJob.CreateSeedCricketTeamHistoryH2HTableJob:
                    RecurringJob.AddOrUpdate(
                         $"{CricketJob.CreateSeedCricketTeamHistoryH2HTableJob}:{team1Name}vs{team2Name}:{format}",
                         () => cricketTeamHistoryH2HRepository.SeedCricketTeamHistoryH2HTable(team1Name, team2Name, format, true),
                         Cron.Never);
                    break;

                case CricketJob.UpdatePlayersCareerStatisticsJob:
                    RecurringJob.AddOrUpdate(
                        CricketJob.UpdatePlayersCareerStatisticsJob.ToString(),
                        () => hangfireRepository.UpdatePlayersCareerStatistics(),
                        Cron.Never);
                    break;

                case CricketJob.UpdateTeamRecordsJob:
                    RecurringJob.AddOrUpdate(
                        CricketJob.UpdateTeamRecordsJob.ToString(),
                        () => hangfireRepository.UpdateTeamRecords(null),
                        Cron.Never);
                    break;

                case CricketJob.CleanDatabaseJob:
                    RecurringJob.AddOrUpdate(
                        CricketJob.CleanDatabaseJob.ToString(),
                        () => hangfireRepository.CleanDatabase(),
                        Cron.Never);
                    break;

                default:
                    return BadRequest($"Invalid job name. Valid options are: " +
                        $"{CricketJob.CreateSeedCricketTeamHistoryTableJob}, " +
                        $"{CricketJob.CreateSeedCricketTeamHistoryH2HTableJob}, " +
                        $"{CricketJob.UpdatePlayersCareerStatisticsJob}, " +
                        $"{CricketJob.UpdateTeamRecordsJob}, " +
                        $"{CricketJob.CleanDatabaseJob}");
            }

            return Ok($"Job '{jobName}' has been scheduled successfully.");
        }
    }
}