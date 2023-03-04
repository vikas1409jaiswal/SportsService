using System.ComponentModel.DataAnnotations;
using CricketService.Data.Repositories.Interfaces;
using CricketService.Domain;
using CricketService.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("teams/all")]
        public IActionResult GetAllTeams([FromQuery, Required] CricketFormat format)
        {
            List<string> allTeams = new List<string>();

            if (format == CricketFormat.T20I)
            {
                allTeams.AddRange(cricketTeamRepository.GetAllTeamNamesT20I());
            }
            else if (format == CricketFormat.ODI)
            {
                allTeams.AddRange(cricketTeamRepository.GetAllTeamNamesODI());
            }

            Response.Headers.Add("total-teams", allTeams.Count().ToString());

            return Ok(allTeams);
        }

        [HttpGet("teams/all/records")]
        public ActionResult<IEnumerable<CricketTeamInfo>> GetAllTeamRecords()
        {
            List<string> teamNames = new List<string>();
            teamNames = cricketTeamRepository.GetAllTeamNamesODI()
                .Concat(cricketTeamRepository.GetAllTeamNamesT20I()).Distinct().ToList();

            List<CricketTeamInfo> teamRecords = new List<CricketTeamInfo>();

            foreach (var teamName in teamNames)
            {
                var teamRecord = cricketTeamRepository.GetTeamRecordsByName(teamName, false);

                if (teamRecord != null)
                {
                    teamRecords.Add(teamRecord);
                }
            }

            return Ok(teamRecords);
        }

        [HttpGet("teams/{teamName}/records")]
        public ActionResult<CricketTeamInfo> GetAllTeamRecords([FromRoute, Required] string teamName)
        {
            var teamRecord = cricketTeamRepository.GetTeamRecordsByName(teamName, true);

            return Ok(teamRecord);
        }

        [HttpGet("teams/{teamName}/records/against/all")]
        public ActionResult<CricketTeamInfo> GetAllAgainstRecords([FromRoute, Required] string teamName, [FromQuery, Required] CricketFormat format)
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
