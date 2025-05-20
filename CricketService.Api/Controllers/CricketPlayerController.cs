using System.ComponentModel.DataAnnotations;
using CricketService.Data.Repositories.Interfaces;
using CricketService.Domain;
using CricketService.Domain.Common;
using CricketService.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
    public ActionResult<PlayerDetails> GetPlayerByUuid([FromRoute, Required] Guid playerUuid)
    {
        var player = cricketPlayerRepository.GetPlayerByUuid(playerUuid);

        return Ok(player);
    }

    [HttpGet("players/all")]
    public ActionResult<PlayerDetails> GetAllPlayers(
        [FromQuery, Required] CricketFormat format,
        [FromQuery] string? teamName,
        [FromQuery] string? dateOfBirth,
        [FromQuery] int? birthYear,
        [FromQuery] bool? isExpired,
        [FromQuery] string? playingRole,
        [FromQuery] string? nameStartsWith)
    {
        var filters = new PlayersFilters(format, teamName, dateOfBirth, birthYear, isExpired, playingRole, nameStartsWith);

        var allPlayers = cricketPlayerRepository.GetAllPlayers(filters);

        Response.Headers.Add("total-players", allPlayers.Count().ToString());

        return Ok(allPlayers);
    }

    [HttpGet("players/unique")]
    public IActionResult GetAllPlayersUuids([FromQuery, Required] CricketFormat format)
    {
        var allPlayers = cricketPlayerRepository.GetAllPlayersUuidAndHref(format);

        Response.Headers.Add("total-players", allPlayers.Count().ToString());

        return Ok(allPlayers);
    }

    [HttpGet("team/{teamName}/player/{playerName}")]
    public IActionResult GetAllPlayersByTeamName([FromRoute, Required] string teamName, [FromRoute, Required] string playerName)
    {
        var playerDetail = cricketPlayerRepository.GetPlayerDetailsByTeamName(teamName, playerName, true);

        return Ok(playerDetail);
    }
}