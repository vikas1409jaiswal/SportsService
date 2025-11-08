namespace CricketService.Domain.BaseDomains
{
    public class InningScoreboard<TBatting, TBowling>
     where TBatting : class
     where TBowling : class
    {
        public InningScoreboard(
            string title,
            ICollection<TBatting> battingScoreboard,
            ICollection<TBowling> bowlingScoreboard,
            string extras,
            string[] fallOfWickets,
            CricketPlayer[] didNotBat)
        {
            Title = title;
            BattingScorecard = battingScoreboard;
            BowlingScorecard = bowlingScoreboard;
            Extras = extras;
            FallOfWickets = fallOfWickets;
            DidNotBat = didNotBat;
        }

        public string Title { get; set; }

        public ICollection<TBatting> BattingScorecard { get; set; }

        public ICollection<TBowling> BowlingScorecard { get; set; }

        public string Extras { get; set; } = string.Empty;

        public string[] FallOfWickets { get; set; }

        public CricketPlayer[] DidNotBat { get; set; }
    }
}
