using CricketService.Domain.BaseDomains;
using CricketService.Domain.Common;

namespace CricketService.Domain
{
    public class CricketTeamInfoResponse
    {
        public CricketTeamInfoResponse(
            Guid teamUuid,
            string teamName,
            TeamRecordDetails recordDetails,
            string flagUri)
        {
            TeamUuid = teamUuid;
            TeamName = teamName;
            TeamRecordDetails = recordDetails;
            FlagUri = flagUri;
        }

        public Guid TeamUuid { get; set; } = Guid.Empty;

        public string TeamName { get; set; }

        public TeamRecordDetails TeamRecordDetails { get; set; }

        public string FlagUri { get; set; }
    }

    public class TeamRecordDetails
    {
        public TeamRecordDetails(
            TeamFormatRecordDetails t20IResults,
            TeamFormatRecordDetails oDIResults,
            TeamFormatRecordDetails testResults)
        {
            T20IResults = t20IResults;
            ODIResults = oDIResults;
            TestResults = testResults;
        }

        public TeamFormatRecordDetails T20IResults { get; set; }

        public TeamFormatRecordDetails ODIResults { get; set; }

        public TeamFormatRecordDetails TestResults { get; set; }
    }

    public class TeamFormatRecordDetails
    {
        public TeamFormatRecordDetails(
            MatchDetails debut,
            int matches,
            int won,
            int lost,
            int tied,
            int nRorDraw,
            TeamMileStones teamMileStones)
        {
            Debut = debut;
            Matches = matches;
            Won = won;
            Lost = lost;
            Tied = tied;
            TeamMileStones = teamMileStones;
            NRorDraw = nRorDraw;
        }

        public MatchDetails Debut { get; set; }

        public int Matches { get; set; }

        public int Won { get; set; }

        public int Lost { get; set; }

        public int Tied { get; set; }

        public int NRorDraw { get; set; }

        public double WinPercentage
        {
            get
            {
                if (Matches == 0)
                {
                    return 0;
                }

                return ((Won + (0.5 * Tied)) / Matches) * 100;
            }
        }

        public TeamMileStones TeamMileStones { get; set; }
    }

    public class MatchDetails
    {
        public MatchDetails(
            Guid matchUuid,
            string date,
            string opponent,
            string venue,
            string result,
            string matchNo)
        {
            MatchUuid = matchUuid;
            Date = date;
            Opponent = opponent;
            Venue = venue;
            Result = result;
            MatchNo = matchNo;
        }

        public Guid MatchUuid { get; set; }

        public string Date { get; set; }

        public string Opponent { get; set; }

        public string Venue { get; set; }

        public string Result { get; set; }

        public string MatchNo { get; set; }
    }

    public class TeamMileStones
    {
        public TeamMileStones(
            int careerRuns,
            int careerDucks,
            int careerCenturies,
            int careerHalfCenturies,
            int careerOneAndHalfCenturies,
            int careerDoubleCenturies,
            InningRecordDetails inningRecordDetails,
            KeyValuePair<string, int> mostInnings,
            KeyValuePair<string, int> mostRuns,
            KeyValuePair<string, int> mostWickets,
            KeyValuePair<string, int> mostDucks,
            KeyValuePair<string, string> bestBowlingInning,
            KeyValuePair<string, string> bestBowlingMatch,
            KeyValuePair<string, int> mostSixes,
            KeyValuePair<string, int> mostFours,
            KeyValuePair<string, int> most50s,
            KeyValuePair<string, int> most100s,
            KeyValuePair<string, int> most150s,
            KeyValuePair<string, int> most200s,
            KeyValuePair<string, int> hiScore,
            List<KeyValuePair<CricketPlayer, PlayerRepresentedDetails>> playersRepresented,
            List<KeyValuePair<CricketPlayer, PlayerRepresentedDetails>> captainsRepresented,
            List<KeyValuePair<CricketPlayer, PlayerRepresentedDetails>> wicketKeepersRepresented)
        {
            CareerRuns = careerRuns;
            CareerDucks = careerDucks;
            CareerCenturies = careerCenturies;
            CareerHalfCenturies = careerHalfCenturies;
            CareerOneAndHalfCenturies = careerOneAndHalfCenturies;
            CareerDoubleCenturies = careerDoubleCenturies;
            InningRecordDetails = inningRecordDetails;
            MostInnings = mostInnings;
            MostRuns = mostRuns;
            MostWickets = mostWickets;
            MostDucks = mostDucks;
            BestBowlingInning = bestBowlingInning;
            BestBowlingMatch = bestBowlingMatch;
            MostSixes = mostSixes;
            MostFours = mostFours;
            Most50s = most50s;
            Most100s = most100s;
            Most150s = most150s;
            Most200s = most200s;
            HighestIndividualScore = hiScore;
            PlayersRepresented = playersRepresented;
            CaptainsRepresented = captainsRepresented;
            WicketKeepersRepresented = wicketKeepersRepresented;
        }

        public int CareerRuns { get; set; }

        public int CareerDucks { get; set; }

        public int CareerCenturies { get; set; }

        public int CareerHalfCenturies { get; set; }

        public int CareerOneAndHalfCenturies { get; set; }

        public int CareerDoubleCenturies { get; set; }

        public InningRecordDetails InningRecordDetails { get; set; }

        public KeyValuePair<string, int> MostInnings { get; set; }

        public KeyValuePair<string, int> MostRuns { get; set; }

        public KeyValuePair<string, int> MostWickets { get; set; }

        public KeyValuePair<string, int> MostDucks { get; set; }

        public KeyValuePair<string, string> BestBowlingInning { get; set; }

        public KeyValuePair<string, string> BestBowlingMatch { get; set; }

        public KeyValuePair<string, int> MostSixes { get; set; }

        public KeyValuePair<string, int> MostFours { get; set; }

        public KeyValuePair<string, int> Most50s { get; set; }

        public KeyValuePair<string, int> Most100s { get; set; }

        public KeyValuePair<string, int> Most150s { get; set; }

        public KeyValuePair<string, int> Most200s { get; set; }

        public KeyValuePair<string, int> HighestIndividualScore { get; set; }

        public List<KeyValuePair<CricketPlayer, PlayerRepresentedDetails>> PlayersRepresented { get; set; }

        public List<KeyValuePair<CricketPlayer, PlayerRepresentedDetails>> CaptainsRepresented { get; set; }

        public List<KeyValuePair<CricketPlayer, PlayerRepresentedDetails>> WicketKeepersRepresented { get; set; }
    }

    public class PlayerRepresentedDetails
    {
        public DateOnly FirstMatchDate { get; set; }

        public Guid FirstMatchUuid { get; set; }
    }

    public class InningRecordDetails
    {
        public InningRecordDetails(
            InningTotalDetails highestTotal,
            InningTotalDetails lowestTotal)
        {
            HighestTotal = highestTotal;
            LowestTotal = lowestTotal;
        }

        public InningTotalDetails HighestTotal { get; set; }

        public InningTotalDetails LowestTotal { get; set; }
    }

    public class InningTotalDetails
    {
        public InningTotalDetails(
            Guid matchUuid,
            int runs,
            int wickets,
            Over overs)
        {
            MatchUuid = matchUuid;
            Runs = runs;
            Wickets = wickets;
            Overs = overs;
        }

        public Guid MatchUuid { get; set; }

        public int Runs { get; set; }

        public int Wickets { get; set; }

        public Over Overs { get; set; }
    }
}