using CricketService.Domain.Common;

namespace CricketService.Domain;

public class TestCricketMatchInfoResponse : CricketMatchInfo
{
    public TestCricketMatchInfoResponse(
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
        TestTeamScoreDetails team1,
        TestTeamScoreDetails team2,
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

    public TestTeamScoreDetails Team1 { get; set; }

    public TestTeamScoreDetails Team2 { get; set; }
}

public class TestTeamScoreDetails {
    public TestTeamScoreDetails(
        string teamName,
        InningScoreCard inning1,
        InningScoreCard inning2)
    {
        TeamName = teamName;
        Inning1 = inning1;
        Inning2 = inning2;
    }

    public string TeamName { get; set; } = string.Empty;

    public InningScoreCard Inning1 { get; set; }

    public InningScoreCard Inning2 { get; set; }
}

public class InningScoreCard
{
    public InningScoreCard(
        ICollection<BattingScoreCard> battingScoreCard,
        ICollection<BowlingScoreCard> bowlingScoreCard,
        string extras,
        string[] fallOfWickets,
        Player[] didNotBat)
    {
        BattingScoreCard = battingScoreCard;
        BowlingScoreCard = bowlingScoreCard;
        Extras = extras;
        FallOfWickets = fallOfWickets;
        DidNotBat = didNotBat;
    }

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
