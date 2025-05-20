using CricketService.Domain.BaseDomains;

namespace CricketService.Domain.RequestDomains;

public class TestCricketMatchRequest : InternationalCricketMatch
{
    public TestCricketMatchRequest(
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
        DoubleInningTeamScoreboardRequest team1,
        DoubleInningTeamScoreboardRequest team2,
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

    public DoubleInningTeamScoreboardRequest Team1 { get; set; }

    public DoubleInningTeamScoreboardRequest Team2 { get; set; }
}
