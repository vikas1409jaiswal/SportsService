using CricketService.Domain.Common;

namespace CricketService.Domain;

public class CricketMatchInfoResponse : CricketMatchInfo
{
    public CricketMatchInfoResponse(
        Guid? matchUuid,
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
        TeamScoreDetails team1,
        TeamScoreDetails team2,
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
        MatchUuid = matchUuid;
        Team1 = team1;
        Team2 = team2;
    }

    public Guid? MatchUuid { get; set; }

    public TeamScoreDetails Team1 { get; set; }

    public TeamScoreDetails Team2 { get; set; }
}

public class TeamScoreDetails
{
    public TeamScoreDetails(
        string teamName,
        ICollection<BattingScoreCard> battingScoreCard,
        ICollection<BowlingScoreCard> bowlingScoreCard,
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

    public string FlagUrl { get; set; } = string.Empty;

    public string LogoUrl { get; set; } = string.Empty;

    public ICollection<BattingScoreCard> BattingScoreCard { get; set; }

    public ICollection<BowlingScoreCard> BowlingScoreCard { get; set; }

    public string Extras { get; set; } = string.Empty;

    public string[] FallOfWickets { get; set; }

    public Player[] DidNotBat { get; set; }

    public Player[] Playing11
    {
        get
        {
            var playing11 = BattingScoreCard.Select(x => x.PlayerName).Concat(DidNotBat).ToArray();
            if (playing11.Length == 0)
            {
                return null!;
            }

            if (playing11.Length < 11)
            {
                throw new FormatException("playing 11 should contain exact 11 players.");
            }

            return playing11;
        }
    }

    public TotalInningScore TotalInningDetails
    {
        get
        {
            return new TotalInningScore(
                (int)BattingScoreCard.Sum(x => x.RunsScored)!,
                FallOfWickets.Length,
                BowlingScoreCard.Sum(x => new Over(x.OversBowled).Balls).ToOvers(),
                Extras);
        }
    }
}

public class BattingScoreCard
{
    public BattingScoreCard(
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

    public double StrikeRate
    {
        get
        {
            if (BallsFaced == 0 || BallsFaced is null)
            {
                return 0;
            }

            var strikeRate = (double)(RunsScored! * 100) / BallsFaced;
            return (double)strikeRate!;
        }
    }
}

public class BowlingScoreCard
{
    public BowlingScoreCard(
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

    public double Economy
    {
        get
        {
            var balls = new Over(OversBowled).Balls;
            if (balls == 0)
            {
                return 0;
            }

            var economy = (double)(RunsConceded * 6) / balls;
            return economy;
        }
    }

    public int? Dots { get; set; } = null!;

    public int? Fours { get; set; } = null!;

    public int? Sixes { get; set; } = null!;

    public int WideBall { get; set; }

    public int NoBall { get; set; }
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
        IDictionary<string, int> extraDictionary = new Dictionary<string, int>();
        if (extra.Length > 0)
        {
            extra.Replace("(", string.Empty).Replace(")", string.Empty)
           .Split(",").Select(e => e.Trim()).ToList()
           .ForEach(x => extraDictionary.Add(x.Split(" ")[0], Convert.ToInt32(x.Split(" ")[1])));
        }

        ExtraDetails = extraDictionary;
    }

    public IDictionary<string, int> ExtraDetails { get; }

    public int TotalExtras
    {
        get
        {
            if (ExtraDetails.Count == 0)
            {
                return 0;
            }

            return ExtraDetails.Sum(x => x.Value);
        }
    }
}