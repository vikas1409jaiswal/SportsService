using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CricketService.Data.Entities
{
    [Table("cricket_teams_info")]
    public class CricketTeamInfoDTO
    {
        [Key]
        [Column("uuid")]
        public Guid Uuid { get; set; } = Guid.Empty;

        [Column("team_name")]
        public string TeamName { get; set; } = string.Empty;

        [Column("formats")]
        public ICollection<string> Formats { get; set; } = new List<string>();

        [Column("country")]
        public string Country { get; set; } = string.Empty;

        [Column("logo_url")]
        public string LogoUrl { get; set; } = string.Empty;

        [Column("flag_url")]
        public string FlagUrl { get; set; } = string.Empty;

        public ICollection<CricketTeamPlayerInfos> TeamsPlayersInfos { get; set; } = null!;
    }
}
