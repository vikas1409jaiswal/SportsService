namespace CricketService.Domain.Common
{
    public class PlayerExtraInfo
    {
        public string BattingStyle { get; set; } = string.Empty;

        public string BowlingStyle { get; set; } = string.Empty;

        public string PlayingRole { get; set; } = string.Empty;

        public string Height { get; set; } = string.Empty;

        public string ImageSrc { get; set; } = string.Empty;

        public string[] Content { get; set; } = Array.Empty<string>();
    }
}
