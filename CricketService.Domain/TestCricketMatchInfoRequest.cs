namespace CricketService.Domain;

public class TestCricketMatchInfoRequest : CricketMatchInfo
{
    public TestCricketMatchInfoRequest(
        string season,
        string series,
        string playerOfTheMatch,
        string matchNo,
        string matchDays,
        string matchTitle,
        string venue,
        string matchDate,
        string tossWinner,
        string tossDecision,
        string result,
        TestTeamScoreDetailsRequest team1,
        TestTeamScoreDetailsRequest team2,
        string tvUmpire,
        string matchReferee,
        string reserveUmpire,
        string[] umpires,
        string[] formatDebut,
        string[] internationalDebut)
        : base(
            season,
            series,
            playerOfTheMatch,
            matchNo,
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
            internationalDebut)
    {
        Team1 = team1;
        Team2 = team2;
    }

    public TestTeamScoreDetailsRequest Team1 { get; set; }

    public TestTeamScoreDetailsRequest Team2 { get; set; }
}

public class TestTeamScoreDetailsRequest
{
    public TestTeamScoreDetailsRequest(
        string teamName,
        InningScoreCardDetails inning1,
        InningScoreCardDetails inning2)
    {
        TeamName = teamName;
        Inning1 = inning1;
        Inning2 = inning2;
    }

    public string TeamName { get; set; } = string.Empty;

    public InningScoreCardDetails Inning1 { get; set; }

    public InningScoreCardDetails Inning2 { get; set; }
}

public class InningScoreCardDetails
{
    public InningScoreCardDetails(
        ICollection<BattingScoreCardRequest> battingScorecard,
        ICollection<BowlingScoreCardRequest> bowlingScorecard,
        string extras,
        string[] fallOfWickets,
        Player[] didNotBat)
    {
        BattingScorecard = battingScorecard;
        BowlingScorecard = bowlingScorecard;
        Extras = extras;
        FallOfWickets = fallOfWickets;
        DidNotBat = didNotBat;
    }

    public ICollection<BattingScoreCardRequest> BattingScorecard { get; set; }

    public ICollection<BowlingScoreCardRequest> BowlingScorecard { get; set; }

    public string Extras { get; set; } = string.Empty;

    public string[] FallOfWickets { get; set; }

    public Player[] DidNotBat { get; set; }
}