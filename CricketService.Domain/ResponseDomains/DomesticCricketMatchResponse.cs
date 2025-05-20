using CricketService.Domain.BaseDomains;

namespace CricketService.Domain.ResponseDomains;

public class DomesticCricketMatchResponse : DomesticCricketMatch
{
    public DomesticCricketMatchResponse(
        Guid matchUuid,
        string season,
        string series,
        string seriesResult,
        string matchDays,
        string matchTitle,
        string venue,
        string matchDate,
        string tossWinner,
        string tossDecision,
        string result,
        SingleInningTeamScoreboardResponse team1,
        SingleInningTeamScoreboardResponse team2,
        string tvUmpire,
        string matchReferee,
        string reserveUmpire,
        string[] umpires,
        List<CricketPlayer> formatDebut,
        PlayerOfTheMatch? playerOfTheMatch = null)
        : base(
            matchUuid,
            season,
            series,
            seriesResult,
            matchDays,
            matchTitle,
            venue,
            matchDate,
            tossWinner,
            tossDecision,
            result,
            tvUmpire,
            matchReferee,
            reserveUmpire,
            umpires,
            formatDebut,
            playerOfTheMatch)
    {
        Team1 = team1;
        Team2 = team2;
    }

    public SingleInningTeamScoreboardResponse Team1 { get; set; }

    public SingleInningTeamScoreboardResponse Team2 { get; set; }
}
