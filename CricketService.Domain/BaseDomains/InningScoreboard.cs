namespace CricketService.Domain.BaseDomains
{
    public class InningScoreboard<TBatting, TBowling>
     where TBatting : class
     where TBowling : class
    {
        public InningScoreboard(
            ICollection<TBatting> battingScoreboard,
            ICollection<TBowling> bowlingScoreboard,
            string extras,
            string[] fallOfWickets,
            CricketPlayer[] didNotBat)
        {
            BattingScorecboard = battingScoreboard;
            BowlingScoreboard = bowlingScoreboard;
            Extras = extras;
            FallOfWickets = fallOfWickets;
            DidNotBat = didNotBat;
        }

        public ICollection<TBatting> BattingScorecboard { get; set; }

        public ICollection<TBowling> BowlingScoreboard { get; set; }

        public string Extras { get; set; } = string.Empty;

        public string[] FallOfWickets { get; set; }

        public CricketPlayer[] DidNotBat { get; set; }
    }
}
