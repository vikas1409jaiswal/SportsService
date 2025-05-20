using CricketService.Domain.BaseDomains;
using CricketService.Domain.Common;

namespace CricketService.Domain.ResponseDomains
{
    public class SingleInningTeamScoreboardResponse
    {
        public SingleInningTeamScoreboardResponse(
            CricketTeam team,
            ICollection<BattingScoreboardResponse> battingScoreCard,
            ICollection<BowlingScoreboardResponse> bowlingScoreCard,
            string extras,
            string[] fallOfWickets,
            CricketPlayer[] didNotBat)
        {
            Team = team;
            BattingScoreCard = battingScoreCard;
            BowlingScoreCard = bowlingScoreCard;
            Extras = extras;
            FallOfWickets = fallOfWickets;
            DidNotBat = didNotBat;
            SetTotalInningScore();
        }

        public CricketTeam Team { get; set; }

        public ICollection<BattingScoreboardResponse> BattingScoreCard { get; set; }

        public ICollection<BowlingScoreboardResponse> BowlingScoreCard { get; set; }

        public string Extras { get; set; } = string.Empty;

        public string[] FallOfWickets { get; set; }

        public CricketPlayer[] DidNotBat { get; set; }

        public CricketPlayer[] Playing11
        {
            get
            {
                var playing11 = BattingScoreCard.Select(x => x.PlayerName).Concat(DidNotBat).ToArray();
                if (playing11.Length == 0)
                {
                    return null!;
                }

                if (playing11.Length < 10)
                {
                    throw new FormatException("playing 11 should contain exact 11 players.");
                }

                return playing11;
            }
        }

        public TotalInningScore TotalInningDetails { get; set; }

        public void SetTotalInningScore()
        {
            TotalInningDetails = new TotalInningScore(
                                (int)BattingScoreCard.Sum(x => x.RunsScored)!,
                                BattingScoreCard.Count(x => !x.OutStatus.Contains("not out")),
                                BowlingScoreCard.Sum(x => new Over(x.OversBowled).Balls).ToOvers(),
                                Extras);
        }
    }
}
