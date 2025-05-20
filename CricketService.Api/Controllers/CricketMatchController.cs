using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using CricketService.Data.Repositories.Interfaces;
using CricketService.Domain.Common;
using CricketService.Domain.Enums;
using CricketService.Domain.RequestDomains;
using CricketService.Domain.ResponseDomains;
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
    public IActionResult GetInternationalMatches(
        [FromQuery, Required] CricketFormat format,
        [FromQuery] Guid teamUuid,
        [FromQuery] Guid oppositionTeamUuid)
    {
        IEnumerable<object> allMatches = new List<object>();

        var matchesFilters = new MatchesFilters(format, teamUuid, oppositionTeamUuid);

        if (format == CricketFormat.T20I || format == CricketFormat.ODI)
        {
           allMatches = cricketMatchRepository.GetAllLimitedOverInternationalMatches(matchesFilters);
        }
        else if (format == CricketFormat.TestCricket)
        {
           allMatches = cricketMatchRepository.GetAllMatchesTest(matchesFilters);
        }

        Response.Headers.Add($"total-{format}-matches", allMatches.Count().ToString());

        return Ok(allMatches);
    }

    [HttpGet("domesticMatches")]
    public IActionResult GetDomesticMatches(
        [FromQuery, Required] CricketFormat format,
        [FromQuery] Guid teamUuid,
        [FromQuery] Guid oppositionTeamUuid)
    {
        IEnumerable<object> allMatches = new List<object>();

        var matchesFilters = new MatchesFilters(format, teamUuid, oppositionTeamUuid);

        if (format == CricketFormat.Twenty20)
        {
            allMatches = cricketMatchRepository.GetAllTwenty20Matches(matchesFilters);
        }

        Response.Headers.Add($"total-{format}-matches", allMatches.Count().ToString());

        return Ok(allMatches);
    }

    [HttpGet("internationalMatches/{matchNumber}")]
    public async Task<ActionResult<InternationalCricketMatchResponse>> GetInternationalMatch(
        [FromQuery, Required] CricketFormat format,
        [FromRoute, Required] int matchNumber)
    {
        object match = new object();

        switch (format)
        {
            case CricketFormat.T20I:
                match = await cricketMatchRepository.GetLimitedOverInternationalMatchByNumber(matchNumber, format);
                break;
            case CricketFormat.ODI:
                match = await cricketMatchRepository.GetLimitedOverInternationalMatchByNumber(matchNumber, format);
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
    public async Task<ActionResult<IEnumerable<InternationalCricketMatchResponse>>> GetInternationalMatchesByRangeNumber(
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
                    match = await cricketMatchRepository.GetLimitedOverInternationalMatchByNumber(matchNumber, format);
                    break;
                case CricketFormat.ODI:
                    match = await cricketMatchRepository.GetLimitedOverInternationalMatchByNumber(matchNumber, format);
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

    [HttpPost("t20IMatch/addRange")]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> AddT20IMatches([FromBody, Required, ModelBinder(Name = "CricketMatchInfos")] IEnumerable<InternationalCricketMatchRequest> cricketMatchInfoRequest)
    {
        var resultResponse = new { success = new List<string>(), failed = new List<string>() };
        var addingMatches = new List<InternationalCricketMatchRequest>();

        foreach (var matchInfo in cricketMatchInfoRequest)
        {
            var foundMatch = await cricketMatchRepository.GetLimitedOverInternationalMatchByNumber(Convert.ToInt32(matchInfo.MatchNumber.Replace("T20I no. ", string.Empty)), CricketFormat.T20I);

            if (foundMatch is not null)
            {
                resultResponse.failed.Add(matchInfo.MatchNumber);
            }
            else
            {
                addingMatches.Add(matchInfo);
            }
        }

        foreach (var matchInfo in addingMatches)
        {
            await cricketMatchRepository.AddLimitedOverInternationalMatch(matchInfo, CricketFormat.T20I);

            resultResponse.success.Add(matchInfo.MatchNumber);
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
    public async Task<IActionResult> AddODIMatches([FromBody, Required, ModelBinder(Name = "CricketMatchInfos")] IEnumerable<InternationalCricketMatchRequest> cricketMatchInfoRequest)
    {
        var resultResponse = new { success = new List<string>(), failed = new List<string>() };
        var addingMatches = new List<InternationalCricketMatchRequest>();

        foreach (var matchInfo in cricketMatchInfoRequest)
        {
            var foundMatch = await cricketMatchRepository.GetLimitedOverInternationalMatchByNumber(Convert.ToInt32(matchInfo.MatchNumber.Replace("ODI no. ", string.Empty)), CricketFormat.ODI);

            if (foundMatch is not null)
            {
                resultResponse.failed.Add(matchInfo.MatchNumber);
            }
            else
            {
                addingMatches.Add(matchInfo);
            }
        }

        foreach (var matchInfo in addingMatches)
        {
            await cricketMatchRepository.AddLimitedOverInternationalMatch(matchInfo, CricketFormat.ODI);

            resultResponse.success.Add(matchInfo.MatchNumber);
        }

        return Ok(new
        {
            successMatches = resultResponse.success.Count,
            failedMatches = resultResponse.failed.Count,
            failedMatchNumbers = resultResponse.failed,
        });
    }

    [HttpPost("t20Match/addRange")]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> AddT20Matches([FromBody, Required, ModelBinder(Name = "T20Matches")] IEnumerable<DomesticCricketMatchRequest> t20MatchesRequest)
    {
        var resultResponse = new { success = new List<string>(), failed = new List<string>() };
        var addingMatches = new List<DomesticCricketMatchRequest>();

        foreach (var matchInfo in t20MatchesRequest)
        {
            var foundMatch = await cricketMatchRepository.GetT20IMatchesByTitleAndDate(matchInfo.MatchTitle, matchInfo.MatchDate);

            if (foundMatch is not null)
            {
                resultResponse.failed.Add($"{matchInfo.MatchTitle}::{matchInfo.MatchDate}");
            }
            else
            {
                addingMatches.Add(matchInfo);
            }
        }

        foreach (var matchInfo in addingMatches)
        {
            await cricketMatchRepository.AddT20Match(matchInfo);

            resultResponse.success.Add($"{matchInfo.MatchTitle}::{matchInfo.MatchDate}");
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
    public async Task<IActionResult> AddTestMatches([FromBody, Required, ModelBinder(Name = "CricketMatchInfos")] IEnumerable<TestCricketMatchRequest> cricketMatchInfoRequest)
    {
        var resultResponse = new { success = new List<string>(), failed = new List<string>() };
        var addingMatches = new List<TestCricketMatchRequest>();

        foreach (var matchInfo in cricketMatchInfoRequest)
        {
            var foundMatch = await cricketMatchRepository.GetMatchByMNumberTest(Convert.ToInt32(matchInfo.MatchNumber.Replace("Test no. ", string.Empty)));

            if (foundMatch is not null)
            {
                resultResponse.failed.Add(matchInfo.MatchNumber);
            }
            else
            {
                addingMatches.Add(matchInfo);
            }
        }

        foreach (var matchInfo in addingMatches)
        {
            await cricketMatchRepository.AddMatchTest(matchInfo);

            resultResponse.success.Add(matchInfo.MatchNumber);
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