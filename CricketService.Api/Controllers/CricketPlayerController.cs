using System.ComponentModel.DataAnnotations;
using CricketService.Data.Repositories.Interfaces;
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

    [HttpGet("player/{playerUuid}")]
    public IActionResult GetTeamByUuid([FromRoute, Required] Guid playerUuid)
    {
        var player = cricketPlayerRepository.GetPlayerByUuid(playerUuid);

        return Ok(player);
    }

    [HttpGet("players/all")]
    public IActionResult GetAllPlayers()
    {
        var allPlayers = cricketPlayerRepository.GetAllPlayers();

        Response.Headers.Add("total-players", allPlayers.Count().ToString());

        return Ok(allPlayers);
    }

    [HttpGet("players")]
    public IActionResult GetAllPlayersName([FromQuery, Required] CricketFormat format)
    {
        var allTeamsName = cricketTeamRepository.GetAllTeamNames(format);

        Dictionary<string, IEnumerable<string>> allPlayers = new();

        foreach (var teamName in allTeamsName)
        {
            var playersByTeam = cricketPlayerRepository.GetPlayersByTeamName(teamName, format);
            allPlayers.Add(teamName, playersByTeam);
        }

        Response.Headers.Add("total-teams-length", allTeamsName.Count().ToString());

        return Ok(allPlayers);
    }

    [HttpGet("team/{teamName}/players")]
    public IActionResult GetAllPlayersByTeamName([FromRoute, Required] string teamName, [FromQuery, Required] CricketFormat format)
    {
        List<string> playersByTeam = new();

        if (format == CricketFormat.T20I)
        {
            playersByTeam.AddRange(cricketPlayerRepository.GetPlayersByTeamName(teamName, format));
        }

        if (format == CricketFormat.ODI)
        {
            playersByTeam.AddRange(cricketPlayerRepository.GetPlayersByTeamName(teamName, format));
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