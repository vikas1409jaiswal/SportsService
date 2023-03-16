using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using CricketService.Data.Contexts;
using CricketService.Data.Repositories;
using CricketService.Data.Repositories.Interfaces;
using CricketService.Domain;
using Hangfire;
using Hangfire.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace CricketService.Api.Controllers
{
    [ApiController]
    [Route("[controller]/jobs")]
    public class HangfireController : Controller
    {
        private readonly IHangfireRepository hangfireRepository;

        public HangfireController(IHangfireRepository hangfireRepository)
        {
            this.hangfireRepository = hangfireRepository;
        }

        [HttpGet]
        public IActionResult Hangfire()
        {
            RecurringJob.AddOrUpdate(() => hangfireRepository.UpdatePlayersCareerStatistics(), Cron.Never);

            RecurringJob.AddOrUpdate(() => hangfireRepository.UpdateTeamRecords(), Cron.Never);

            return Ok();
        }
    }
}
