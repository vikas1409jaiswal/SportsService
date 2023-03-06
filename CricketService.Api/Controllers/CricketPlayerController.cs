using System.ComponentModel.DataAnnotations;
using CricketService.Data.Repositories.Interfaces;
using CricketService.Domain;
using CricketService.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CricketService.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class CricketPlayerController : Controller
{
    private ICricketPlayerRepository cricketPlayerRepository;
    private ICricketTeamRepository cricketTeamRepository;

    public CricketPlayerController(
        ICricketPlayerRepository cricketPlayerRepository,
        ICricketTeamRepository cricketTeamRepository)
    {
        this.cricketPlayerRepository = cricketPlayerRepository;
        this.cricketTeamRepository = cricketTeamRepository;
    }

    [HttpGet("t20IMatches/players/all")]
    public IActionResult GetAllPlayersT20I()
    {
        var allTeamsName = cricketTeamRepository.GetAllTeamNamesT20I();

        Dictionary<string, IEnumerable<string>> allPlayers = new Dictionary<string, IEnumerable<string>>();

        foreach (var teamName in allTeamsName)
        {
            var playersByTeam = cricketPlayerRepository.GetPlayersByTeamNameT20I(teamName);
            allPlayers.Add(teamName, playersByTeam);
        }

        Response.Headers.Add("total-teams-length", allTeamsName.Count().ToString());

        return Ok(allPlayers);
    }

    [HttpGet("odiMatches/players/all")]
    public IActionResult GetAllPlayersODI()
    {
        var allTeamsName = cricketTeamRepository.GetAllTeamNamesODI();

        Dictionary<string, IEnumerable<string>> allPlayers = new Dictionary<string, IEnumerable<string>>();

        foreach (var teamName in allTeamsName)
        {
            var playersByTeam = cricketPlayerRepository.GetPlayersByTeamNameODI(teamName);
            allPlayers.Add(teamName, playersByTeam);
        }

        Response.Headers.Add("total-teams-length", allTeamsName.Count().ToString());

        return Ok(allPlayers);
    }

    [HttpGet("team/{teamName}/players")]
    public IActionResult GetAllPlayersByTeamName([FromRoute, Required] string teamName, [FromQuery, Required] CricketFormat format)
    {
        List<string> playersByTeam = new List<string>();

        if (format == CricketFormat.T20I)
        {
           playersByTeam.AddRange(cricketPlayerRepository.GetPlayersByTeamNameT20I(teamName));
        }

        if (format == CricketFormat.ODI)
        {
           playersByTeam.AddRange(cricketPlayerRepository.GetPlayersByTeamNameODI(teamName));
        }

        var playerDetails = new List<object>();

        foreach (var playerName in playersByTeam)
        {
            var playerDetail = cricketPlayerRepository.GetPlayerDetailsByTeamName(teamName, playerName);
            playerDetails.Add(playerDetail);
        }

        Response.Headers.Add($"total-{format}-players", playerDetails.Count().ToString());

        return Ok(playerDetails);
    }

    [HttpGet("team/{teamName}/player/{playerName}")]
    public IActionResult GetAllPlayersByTeamName([FromRoute, Required] string teamName, [FromRoute, Required] string playerName)
    {
        var playerDetail = cricketPlayerRepository.GetPlayerDetailsByTeamName(teamName, playerName, true);

        return Ok(playerDetail);
    }
}