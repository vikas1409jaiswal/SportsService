namespace CricketService.Domain.Common
{
    public static class OversExtension
    {
        public static Over ToOvers(this int balls)
        {
            var overs = balls / 6;
            var overballs = balls % 6;

            return new Over(overs + (overballs * 0.1));
        }
    }

    public class Over
    {
        public Over(double overs)
        {
            Balls = ToBalls(overs);
            Overs = overs;
        }

        public double Overs { get; set; }

        public int Balls { get; set; }

        private int ToBalls(double overs)
        {
            var fullOvers = 0;
            var remainderBalls = 0;
            if (overs.ToString().Contains("."))
            {
                var arr = overs.ToString().Split('.');
                fullOvers = Convert.ToInt32(arr[0]);
                remainderBalls = Convert.ToInt32(arr[1][0].ToString());
                if (remainderBalls > 7)
                {
                    throw new FormatException($"{overs} is not a valid over format.");
                }
            }
            else
            {
                fullOvers = (int)overs;
            }

            return (fullOvers * 6) + remainderBalls;
        }
    }
}
