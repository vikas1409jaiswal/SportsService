using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CricketService.Data.Entities
{
    [Table("cricket_teams_history_h2h")]
    public class CricketTeamHistoryH2hDTO
    {
        [Key]
        [Column("match_uuid")]
        public Guid MatchUuid { get; set; } = Guid.Empty;

        [Column("team1_name")]
        public string Team1Name { get; set; } = string.Empty;

        [Column("team2_name")]
        public string Team2Name { get; set; } = string.Empty;

        [Key]
        [Column("match_number")]
        public int MatchNumber { get; set; }

        [Column("instant_teams_records")]
        public List<TeamFormatRecords> InstantTeamsRecords { get; set; } = new List<TeamFormatRecords>();

        [Column("format")]
        public string Format { get; set; } = string.Empty;
    }
}
