using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public ICollection<string> Formats { get; set; } = new List<string>();

        [Column("logo_url")]
        public string LogoUrl { get; set; } = string.Empty;

        [Column("flag_url")]
        public string FlagUrl { get; set; } = string.Empty;
    }
}
