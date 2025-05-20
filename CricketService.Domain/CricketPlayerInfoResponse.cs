using System.ComponentModel.DataAnnotations;
using CricketService.Domain.Common;

namespace CricketService.Domain;

public class CricketPlayerInfoResponse
{
    public CricketPlayerInfoResponse(
        Guid playerUuid,
        string fullName,
        string dateOfBirth,
        string teamName,
        string birthPlace,
        CareerDetailsInfo careerDetailsInfo,
        string? imageSrc)
    {
        PlayerUuid = playerUuid;
        FullName = fullName;
        DateOfBirth = dateOfBirth;
        TeamName = teamName;
        BirthPlace = birthPlace;
        CareerDetails = careerDetailsInfo;
        ImageSrc = imageSrc;
    }

    public Guid PlayerUuid { get; set; } = Guid.Empty;

    public string FullName { get; set; } = string.Empty;

    public string DateOfBirth { get; set; }

    public string TeamName { get; set; } = string.Empty;

    public string BirthPlace { get; set; } = string.Empty;

    public CareerDetailsInfo CareerDetails { get; set; }

    public string? ImageSrc { get; set; } = string.Empty;
}

public class CareerDetailsInfo
{
    public CareerDetailsInfo(
        string teamName,
        CareerInfo testCareer,
        CareerInfo oDICareer,
        CareerInfo t20Career)
    {
        TeamName = teamName;
        TestCareer = testCareer;
        ODICareer = oDICareer;
        T20Career = t20Career;
    }

    public string TeamName { get; }

    public CareerInfo TestCareer { get; set; }

    public CareerInfo ODICareer { get; set; }

    public CareerInfo T20Career { get; set; }
}

public class CareerInfo
{
    public CareerInfo(
        MatchDetails debutDetails,
        MatchDetails lastMatchDetails,
        IEnumerable<BattingStatistics> battingStatistics,
        IEnumerable<BowlingStatistics> bowlingStatistics,
        IEnumerable<FieldingStatistics> fieldingStatistics)
    {
        DebutDetails = debutDetails;
        LastMatchDetails = lastMatchDetails;
        BattingStatistics = battingStatistics;
        BowlingStatistics = bowlingStatistics;
        FieldingStatistics = fieldingStatistics;
    }

    public MatchDetails DebutDetails { get; set; }

    public MatchDetails LastMatchDetails { get; set; }

    public IEnumerable<BattingStatistics> BattingStatistics { get; set; }

    public IEnumerable<BowlingStatistics> BowlingStatistics { get; set; }

    public IEnumerable<FieldingStatistics> FieldingStatistics { get; set; }
}

public class BattingStatistics
{
    public BattingStatistics(
        int matches,
        int innings,
        int notOut,
        int runs,
        int ducks,
        string highestScore,
        int ballsFaced,
        int centuries,
        int halfCenturies,
        int fours,
        int sixes,
        string span,
        string title,
        string subTitle)
    {
        Matches = matches;
        Innings = innings;
        NotOut = notOut;
        Runs = runs;
        Ducks = ducks;
        HighestScore = highestScore;
        BallsFaced = ballsFaced;
        Centuries = centuries;
        HalfCenturies = halfCenturies;
        Fours = fours;
        Sixes = sixes;
        Span = span;
        Title = title;
        SubTitle = subTitle;
    }

    public string Title { get; set; }

    public string SubTitle { get; set; }

    public string Span { get; set; }

    public int Matches { get; set; }

    public int Innings { get; set; }

    public int NotOut { get; set; }

    public int Ducks { get; set; }

    public int Runs { get; set; }

    public string HighestScore { get; set; }

    public double Average
    {
        get
        {
            var outTimes = Innings - NotOut;
            if (outTimes == 0)
            {
                return 0;
            }

            var average = (double)Runs / outTimes;
            return average;
        }
    }

    public int BallsFaced { get; set; }

    public double StrikeRate
    {
        get
        {
            if (BallsFaced == 0)
            {
                return 0;
            }

            var strikeRate = (double)(Runs * 100) / BallsFaced;
            return strikeRate;
        }
    }

    public int Centuries { get; set; }

    public int HalfCenturies { get; set; }

    public int Fours { get; set; }

    public int Sixes { get; set; }
}

public class BowlingStatistics
{
    public BowlingStatistics(
        int matches,
        int innings,
        int wickets,
        Over overs,
        int runsConceded,
        string bestBowlingInning,
        string bestBowlingMatch,
        int fourWicketsHaul,
        int fiveWicketsHaul,
        int tenWicketsHaul,
        int noBall,
        int wideBall,
        int maidens,
        int dots,
        int sixes,
        int fours,
        string span,
        string subTitle,
        string title)
    {
        Matches = matches;
        Innings = innings;
        Wickets = wickets;
        Overs = overs;
        RunsConceded = runsConceded;
        BestBowlingInning = bestBowlingInning;
        BestBowlingMatch = bestBowlingMatch;
        FourWicketsHaul = fourWicketsHaul;
        FiveWicketsHaul = fiveWicketsHaul;
        TenWicketsHaul = tenWicketsHaul;
        NoBall = noBall;
        WideBall = wideBall;
        Maidens = maidens;
        Dots = dots;
        Sixes = sixes;
        Fours = fours;
        Span = span;
        SubTitle = subTitle;
        Title = title;
    }

    public string Span { get; set; }

    public string SubTitle { get; set; }

    public string Title { get; set; }

    public int Matches { get; set; }

    public int Innings { get; set; }

    public int Wickets { get; set; }

    public Over Overs { get; set; }

    public int RunsConceded { get; set; }

    public string BestBowlingInning { get; set; }

    public string BestBowlingMatch { get; set; }

    public double Average
    {
        get
        {
            if (Wickets == 0)
            {
                return 0;
            }

            var average = (double)RunsConceded / Wickets;
            return average;
        }
    }

    public double Economy
    {
        get
        {
            if (Overs.Balls == 0)
            {
                return 0;
            }

            var economy = (double)(RunsConceded * 6) / Overs.Balls;
            return economy;
        }
    }

    public double StrikeRate
    {
        get
        {
            if (Wickets == 0)
            {
                return 0;
            }

            var economy = (double)Overs.Balls / Wickets;
            return economy;
        }
    }

    public int FourWicketsHaul { get; set; }

    public int FiveWicketsHaul { get; set; }

    public int TenWicketsHaul { get; set; }

    public int NoBall { get; set; }

    public int WideBall { get; set; }

    public int Maidens { get; set; }

    public int Dots { get; set; }

    public int Sixes { get; set; }

    public int Fours { get; set; }
}

public class FieldingStatistics
{
    public FieldingStatistics(
        int caught,
        int stumpings,
        int runOut,
        string span,
        string subTitle,
        string title,
        int matches,
        int innings,
        int dismissal,
        int caughtKeeper,
        int caughtFielder)
    {
        Caught = caught;
        Stumpings = stumpings;
        RunOut = runOut;
        Span = span;
        SubTitle = subTitle;
        Title = title;
        Matches = matches;
        Innings = innings;
        Dismissal = dismissal;
        CaughtKeeper = caughtKeeper;
        CaughtFielder = caughtFielder;
    }

    public string Span { get; set; }

    public string SubTitle { get; set; }

    public string Title { get; set; }

    public int Matches { get; set; }

    public int Innings { get; set; }

    public int Dismissal { get; set; }

    public int Caught { get; set; }

    public int CaughtKeeper { get; set; }

    public int CaughtFielder { get; set; }

    public int Stumpings { get; set; }

    public int RunOut { get; set; }
}
