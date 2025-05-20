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
        private const string CreateSeedCricketTeamHistoryTableJob = "CreateSeedCricketTeamHistoryTableJob";
        private const string CreateSeedCricketTeamHistoryH2HTableJob = "CreateSeedCricketTeamHistoryH2HTableJob";
        private const string UpdatePlayersCareerStatisticsJob = "UpdatePlayersCareerStatisticsJob";
        private const string UpdateTeamRecordsJob = "UpdateTeamRecordsJob";
        private const string CleanDatabaseJob = "CleanDatabaseJob";

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
            [FromQuery, Required] string jobName,
            [FromQuery] string team1Name,
            [FromQuery] string team2Name,
            [FromQuery] CricketFormat format)
        {
            switch (jobName)
            {
                case CreateSeedCricketTeamHistoryTableJob:
                    RecurringJob.AddOrUpdate(
                         CreateSeedCricketTeamHistoryTableJob,
                         () => cricketTeamHistoryRepository.SeedCricketTeamHistoryTable(),
                         Cron.Never);
                    break;

                case CreateSeedCricketTeamHistoryH2HTableJob:
                    RecurringJob.AddOrUpdate(
                         $"{CreateSeedCricketTeamHistoryH2HTableJob}:{team1Name}vs{team2Name}:{format}",
                         () => cricketTeamHistoryH2HRepository.SeedCricketTeamHistoryH2HTable(team1Name, team2Name, format),
                         Cron.Never);
                    break;

                case UpdatePlayersCareerStatisticsJob:
                    RecurringJob.AddOrUpdate(
                        UpdatePlayersCareerStatisticsJob,
                        () => hangfireRepository.UpdatePlayersCareerStatistics(),
                        Cron.Never);
                    break;

                case UpdateTeamRecordsJob:
                    RecurringJob.AddOrUpdate(
                        UpdateTeamRecordsJob,
                        () => hangfireRepository.UpdateTeamRecords(null),
                        Cron.Never);
                    break;

                case CleanDatabaseJob:
                    RecurringJob.AddOrUpdate(
                        CleanDatabaseJob,
                        () => hangfireRepository.CleanDatabase(),
                        Cron.Never);
                    break;

                default:
                    return BadRequest($"Invalid job name. Valid options are: " +
                        $"{CreateSeedCricketTeamHistoryTableJob}, " +
                        $"{UpdatePlayersCareerStatisticsJob}, " +
                        $"{UpdateTeamRecordsJob}, " +
                        $"{CleanDatabaseJob}");
            }

            return Ok($"Job '{jobName}' has been scheduled successfully.");
        }
    }
}