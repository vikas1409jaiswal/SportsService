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

public class TotalInningScore
{
    public TotalInningScore(
        int runs,
        int wickets,
        Over overs,
        string extras)
    {
        Wickets = wickets;
        Overs = overs;
        Extras = new ExtraRuns(extras);
        Runs = runs + Extras.TotalExtras;
    }

    public int Runs { get; set; }

    public int Wickets { get; set; }

    public Over Overs { get; set; }

    public ExtraRuns Extras { get; set; }
}

public class ExtraRuns
{
    public ExtraRuns(string extra)
    {
        var totalExtras = 0;
        IDictionary<string, int> extraDictionary = new Dictionary<string, int>();
        if (extra.Length > 0 && extra.Contains('(') && extra.Contains(')'))
        {
            extra.Replace("(", string.Empty).Replace(")", string.Empty)
           .Split(",").Select(e => e.Trim()).ToList()
           .ForEach(x => extraDictionary.Add(x.Split(" ")[0], Convert.ToInt32(x.Split(" ")[1])));

            TotalExtras = ExtraDetails.Sum(x => x.Value);
        }
        else if (extra.Length > 0 && int.TryParse(extra, out totalExtras))
        {
            TotalExtras = totalExtras;
        }

        ExtraDetails = extraDictionary;
    }

    public IDictionary<string, int> ExtraDetails { get; }

    public int TotalExtras { get; set; }
}