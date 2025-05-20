using CricketService.Domain.BaseDomains;
using CricketService.Domain.Common;

namespace CricketService.Domain
{
    public class PlayerDetails
    {
        public PlayerDetails(
            Guid uuid,
            string fullName,
            string playerUrl,
            CricketTeam[] cricketTeams,
            string dateOfBirth,
            string birthPlace,
            DebutDetailsInfo debutDetailsInfo,
            CareerDetailsInfo[] careerInfos,
            string dateOfDeath,
            ICollection<string> internationalFormats,
            string teamNames,
            PlayerExtraInfo extraInfo,
            string[] content)
        {
            Uuid = uuid;
            FullName = fullName;
            PlayerUrl = playerUrl;
            Teams = cricketTeams;
            DateOfBirth = dateOfBirth;
            DebutDetails = debutDetailsInfo;
            CareerStatistics = careerInfos;
            DateOfDeath = dateOfDeath;
            BirthPlace = birthPlace;
            InternationalFormats = internationalFormats;
            TeamNames = teamNames.Split(", ");
            ExtraInfo = extraInfo;
            Content = content;
        }

        public Guid Uuid { get; set; }

        public string FullName { get; set; }

        public string PlayerUrl { get; set; }

        public string DateOfBirth { get; set; }

        public string DateOfDeath { get; set; }

        public CricketTeam[] Teams { get; set; }

        public DebutDetailsInfo DebutDetails { get; set; }

        public CareerDetailsInfo[] CareerStatistics { get; set; }

        public string BirthPlace { get; set; }

        public ICollection<string> InternationalFormats { get; set; }

        public string[] TeamNames { get; set; } = Array.Empty<string>();

        public PlayerExtraInfo ExtraInfo { get; set; }

        public string[] Content { get; set; } = Array.Empty<string>();
    }
}
