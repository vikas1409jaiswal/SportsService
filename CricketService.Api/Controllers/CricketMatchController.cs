using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using CricketService.Data.Repositories.Interfaces;
using CricketService.Domain;
using CricketService.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CricketService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CricketMatchController : Controller
{
    private ICricketMatchRepository cricketMatchRepository;

    public CricketMatchController(ICricketMatchRepository cricketMatchRepository)
    {
        this.cricketMatchRepository = cricketMatchRepository ?? throw new ArgumentNullException($"{nameof(cricketMatchRepository)} is null");
    }

    [HttpGet("internationalMatches")]
    public IActionResult GetInternationalMatches([FromQuery, Required] CricketFormat format)
    {
        IEnumerable<object> allMatches = new List<object>();

        switch (format)
        {
            case CricketFormat.T20I:
                allMatches = cricketMatchRepository.GetAllMatchesT20I();
                break;
            case CricketFormat.ODI:
                allMatches = cricketMatchRepository.GetAllMatchesODI();
                break;
            case CricketFormat.TestCricket:
                allMatches = cricketMatchRepository.GetAllMatchesTest();
                break;
            default:
                break;
        }

        Response.Headers.Add($"total-{format}-matches", allMatches.Count().ToString());

        return Ok(allMatches);
    }

    [HttpGet("internationalMatches/{matchNumber}")]
    public async Task<ActionResult<CricketMatchInfoResponse>> GetInternationalMatch([FromQuery, Required] CricketFormat format, [FromRoute, Required] int matchNumber)
    {
        object match = new object();

        switch (format)
        {
            case CricketFormat.T20I:
                match = await cricketMatchRepository.GetMatchByMNumberT20I(matchNumber);
                break;
            case CricketFormat.ODI:
                match = await cricketMatchRepository.GetMatchByMNumberODI(matchNumber);
                break;
            case CricketFormat.TestCricket:
                match = await cricketMatchRepository.GetMatchByMNumberTest(matchNumber);
                break;
            default:
                break;
        }

        return Ok(match);
    }

    [HttpGet("internationalMatches/{startMatchNumber}/{endMatchNumber}")]
    public async Task<ActionResult<IEnumerable<CricketMatchInfoResponse>>> GetInternationalMatchesByRangeNumber(
        [FromRoute, Required] int startMatchNumber,
        [FromRoute, Required] int endMatchNumber,
        [FromQuery, Required] CricketFormat format)
    {
        var allMatchNumbers = Enumerable.Range(startMatchNumber, endMatchNumber - startMatchNumber + 1);

        List<object> matches = new List<object>();

        foreach (var matchNumber in allMatchNumbers)
        {
            object match = new object();

            switch (format)
            {
                case CricketFormat.T20I:
                    match = await cricketMatchRepository.GetMatchByMNumberT20I(matchNumber);
                    break;
                case CricketFormat.ODI:
                    match = await cricketMatchRepository.GetMatchByMNumberODI(matchNumber);
                    break;
                case CricketFormat.TestCricket:
                    match = await cricketMatchRepository.GetMatchByMNumberTest(matchNumber);
                    break;
                default:
                    break;
            }

            if (match is not null)
            {
                matches.Add(match);
            }
        }

        return Ok(matches);
    }

    [HttpGet("internationalMatches/team/{teamUuid}")]
    public IActionResult GetInternationalMatchesByTeamUuid([FromRoute, Required] Guid teamUuid, [FromQuery, Required] CricketFormat format)
    {
        var allMatches = cricketMatchRepository.GetMatchesByTeamUuid(teamUuid, format);

        Response.Headers.Add($"total-{format}-matches", allMatches.Count().ToString());

        return Ok(allMatches);
    }

    [HttpPost("t20Match/addRange")]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> AddT20IMatches([FromBody, Required, ModelBinder(Name = "CricketMatchInfos")] IEnumerable<CricketMatchInfoRequest> cricketMatchInfoRequest)
    {
        var resultResponse = new { success = new List<string>(), failed = new List<string>() };
        var addingMatches = new List<CricketMatchInfoRequest>();

        foreach (var matchInfo in cricketMatchInfoRequest)
        {
            var foundMatch = await cricketMatchRepository.GetMatchByMNumberT20I(Convert.ToInt32(matchInfo.MatchNo.Replace("T20I no. ", string.Empty)));

            if (foundMatch is not null)
            {
                resultResponse.failed.Add(matchInfo.MatchNo);
            }
            else
            {
                addingMatches.Add(matchInfo);
            }
        }

        foreach (var matchInfo in addingMatches)
        {
            await cricketMatchRepository.AddMatchT20I(matchInfo);

            resultResponse.success.Add(matchInfo.MatchNo);
        }

        return Ok(new
        {
            successMatches = resultResponse.success.Count,
            failedMatches = resultResponse.failed.Count,
            failedMatchNumbers = resultResponse.failed,
        });
    }

    [HttpPost("odiMatch/addRange")]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> AddODIMatches([FromBody, Required, ModelBinder(Name = "CricketMatchInfos")] IEnumerable<CricketMatchInfoRequest> cricketMatchInfoRequest)
    {
        var resultResponse = new { success = new List<string>(), failed = new List<string>() };
        var addingMatches = new List<CricketMatchInfoRequest>();

        foreach (var matchInfo in cricketMatchInfoRequest)
        {
            var foundMatch = await cricketMatchRepository.GetMatchByMNumberODI(Convert.ToInt32(matchInfo.MatchNo.Replace("ODI no. ", string.Empty)));

            if (foundMatch is not null)
            {
                resultResponse.failed.Add(matchInfo.MatchNo);
            }
            else
            {
                addingMatches.Add(matchInfo);
            }
        }

        foreach (var matchInfo in addingMatches)
        {
            await cricketMatchRepository.AddMatchODI(matchInfo);

            resultResponse.success.Add(matchInfo.MatchNo);
        }

        return Ok(new
        {
            successMatches = resultResponse.success.Count,
            failedMatches = resultResponse.failed.Count,
            failedMatchNumbers = resultResponse.failed,
        });
    }

    [HttpPost("testMatch/addRange")]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> AddTestMatches([FromBody, Required, ModelBinder(Name = "CricketMatchInfos")] IEnumerable<TestCricketMatchInfoRequest> cricketMatchInfoRequest)
    {
        var resultResponse = new { success = new List<string>(), failed = new List<string>() };
        var addingMatches = new List<TestCricketMatchInfoRequest>();

        foreach (var matchInfo in cricketMatchInfoRequest)
        {
            var foundMatch = await cricketMatchRepository.GetMatchByMNumberTest(Convert.ToInt32(matchInfo.MatchNo.Replace("Test no. ", string.Empty)));

            if (foundMatch is not null)
            {
                resultResponse.failed.Add(matchInfo.MatchNo);
            }
            else
            {
                addingMatches.Add(matchInfo);
            }
        }

        foreach (var matchInfo in addingMatches)
        {
            await cricketMatchRepository.AddMatchTest(matchInfo);

            resultResponse.success.Add(matchInfo.MatchNo);
        }

        return Ok(new
        {
            successMatches = resultResponse.success.Count,
            failedMatches = resultResponse.failed.Count,
            failedMatchNumbers = resultResponse.failed,
        });
    }

    [HttpGet("internationalMatches/tournament/{tournament}")]
    public IActionResult GetInternationalTournamentMatches([FromRoute, Required] CricketTournament tournament, [FromQuery, Required] CricketFormat format)
    {
        var allMatches = cricketMatchRepository.GetMatchesByTournament(tournament, format);

        Response.Headers.Add($"total-{tournament}-matches", allMatches.Count().ToString());

        return Ok(allMatches);
    }
}