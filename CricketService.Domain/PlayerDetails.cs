using CricketService.Domain.BaseDomains;
using CricketService.Domain.Common;

namespace CricketService.Domain
{
    public class PlayerDetails
    {
        public PlayerDetails(
            Guid uuid,
            string fullName,
            string playerHref,
            CricketTeam[] cricketTeams,
            string dateOfBirth,
            string birthPlace,
            DebutDetailsInfo debutDetailsInfo,
            CareerDetailsInfo[] careerInfos,
            string dateOfDeath,
            ICollection<string> internationalFormats,
            string teamNames,
            PlayerExtraInfo extraInfo,
            string[] content,
            PlayerOverallStats playerOverallStats)
        {
            Uuid = uuid;
            FullName = fullName;
            PlayerHref = playerHref;
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
            OverallStats = playerOverallStats;
        }

        public Guid Uuid { get; set; }

        public string FullName { get; set; }

        public string PlayerHref { get; set; }

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

        public PlayerOverallStats OverallStats { get; set; }
    }

    public class PlayerOverallStats
    {
        public PlayerFormatStats PlayerODIStats { get; set; }

        public PlayerFormatStats PlayerT20IStats { get; set; }
    }

    public class PlayerFormatStats 
    {
        public IEnumerable<BattingInningsStat> BattingInningsStats { get; set; } = Enumerable.Empty<BattingInningsStat>();

        public BattingStatistics BattingOverallStats { get; set; }
    }
}
