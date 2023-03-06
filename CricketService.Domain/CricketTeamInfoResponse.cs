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
            TeamResultDetails t20IResults,
            TeamResultDetails oDIResults,
            TeamResultDetails testResults)
        {
            T20IResults = t20IResults;
            ODIResults = oDIResults;
            TestResults = testResults;
        }

        public TeamResultDetails T20IResults { get; set; }

        public TeamResultDetails ODIResults { get; set; }

        public TeamResultDetails TestResults { get; set; }
    }

    public class TeamResultDetails
    {
        public TeamResultDetails(
            DebutDetails debut,
            int matches,
            int won,
            int lost,
            int tiedAndWon,
            int tiedAndLost,
            int noResult,
            TeamMileStones teamMileStones)
        {
            Debut = debut;
            Matches = matches;
            Won = won;
            Lost = lost;
            TiedAndWon = tiedAndWon;
            TiedAndLost = tiedAndLost;
            NoResult = noResult;
            TeamMileStones = teamMileStones;
        }

        public DebutDetails Debut { get; set; }

        public int Matches { get; set; }

        public int Won { get; set; }

        public int Lost { get; set; }

        public int TiedAndWon { get; set; }

        public int TiedAndLost { get; set; }

        public int NoResult { get; set; }

        public double WinPercentage
        {
            get
            {
                if (Matches == 0)
                {
                    return 0;
                }

                return ((Won + (0.5 * TiedAndWon)) / Matches) * 100;
            }
        }

        public TeamMileStones TeamMileStones { get; set; }
    }

    public class DebutDetails
    {
        public DebutDetails(
            Guid matchUuid,
            DateTime date,
            string opponent,
            string venue,
            string result,
            string matchNo)
        {
            MatchUuid = matchUuid;
            Date = date.ToShortDateString();
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
            int careerCenturies,
            int careerHalfCenturies,
            int careerOneAndHalfCenturies,
            int careerDoubleCenturies,
            InningRecordDetails inningRecordDetails,
            KeyValuePair<string, int> mostRuns,
            KeyValuePair<string, int> mostWickets,
            KeyValuePair<string, int> mostSixes,
            KeyValuePair<string, int> mostFours,
            KeyValuePair<string, int> most50s,
            KeyValuePair<string, int> most100s,
            KeyValuePair<string, int> most150s,
            KeyValuePair<string, int> most200s,
            KeyValuePair<string, int> hiScore)
        {
            CareerRuns = careerRuns;
            CareerCenturies = careerCenturies;
            CareerHalfCenturies = careerHalfCenturies;
            CareerOneAndHalfCenturies = careerOneAndHalfCenturies;
            CareerDoubleCenturies = careerDoubleCenturies;
            InningRecordDetails = inningRecordDetails;
            MostRuns = mostRuns;
            MostWickets = mostWickets;
            MostSixes = mostSixes;
            MostFours = mostFours;
            Most50s = most50s;
            Most100s = most100s;
            Most150s = most150s;
            Most200s = most200s;
            HighestIndividualScore = hiScore;
        }

        public int CareerRuns { get; set; }

        public int CareerCenturies { get; set; }

        public int CareerHalfCenturies { get; set; }

        public int CareerOneAndHalfCenturies { get; set; }

        public int CareerDoubleCenturies { get; set; }

        public InningRecordDetails InningRecordDetails { get; set; }

        public KeyValuePair<string, int> MostRuns { get; set; }

        public KeyValuePair<string, int> MostWickets { get; set; }

        public KeyValuePair<string, int> MostSixes { get; set; }

        public KeyValuePair<string, int> MostFours { get; set; }

        public KeyValuePair<string, int> Most50s { get; set; }

        public KeyValuePair<string, int> Most100s { get; set; }

        public KeyValuePair<string, int> Most150s { get; set; }

        public KeyValuePair<string, int> Most200s { get; set; }

        public KeyValuePair<string, int> HighestIndividualScore { get; set; }
    }

    public class InningRecordDetails
    {
        public InningRecordDetails(
            InningTotal highestTotal,
            InningTotal lowestTotal)
        {
            HighestTotal = highestTotal;
            LowestTotal = lowestTotal;
        }

        public InningTotal HighestTotal { get; set; }

        public InningTotal LowestTotal { get; set; }
    }

    public class InningTotal
    {
        public InningTotal(
            int runs,
            int wickets,
            Over overs)
        {
            Runs = runs;
            Wickets = wickets;
            Overs = overs;
        }

        public int Runs { get; set; }

        public int Wickets { get; set; }

        public Over Overs { get; set; }
    }
}