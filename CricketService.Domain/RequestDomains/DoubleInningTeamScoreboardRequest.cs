using CricketService.Domain.BaseDomains;

namespace CricketService.Domain.RequestDomains
{
    public class DoubleInningTeamScoreboardRequest
    {
        public DoubleInningTeamScoreboardRequest(
            CricketTeam team,
            InningScoreboard<BattingScoreboardRequest, BowlingScoreboardRequest> inning1,
            InningScoreboard<BattingScoreboardRequest, BowlingScoreboardRequest> inning2)
        {
            Team = team;
            Inning1 = inning1;
            Inning2 = inning2;
        }

        public CricketTeam Team { get; set; }

        public InningScoreboard<BattingScoreboardRequest, BowlingScoreboardRequest> Inning1 { get; set; }

        public InningScoreboard<BattingScoreboardRequest, BowlingScoreboardRequest> Inning2 { get; set; }
    }
}
