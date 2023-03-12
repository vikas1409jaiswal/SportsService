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
        CareerInfo testCareer,
        CareerInfo oDICareer,
        CareerInfo t20Career)
    {
        TestCareer = testCareer;
        ODICareer = oDICareer;
        T20Career = t20Career;
    }

    public CareerInfo TestCareer { get; set; }

    public CareerInfo ODICareer { get; set; }

    public CareerInfo T20Career { get; set; }
}

public class CareerInfo
{
    public CareerInfo(
        DebutDetails debutDetails,
        BattingStatistics battingStatistics,
        BowlingStatistics bowlingStatistics)
    {
        DebutDetails = debutDetails;
        BattingStatistics = battingStatistics;
        BowlingStatistics = bowlingStatistics;
    }

    public DebutDetails DebutDetails { get; set; }

    public BattingStatistics BattingStatistics { get; set; }

    public BowlingStatistics BowlingStatistics { get; set; }
}

public class BattingStatistics
{
    public BattingStatistics(
        int matches,
        int innings,
        int runs,
        int ballsFaced,
        int centuries,
        int halfCenturies,
        int highestScore,
        int fours,
        int sixes)
    {
        Matches = matches;
        Innings = innings;
        Runs = runs;
        BallsFaced = ballsFaced;
        Centuries = centuries;
        HalfCenturies = halfCenturies;
        HighestScore = highestScore;
        Fours = fours;
        Sixes = sixes;
    }

    public int Matches { get; set; }

    public int Innings { get; set; }

    public int Runs { get; set; }

    public int BallsFaced { get; set; }

    public int Centuries { get; set; }

    public int HalfCenturies { get; set; }

    public int HighestScore { get; set; }

    public int Fours { get; set; }

    public int Sixes { get; set; }

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
}

public class BowlingStatistics
{
    public BowlingStatistics(
        int matches,
        int innings,
        int wickets,
        Over overs,
        int runsConceded,
        int noBall,
        int wideBall,
        int maidens,
        int dots,
        int sixes,
        int fours)
    {
        Matches = matches;
        Innings = innings;
        Wickets = wickets;
        Overs = overs;
        RunsConceded = runsConceded;
        NoBall = noBall;
        WideBall = wideBall;
        Maidens = maidens;
        Dots = dots;
        Sixes = sixes;
        Fours = fours;
    }

    public int Matches { get; set; }

    public int Innings { get; set; }

    public int Wickets { get; set; }

    public Over Overs { get; set; }

    public int RunsConceded { get; set; }

    public int NoBall { get; set; }

    public int WideBall { get; set; }

    public int Maidens { get; set; }

    public int Dots { get; set; }

    public int Sixes { get; set; }

    public int Fours { get; set; }

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
}
