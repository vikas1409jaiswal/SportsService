using CricketService.Data.Repositories.Interfaces;
using CricketService.Domain;
using CricketService.Domain.Enums;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CricketService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CricketTeamController : Controller
    {
        private ICricketTeamRepository cricketTeamRepository;

        public CricketTeamController(ICricketTeamRepository cricketTeamRepository)
        {
            this.cricketTeamRepository = cricketTeamRepository;
        }

        [HttpGet("team/{teamUuid}")]
        public IActionResult GetTeamByUuid([FromRoute, Required] Guid teamUuid)
        {
            var team = cricketTeamRepository.GetTeamByUuid(teamUuid);

            return Ok(team);
        }

        [HttpGet("teamDetails/all")]
        public IActionResult GetAllTeamDetails()
        {
            var allTeamDetails = cricketTeamRepository.GetAllTeamDetails();

            Response.Headers.Add("total-teams", allTeamDetails.Count().ToString());

            return Ok(allTeamDetails);
        }

        [HttpGet("teams/all")]
        public IActionResult GetAllTeams([FromQuery, Required] CricketFormat format)
        {
            List<string> allTeams = new();

            allTeams.AddRange(cricketTeamRepository.GetAllTeamNames(format));

            Response.Headers.Add($"total-{format}-teams", allTeams.Count().ToString());

            return Ok(allTeams);
        }

        [HttpGet("teams/all/records")]
        public ActionResult<IEnumerable<CricketTeamInfoResponse>> GetAllTeamRecords()
        {
            List<string> teamNames = cricketTeamRepository.GetAllTeamNames(CricketFormat.All).ToList();

            List<CricketTeamInfoResponse> teamRecords = new List<CricketTeamInfoResponse>();

            foreach (var teamName in teamNames)
            {
                var teamRecord = cricketTeamRepository.GetTeamRecordsByName(teamName, false);

                if (teamRecord != null)
                {
                    teamRecords.Add(teamRecord);
                }
            }

            Response.Headers.Add("total-international-teams", teamRecords.Count().ToString());

            return Ok(teamRecords);
        }

        [HttpGet("teams/{teamName}/records")]
        public ActionResult<CricketTeamInfoResponse> GetAllTeamRecords([FromRoute, Required] string teamName)
        {
            var teamRecord = cricketTeamRepository.GetTeamRecordsByName(teamName, true);

            return Ok(teamRecord);
        }

        [HttpGet("teams/{teamName}/records/against/all")]
        public ActionResult<CricketTeamInfoResponse> GetAllAgainstRecords([FromRoute, Required] string teamName, [FromQuery, Required] CricketFormat format)
        {
            object teamRecord = null!;

            if (format == CricketFormat.T20I)
            {
                teamRecord = cricketTeamRepository.GetAllAgainstRecordsByNameT20I(teamName);
            }
            else if (format == CricketFormat.ODI)
            {
                teamRecord = cricketTeamRepository.GetAllAgainstRecordsByNameODI(teamName);
            }

            return Ok(teamRecord);
        }
    }
}
