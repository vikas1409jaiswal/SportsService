using CricketService.Domain.BaseDomains;
using CricketService.Domain.RequestDomains;

namespace CricketService.Domain.ResponseDomains
{
    public class BattingScoreboardResponse : BattingScoreboardRequest
    {
        public BattingScoreboardResponse(
            CricketPlayer playerName,
            string outStatus,
            int? runsScored = null,
            int? ballsFaced = null,
            int? fours = null,
            int? sixes = null,
            int? minutes = null)
            : base(
              playerName,
              outStatus,
              runsScored,
              ballsFaced,
              fours,
              sixes,
              minutes)
        {
        }

        public double StrikeRate
        {
            get
            {
                if (BallsFaced == 0 || BallsFaced is null)
                {
                    return 0;
                }

                var strikeRate = (double)(RunsScored! * 100) / BallsFaced;
                return (double)strikeRate!;
            }
        }
    }
}
