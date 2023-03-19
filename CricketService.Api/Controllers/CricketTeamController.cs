using System.ComponentModel.DataAnnotations;
using CricketService.Data.Repositories.Interfaces;
using CricketService.Domain;
using CricketService.Domain.Enums;
using CricketService.Domain.Exceptions;
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

        [HttpGet("team/{teamUuid}")]
        public IActionResult GetTeamByUuid([FromRoute, Required] Guid teamUuid)
        {
            CricketTeamInfoResponse teamInfo;

            try
            {
                teamInfo = cricketTeamRepository.GetTeamByUuid(teamUuid);
            }
            catch (CricketTeamNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            return Ok(teamInfo);
        }

        [HttpGet("teams/{teamName}")]
        public ActionResult<CricketTeamInfoResponse> GetTeamByName([FromRoute, Required] string teamName)
        {
            CricketTeamInfoResponse teamInfo;

            try
            {
                teamInfo = cricketTeamRepository.GetTeamByName(teamName);
            }
            catch (CricketTeamNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            return Ok(teamInfo);
        }

        [HttpGet("teams/all")]
        public IActionResult GetAllTeams()
        {
            var allTeams = cricketTeamRepository.GetAllTeamDetails();

            Response.Headers.Add("total-teams", allTeams.Count().ToString());

            return Ok(allTeams);
        }

        [HttpGet("teamsDetails/all")]
        public IActionResult GetAllTeamDetails([FromQuery, Required] CricketFormat format)
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
                var teamRecord = cricketTeamRepository.GetTeamByName(teamName);

                if (teamRecord != null)
                {
                    teamRecords.Add(teamRecord);
                }
            }

            Response.Headers.Add("total-international-teams", teamRecords.Count().ToString());

            return Ok(teamRecords);
        }

        [HttpGet("teams/{teamUuid}/records/against/all")]
        public ActionResult<CricketTeamInfoResponse> GetAllAgainstRecords([FromRoute, Required] Guid teamUuid, [FromQuery, Required] CricketFormat format)
        {
            object teamRecord = null!;

            if (format == CricketFormat.T20I)
            {
                teamRecord = cricketTeamRepository.GetAllAgainstRecordsByTeamT20I(teamUuid);
            }
            else if (format == CricketFormat.ODI)
            {
                teamRecord = cricketTeamRepository.GetAllAgainstRecordsByTeamODI(teamUuid);
            }
            else if (format == CricketFormat.TestCricket)
            {
                teamRecord = cricketTeamRepository.GetAllAgainstRecordsByTeamTest(teamUuid);
            }

            return Ok(teamRecord);
        }
    }
}
