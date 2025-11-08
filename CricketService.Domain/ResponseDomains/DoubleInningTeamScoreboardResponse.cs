using CricketService.Domain.BaseDomains;
using CricketService.Domain.Common;

namespace CricketService.Domain.ResponseDomains
{
    public class DoubleInningTeamScoreboardResponse
    {
        public DoubleInningTeamScoreboardResponse(
            CricketTeam team,
            InningScoreboardResponse inning1,
            InningScoreboardResponse inning2)
        {
            Team = team;
            Inning1 = inning1;
            Inning2 = inning2;
        }

        public CricketTeam Team { get; set; }

        public InningScoreboardResponse Inning1 { get; set; }

        public InningScoreboardResponse Inning2 { get; set; }
    }

    public class InningScoreboardResponse : InningScoreboard<BattingScoreboardResponse, BowlingScoreboardResponse>
    {
        public InningScoreboardResponse(
            string title,
            ICollection<BattingScoreboardResponse> battingScoreCard,
            ICollection<BowlingScoreboardResponse> bowlingScoreCard,
            string extras,
            string[] fallOfWickets,
            CricketPlayer[] didNotBat)
            : base(
            title,
            battingScoreCard,
            bowlingScoreCard,
            extras,
            fallOfWickets,
            didNotBat)
        {
        }

        public CricketPlayer[] Playing11
        {
            get
            {
                var playing11 = BattingScorecard.Select(x => x.PlayerName).Concat(DidNotBat).ToArray();
                if (playing11.Length == 0)
                {
                    return null!;
                }

                if (playing11.Length < 11)
                {
                    throw new FormatException("playing 11 should contain exact 11 players.");
                }

                return playing11;
            }
        }

        public TotalInningScoreResponse TotalInningDetails { get; set; }


        public void SetTotalInningScore()
        {
            TotalInningDetails = new TotalInningScoreResponse(
                                (int)BattingScorecard.Sum(x => x.RunsScored)!,
                                BattingScorecard.Count(x => !x.OutStatus.Contains("not out")),
                                BowlingScorecard.Sum(x => new Over(x.OversBowled).Balls).ToOvers(),
                                Extras);
        }
    }
}
