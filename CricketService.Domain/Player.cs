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
            string birthPlace,
            DebutDetailsInfo debutDetailsInfo,
            CareerDetailsInfo careerInfo,
            string dateOfDeath,
            ICollection<string> internationalFormats,
            string teamNames,
            PlayerExtraInfo extraInfo)
        {
            Uuid = uuid;
            FullName = fullName;
            PlayerUrl = playerUrl;
            DateOfBirth = dateOfBirth;
            DebutDetails = debutDetailsInfo;
            CareerStatistics= careerInfo;
            DateOfDeath = dateOfDeath;
            BirthPlace = birthPlace;
            InternationalFormats = internationalFormats;
            TeamNames = teamNames.Split(", ");
            ExtraInfo = extraInfo;
        }

        public Guid Uuid { get; set; }

        public string FullName { get; set; }

        public string PlayerUrl { get; set; }

        public string DateOfBirth { get; set; }

        public string DateOfDeath { get; set; }

        public DebutDetailsInfo DebutDetails { get; set; }

        public CareerDetailsInfo CareerStatistics { get; set; }

        public string BirthPlace { get; set; }

        public ICollection<string> InternationalFormats { get; set; }

        public string[] TeamNames { get; set; } = Array.Empty<string>();

        public PlayerExtraInfo ExtraInfo { get; set; }
    }
}
