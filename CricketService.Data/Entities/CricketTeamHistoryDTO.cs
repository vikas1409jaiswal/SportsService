using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CricketService.Domain;

namespace CricketService.Data.Entities
{
    [Table("cricket_teams_history")]
    public class CricketTeamHistoryDTO
    {
        [Key]
        [Column("match_uuid")]
        public Guid MatchUuid { get; set; } = Guid.Empty;

        [Key]
        [Column("match_number")]
        public int MatchNumber { get; set; }

        [Column("instant_teams_records")]
        public List<TeamFormatRecords> InstantTeamsRecords { get; set; } = new List<TeamFormatRecords>();

        [Column("format")]
        public string Format { get; set; } = string.Empty;
    }

    public class TeamFormatRecords
    {
        public Guid TeamUuid { get; set; } = Guid.Empty;

        public string TeamName { get; set; } = string.Empty;

        public TeamFormatRecordDetails TeamFormatRecordDetails { get; set; } = null!;
    }
}