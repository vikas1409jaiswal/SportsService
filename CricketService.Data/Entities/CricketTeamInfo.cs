using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CricketService.Domain.Enums;

namespace CricketService.Data.Entities
{
        [Table("cricket_teams_info")]
        public class CricketTeamInfo
        {
            [Key]
            [Column("uuid")]
            public Guid Uuid { get; set; } = Guid.NewGuid();

            [Column("team_name")]
            public string TeamName { get; set; } = string.Empty;

            [Column("formats")]
            public IEnumerable<CricketFormat> Formats { get; set; } = new List<CricketFormat>();

            [Column("logo_url")]
            public string LogoUrl { get; set; } = string.Empty;

            [Column("flag_url")]
            public string FlagUrl { get; set; } = string.Empty;
        }
}
