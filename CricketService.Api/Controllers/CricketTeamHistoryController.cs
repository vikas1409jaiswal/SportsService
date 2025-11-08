using System.ComponentModel.DataAnnotations;
using CricketService.Data.Repositories.Interfaces;
using CricketService.Domain.BaseDomains;
using CricketService.Domain;
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

        if (format == CricketFormat.T20I || format == CricketFormat.ODI || format == CricketFormat.TestCricket)
        {
            allTeamsHistory = await cricketTeamHistoryRepository.GetTeamsHistory(format);
        }

        return Ok(allTeamsHistory);
    }

    [HttpGet("internationalTeamsHistoryH2h")]
    public async Task<IActionResult> GetInternationalTeamsHistoryH2H(
       [FromQuery, Required] CricketFormat format,
       [FromQuery] string team1Name,
       [FromQuery] string team2Name)
    {
        IEnumerable<object> allTeamsHistory = new List<object>();

        if (format == CricketFormat.T20I || format == CricketFormat.ODI || format == CricketFormat.TestCricket)
        {
            allTeamsHistory = await cricketTeamHistoryH2HRepository.GetTeamsHistoryH2H(format, team1Name, team2Name);
        }

        return Ok(allTeamsHistory);
    }

    [HttpGet("internationalTeamsHistory/allPlayers")]
    public async Task<IActionResult> GetInternationalPlayers(
        [FromQuery, Required] CricketFormat format,
        [FromQuery, Required] PlayersCategory playersCategory,
        [FromQuery] string teamName)
    {
        IEnumerable<object> allTeamsPlayers = new List<object>();

        if (format == CricketFormat.T20I || format == CricketFormat.ODI || format == CricketFormat.TestCricket)
        {
            allTeamsPlayers = await cricketTeamHistoryRepository.GetTeamsPlayers(format, playersCategory, teamName);
        }

        return Ok(allTeamsPlayers);
    }
}