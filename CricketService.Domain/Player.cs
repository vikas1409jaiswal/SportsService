using CricketService.Domain.Common;

namespace CricketService.Domain
{
    public class Player
    {
        public Player(
            string name,
            string href)
        {
            Name = name;
            Href = href;
        }

        public string Name { get; set; }

        public string Href { get; set; }
    }

    public class PlayerDetails
    {
        public PlayerDetails(
            Guid uuid,
            string fullName,
            string playerUrl,
            string dateOfBirth,
            DebutDetailsInfo debutDetailsInfo,
            string dateOfDeath,
            ICollection<string> internationalFormats,
            string battingStyle,
            string bowlingStyle,
            string playingRole,
            string height,
            string imageSrc,
            string teamNames)
        {
            Uuid = uuid;
            FullName = fullName;
            PlayerUrl = playerUrl;
            DateOfBirth = dateOfBirth;
            DebutDetails = debutDetailsInfo;
            DateOfDeath = dateOfDeath;
            BirthPlace = "chennai";
            InternationalFormats = internationalFormats;
            BattingStyle = battingStyle;
            BowlingStyle = bowlingStyle;
            PlayingRole = playingRole;
            Height = height;
            ImageSrc = imageSrc;
            TeamNames = teamNames.Split(", ");
        }

        public Guid Uuid { get; set; }

        public string FullName { get; set; }

        public string PlayerUrl { get; set; }

        public string DateOfBirth { get; set; }

        public string DateOfDeath { get; set; }

        public DebutDetailsInfo DebutDetails { get; set; }

        public string BirthPlace { get; set; }

        public ICollection<string> InternationalFormats { get; set; }

        public string BattingStyle { get; set; } = string.Empty;

        public string BowlingStyle { get; set; } = string.Empty;

        public string PlayingRole { get; set; } = string.Empty;

        public string Height { get; set; } = string.Empty;

        public string ImageSrc { get; set; } = string.Empty;

        public string[] TeamNames { get; set; } = Array.Empty<string>();
    }
}
