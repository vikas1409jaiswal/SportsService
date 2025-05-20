using CricketService.Domain.Enums;

namespace CricketService.Domain.Common
{
    public class MatchesFilters
    {
        public MatchesFilters(
            CricketFormat format,
            Guid teamUuid,
            Guid oppositionTeamUuid)
        {
            Format = format;
            TeamUuid = teamUuid;
            OppositionTeamUuid = oppositionTeamUuid;
        }

        public CricketFormat Format { get; }

        public Guid TeamUuid { get; }

        public Guid OppositionTeamUuid { get; }
    }
}
