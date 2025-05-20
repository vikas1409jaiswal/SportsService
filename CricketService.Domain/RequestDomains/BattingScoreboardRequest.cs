using CricketService.Domain.BaseDomains;

namespace CricketService.Domain.RequestDomains
{
    public class BattingScoreboardRequest
    {
        public BattingScoreboardRequest(
            CricketPlayer playerName,
            string outStatus,
            int? runsScored = null,
            int? ballsFaced = null,
            int? fours = null,
            int? sixes = null,
            int? minutes = null)
        {
            PlayerName = playerName;
            OutStatus = outStatus;
            RunsScored = runsScored;
            BallsFaced = ballsFaced;
            Minutes = minutes;
            Fours = fours;
            Sixes = sixes;
        }

        public CricketPlayer PlayerName { get; set; }

        public string OutStatus { get; set; } = string.Empty;

        public int? RunsScored { get; set; } = null!;

        public int? BallsFaced { get; set; } = null!;

        public int? Minutes { get; set; } = null!;

        public int? Fours { get; set; } = null!;

        public int? Sixes { get; set; } = null!;
    }
}
