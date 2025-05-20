using CricketService.Domain.BaseDomains;

namespace CricketService.Domain.RequestDomains;

public class InternationalCricketMatchRequest : InternationalCricketMatch
{
    public InternationalCricketMatchRequest(
        Guid matchUuid,
        string season,
        string series,
        string seriesResult,
        string matchNumber,
        string matchDate,
        string matchDays,
        string matchTitle,
        string venue,
        string tossWinner,
        string tossDecision,
        string result,
        SingleInningTeamScoreboardRequest team1,
        SingleInningTeamScoreboardRequest team2,
        string tvUmpire,
        string matchReferee,
        string reserveUmpire,
        string[] umpires,
        List<CricketPlayer> internationalDebut,
        PlayerOfTheMatch? playerOfTheMatch = null)
        : base(
            matchUuid,
            season,
            series,
            seriesResult,
            matchNumber,
            matchDate,
            matchDays,
            matchTitle,
            venue,
            tossWinner,
            tossDecision,
            result,
            tvUmpire,
            matchReferee,
            reserveUmpire,
            umpires,
            internationalDebut,
            playerOfTheMatch)
    {
        Team1 = team1;
        Team2 = team2;
    }

    public SingleInningTeamScoreboardRequest Team1 { get; set; }

    public SingleInningTeamScoreboardRequest Team2 { get; set; }
}
