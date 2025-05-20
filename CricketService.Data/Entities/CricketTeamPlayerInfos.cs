using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CricketService.Domain;

namespace CricketService.Data.Entities
{
    [Table("cricket_teams_and_players_info")]
    public class CricketTeamPlayerInfos
    {
        [Column("team_uuid")]
        public Guid TeamUuid { get; set; }

        [Column("player_uuid")]
        public Guid PlayerUuid { get; set; }

        [Column("team_name")]
        public string TeamName { get; set; } = string.Empty;

        [Column("player_name")]
        public string PlayerName { get; set; } = string.Empty;

        [Column("career_statistics")]
        public CareerDetailsInfo CareerStatistics { get; set; } = null!;

        public CricketTeamInfoDTO TeamInfo { get; set; } = null!;

        public CricketPlayerInfoDTO PlayerInfo { get; set; } = null!;
    }
}
