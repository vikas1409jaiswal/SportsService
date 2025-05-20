using CricketService.Domain.BaseDomains;

namespace CricketService.Domain.RequestDomains
{
    public class BowlingScoreboardRequest
    {
        public BowlingScoreboardRequest(
            CricketPlayer playerName,
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

        public CricketPlayer PlayerName { get; set; }

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
}
