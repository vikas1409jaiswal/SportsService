using CricketService.Domain.Common;

namespace CricketService.Domain;

public class CricketMatchInfoRequest : CricketMatchInfo
{
    public CricketMatchInfoRequest(
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
        TeamScoreDetailsRequest team1,
        TeamScoreDetailsRequest team2,
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

    public TeamScoreDetailsRequest Team1 { get; set; }

    public TeamScoreDetailsRequest Team2 { get; set; }
}

public class TeamScoreDetailsRequest
{
    public TeamScoreDetailsRequest(
        string teamName,
        IEnumerable<BattingScoreCardRequest> battingScoreCard,
        IEnumerable<BowlingScoreCardRequest> bowlingScoreCard,
        string extras,
        string[] fallOfWickets,
        Player[] didNotBat)
    {
        TeamName = teamName;
        BattingScoreCard = battingScoreCard;
        BowlingScoreCard = bowlingScoreCard;
        Extras = extras;
        FallOfWickets = fallOfWickets;
        DidNotBat = didNotBat;
    }

    public string TeamName { get; set; } = string.Empty;

    public IEnumerable<BattingScoreCardRequest> BattingScoreCard { get; set; }

    public IEnumerable<BowlingScoreCardRequest> BowlingScoreCard { get; set; }

    public string Extras { get; set; } = string.Empty;

    public string[] FallOfWickets { get; set; }

    public Player[] DidNotBat { get; set; }
}

public class Player
{
    public Player(string name, string href)
    {
        Name = name;
        Href = href;
    }

    public string Name { get; set; }

    public string Href { get; set; }
}

public class BattingScoreCardRequest
{
    public BattingScoreCardRequest(
        Player playerName,
        string outStatus,
        int? runsScored = null,
        int? ballsFaced = null,
        int? fours = null,
        int? sixes = null,
        int? minutes = null)
    {
        PlayerName = playerName;
        OutStatus = outStatus;
        RunsScored = runsScored;
        BallsFaced = ballsFaced;
        Minutes = minutes;
        Fours = fours;
        Sixes = sixes;
    }

    public Player PlayerName { get; set; }

    public string OutStatus { get; set; } = string.Empty;

    public int? RunsScored { get; set; } = null!;

    public int? BallsFaced { get; set; } = null!;

    public int? Minutes { get; set; } = null!;

    public int? Fours { get; set; } = null!;

    public int? Sixes { get; set; } = null!;
}

public class BowlingScoreCardRequest
{
    public BowlingScoreCardRequest(
        Player playerName,
        double oversBowled,
        int maidens,
        int runsConceded,
        int wickets,
        int wideBall,
        int noBall,
        int? dots = null,
        int? fours = null,
        int? sixes = null)
    {
        PlayerName = playerName;
        OversBowled = oversBowled;
        Maidens = maidens;
        RunsConceded = runsConceded;
        Wickets = wickets;
        Dots = dots;
        Fours = fours;
        Sixes = sixes;
        WideBall = wideBall;
        NoBall = noBall;
    }

    public Player PlayerName { get; set; }

    public double OversBowled { get; set; }

    public int Maidens { get; set; }

    public int RunsConceded { get; set; }

    public int Wickets { get; set; }

    public int? Dots { get; set; } = null!;

    public int? Fours { get; set; } = null!;

    public int? Sixes { get; set; } = null!;

    public int WideBall { get; set; }

    public int NoBall { get; set; }
}