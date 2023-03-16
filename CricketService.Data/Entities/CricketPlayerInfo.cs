using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CricketService.Domain;
using CricketService.Domain.Common;

namespace CricketService.Data.Entities
{
    [Table("cricket_players_info")]
    public class CricketPlayerInfo
    {
        [Key]
        [Column("uuid")]
        public Guid Uuid { get; set; } = Guid.Empty;

        [Column("name")]
        public string PlayerName { get; set; } = string.Empty;

        [Column("full_name")]
        public string FullName { get; set; } = string.Empty;

        [Column("href")]
        public string Href { get; set; } = string.Empty;

        [Column("international_team_names")]
        public ICollection<string> InternationalTeamNames { get; set; } = new List<string>();

        [Column("team_names")]
        public string TeamNames { get; set; } = string.Empty;

        [Column("birth_info")]
        public string Birth { get; set; } = string.Empty;

        [Column("death_info")]
        public string Death { get; set; } = string.Empty;

        [Column("debut_details")]
        public DebutDetailsInfo DebutDetails { get; set; } = null!;

        [Column("formats")]
        public ICollection<string> Formats { get; set; } = new List<string>();

        [Column("career_statistics")]
        public CareerDetailsInfo CareerStatistics { get; set; } = null!;

        [Column("extra_info")]
        public PlayerExtraInfo ExtraInfo { get; set; } = null!;
    }
}
