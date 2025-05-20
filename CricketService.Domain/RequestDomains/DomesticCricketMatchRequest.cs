using CricketService.Domain.BaseDomains;

namespace CricketService.Domain.RequestDomains;

public class DomesticCricketMatchRequest : DomesticCricketMatch
{
    public DomesticCricketMatchRequest(
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
        SingleInningTeamScoreboardRequest team1,
        SingleInningTeamScoreboardRequest team2,
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

    public SingleInningTeamScoreboardRequest Team1 { get; set; }

    public SingleInningTeamScoreboardRequest Team2 { get; set; }
}
