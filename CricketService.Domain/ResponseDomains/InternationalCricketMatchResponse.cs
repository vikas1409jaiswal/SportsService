using CricketService.Domain.BaseDomains;
using CricketService.Domain.Common;

namespace CricketService.Domain.ResponseDomains;

public class InternationalCricketMatchResponse : InternationalCricketMatch
{
    public InternationalCricketMatchResponse(
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
        SingleInningTeamScoreboardResponse team1,
        SingleInningTeamScoreboardResponse team2,
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

    public SingleInningTeamScoreboardResponse Team1 { get; set; }

    public SingleInningTeamScoreboardResponse Team2 { get; set; }
}
