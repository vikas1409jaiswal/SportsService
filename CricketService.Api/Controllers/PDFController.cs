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
public class PDFController : Controller
{
    private ICricketMatchRepository cricketMatchRepository;

    public PDFController(ICricketMatchRepository cricketMatchRepository)
    {
        this.cricketMatchRepository = cricketMatchRepository ?? throw new ArgumentNullException($"{nameof(cricketMatchRepository)} is null");
    }

    [HttpPost("cricketMatches/generatePdf")]
    public async Task<IActionResult> GeneratePDFForMatches(
        [FromBody, Required, ModelBinder(Name = "MatchUuids")] IEnumerable<Guid> matchUuids,
        [FromQuery, Required] CricketFormat format)
    {
        await cricketMatchRepository.GeneratedPDFForMatches(matchUuids, format);

        return Ok();
    }
}
