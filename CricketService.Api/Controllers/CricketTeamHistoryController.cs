using System.ComponentModel.DataAnnotations;
using CricketService.Data.Repositories.Interfaces;
using CricketService.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CricketService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CricketTeamHistoryController : Controller
{
    private readonly ICricketTeamHistoryRepository cricketTeamHistoryRepository;
    private readonly ICricketTeamHistoryH2HRepository cricketTeamHistoryH2HRepository;

    public CricketTeamHistoryController(
        ICricketTeamHistoryRepository cricketTeamHistoryRepository,
        ICricketTeamHistoryH2HRepository cricketTeamHistoryH2HRepository)
    {
        this.cricketTeamHistoryRepository = cricketTeamHistoryRepository;
        this.cricketTeamHistoryH2HRepository = cricketTeamHistoryH2HRepository;
    }

    [HttpGet("internationalTeamsHistory")]
    public async Task<IActionResult> GetInternationalTeamsHistory(
        [FromQuery, Required] CricketFormat format)
    {
        IEnumerable<object> allTeamsHistory = new List<object>();

        if (format == CricketFormat.T20I || format == CricketFormat.ODI)
        {
            allTeamsHistory = await cricketTeamHistoryRepository.GetTeamsHistory(format);
        }
        else if (format == CricketFormat.TestCricket)
        {
            //allTeamsHistory = cricketMatchRepository.GetAllMatchesTest(matchesFilters);
        }

        Response.Headers.Add($"total-{format}-matches", allTeamsHistory.Count().ToString());

        return Ok(allTeamsHistory);
    }

    [HttpGet("internationalTeamsHistoryH2h")]
    public async Task<IActionResult> GetInternationalTeamsHistoryH2H(
       [FromQuery, Required] CricketFormat format,
       [FromQuery] string team1Name,
       [FromQuery] string team2Name)
    {
        IEnumerable<object> allTeamsHistory = new List<object>();

        if (format == CricketFormat.T20I || format == CricketFormat.ODI)
        {
            allTeamsHistory = await cricketTeamHistoryH2HRepository.GetTeamsHistoryH2H(format, team1Name, team2Name);
        }
        else if (format == CricketFormat.TestCricket)
        {
            //allTeamsHistory = cricketMatchRepository.GetAllMatchesTest(matchesFilters);
        }

        Response.Headers.Add($"total-{format}-matches", allTeamsHistory.Count().ToString());

        return Ok(allTeamsHistory);
    }
}