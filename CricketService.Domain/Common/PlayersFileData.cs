namespace CricketService.Domain.Common
{
    public class PlayersFileData
    {
        public Guid PlayerUuid { get; set; } = Guid.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Href { get; set; } = string.Empty;

        public string[] TeamNames { get; set; } = Array.Empty<string>();

        public string Birth { get; set; } = string.Empty;

        public string BattingStyle { get; set; } = string.Empty;

        public string BowlingStyle { get; set; } = string.Empty;

        public string PlayingRole { get; set; } = string.Empty;

        public string Height { get; set; } = string.Empty;

        public string ImageSrc { get; set; } = string.Empty;

        public string Education { get; set; } = string.Empty;
    }
}
