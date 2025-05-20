using CricketService.Domain.BaseDomains;
using CricketService.Domain.Common;
using CricketService.Domain.RequestDomains;

namespace CricketService.Domain.ResponseDomains
{
    public class BowlingScoreboardResponse : BowlingScoreboardRequest
    {
        public BowlingScoreboardResponse(
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
            : base(
              playerName,
              oversBowled,
              maidens,
              runsConceded,
              wickets,
              wideBall,
              noBall,
              dots,
              fours,
              sixes)
        {
        }

        public double Economy
        {
            get
            {
                var balls = new Over(OversBowled).Balls;
                if (balls == 0)
                {
                    return 0;
                }

                var economy = (double)(RunsConceded * 6) / balls;
                return economy;
            }
        }
    }
}
