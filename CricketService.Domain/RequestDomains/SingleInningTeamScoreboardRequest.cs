using CricketService.Domain.BaseDomains;

namespace CricketService.Domain.RequestDomains
{
    public class SingleInningTeamScoreboardRequest
    {
        public SingleInningTeamScoreboardRequest(
            CricketTeam team,
            ICollection<BattingScoreboardRequest> battingScorecard,
            ICollection<BowlingScoreboardRequest> bowlingScorecard,
            string extras,
            string[] fallOfWickets,
            CricketPlayer[] didNotBat)
        {
            Team = team;
            BattingScorecard = battingScorecard;
            BowlingScorecard = bowlingScorecard;
            Extras = extras;
            FallOfWickets = fallOfWickets;
            DidNotBat = didNotBat;
        }

        public CricketTeam Team { get; set; }

        public ICollection<BattingScoreboardRequest> BattingScorecard { get; set; }

        public ICollection<BowlingScoreboardRequest> BowlingScorecard { get; set; }

        public string Extras { get; set; } = string.Empty;

        public string[] FallOfWickets { get; set; }

        public CricketPlayer[] DidNotBat { get; set; }
    }
}
