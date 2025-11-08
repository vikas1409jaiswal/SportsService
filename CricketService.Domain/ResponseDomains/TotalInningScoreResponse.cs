using CricketService.Domain.Common;

namespace CricketService.Domain.ResponseDomains
{
    public class TotalInningScoreResponse
    {
        public TotalInningScoreResponse(
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
}
