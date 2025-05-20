using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CricketService.Domain;
using CricketService.Domain.Common;

namespace CricketService.Data.Entities
{
    [Table("cricket_players_info")]
    public class CricketPlayerInfoDTO
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

        [Column("image_url")]
        public string ImageUrl { get; set; } = string.Empty;

        [Column("international_team_names")]
        public ICollection<string> InternationalTeamNames { get; set; } = new List<string>();

        [Column("team_names")]
        public string TeamNames { get; set; } = string.Empty;

        [Column("date_of_birth")]
        public DateOfEvent? DateOfBirth { get; set; }

        [Column("date_of_death")]
        public DateOfEvent? DateOfDeath { get; set; }

        [Column("debut_details")]
        public DebutDetailsInfo DebutDetails { get; set; } = null!;

        [Column("formats")]
        public ICollection<string> Formats { get; set; } = new List<string>();

        [Column("extra_info")]
        public PlayerExtraInfo ExtraInfo { get; set; } = null!;

        [Column("contents")]
        public string[] Contents { get; set; } = Array.Empty<string>();

        public ICollection<CricketTeamPlayerInfos> TeamsPlayersInfos { get; set; } = null!;
    }
}
