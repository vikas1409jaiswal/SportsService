using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using CricketService.Data.Repositories.Interfaces;
using CricketService.Domain;
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

    [HttpGet("t20Match/all")]
    public ActionResult<IEnumerable<CricketMatchInfoResponse>> GetAllMatchesT20I()
    {
        var allMatches = cricketMatchRepository.GetAllMatchesT20I();

        Response.Headers.Add("total-t20I-matches", allMatches.Count().ToString());

        return Ok(allMatches);
    }

    [HttpGet("odiMatch/all")]
    public ActionResult<IEnumerable<CricketMatchInfoResponse>> GetAllMatchesODI()
    {
        var allMatches = cricketMatchRepository.GetAllMatchesODI();

        Response.Headers.Add("total-ODI-matches", allMatches.Count().ToString());

        return Ok(allMatches);
    }

    [HttpGet("t20Match/{matchNumber}")]
    public async Task<ActionResult<CricketMatchInfoResponse>> GetMatchByMNumberT20I([FromRoute, Required] int matchNumber)
    {
        return Ok(await cricketMatchRepository.GetMatchByMNumberT20I(matchNumber));
    }

    [HttpGet("odiMatch/{matchNumber}")]
    public async Task<ActionResult<CricketMatchInfoResponse>> GetMatchByMNumberODI([FromRoute, Required] int matchNumber)
    {
        return Ok(await cricketMatchRepository.GetMatchByMNumberODI(matchNumber));
    }

    [HttpGet("t20Match/range")]
    public async Task<ActionResult<IEnumerable<CricketMatchInfoResponse>>> GetMatchByMNumberRangeT20I(
        [FromQuery, Required] int startMatchNumber,
        [FromQuery, Required] int endMatchNumber)
    {
        var allMatchNumber = Enumerable.Range(startMatchNumber, endMatchNumber - startMatchNumber);

        List<CricketMatchInfoResponse> matches = new List<CricketMatchInfoResponse>();

        foreach (var matchNumber in allMatchNumber)
        {
            var match = await cricketMatchRepository.GetMatchByMNumberT20I(matchNumber);

            if (match is not null)
            {
                matches.Add(match);
            }
        }

        return Ok(matches);
    }

    [HttpGet("odiMatch/range")]
    public async Task<ActionResult<IEnumerable<CricketMatchInfoResponse>>> GetMatchByMNumberRangeODI(
        [FromQuery, Required] int startMatchNumber,
        [FromQuery, Required] int endMatchNumber)
    {
        var allMatchNumber = Enumerable.Range(startMatchNumber, endMatchNumber - startMatchNumber);

        List<CricketMatchInfoResponse> matches = new List<CricketMatchInfoResponse>();

        foreach (var matchNumber in allMatchNumber)
        {
            var match = await cricketMatchRepository.GetMatchByMNumberODI(matchNumber);

            if (match is not null)
            {
                matches.Add(match);
            }
        }

        return Ok(matches);
    }

    [HttpGet("t20Match")]
    public ActionResult<IEnumerable<CricketMatchInfoResponse>> GetMatchByTeamNameT20I([FromQuery, Required] string teamName)
    {
        var allMatches = cricketMatchRepository.GetMatchByTeamT20I(teamName);

        Response.Headers.Add("total-t20I-matches", allMatches.Count().ToString());

        return Ok(allMatches);
    }

    [HttpGet("odiMatch")]
    public ActionResult<IEnumerable<CricketMatchInfoResponse>> GetMatchByTeamNameODI([FromQuery, Required] string teamName)
    {
        var allMatches = cricketMatchRepository.GetMatchByTeamODI(teamName);

        Response.Headers.Add("total-ODI-matches", allMatches.Count().ToString());

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
}