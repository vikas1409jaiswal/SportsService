namespace CricketService.Domain.Common
{
    public class PlayersFileData
    {
        public Guid PlayerUuid { get; set; } = Guid.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Href { get; set; } = string.Empty;

        public string[] TeamNames { get; set; } = Array.Empty<string>();

        public string Birth { get; set; } = string.Empty;

        public string Died { get; set; } = string.Empty;

        public DebutDetailsInfo DebutDetails { get; set; }

        public string BattingStyle { get; set; } = string.Empty;

        public string BowlingStyle { get; set; } = string.Empty;

        public string PlayingRole { get; set; } = string.Empty;

        public string Height { get; set; } = string.Empty;

        public string ImageSrc { get; set; } = string.Empty;

        public string Education { get; set; } = string.Empty;

        public string[] Content { get; set; } = Array.Empty<string>();
    }

    public class DebutDetailsInfo
    {
        public DebutDetailsInfo(
            DebutInfo t20IMatches,
            DebutInfo oDIMatches,
            DebutInfo testMatches)
        {
            T20IMatches = t20IMatches;
            ODIMatches = oDIMatches;
            TestMatches = testMatches;
        }

        public DebutInfo T20IMatches { get; set; }

        public DebutInfo ODIMatches { get; set; }

        public DebutInfo TestMatches { get; set; }
    }

    public class DebutInfo
    {
        public DebutInfo(
            string first,
            string last)
        {
            First = first;
            Last = last;
        }

        public string First { get; set; }

        public string Last { get; set; }
    }
}
