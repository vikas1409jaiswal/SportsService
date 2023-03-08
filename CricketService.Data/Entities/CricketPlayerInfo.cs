using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Column("date_of_birth")]
        public string DateOfBirth { get; set; } = string.Empty;

        [Column("birth_place")]
        public string BirthPlace { get; set; } = string.Empty;

        [Column("formats")]
        public ICollection<string> Formats { get; set; } = new List<string>();

        [Column("batting_style")]
        public string BattingStyle { get; set; } = string.Empty;

        [Column("bowling_style")]
        public string BowlingStyle { get; set; } = string.Empty;

        [Column("playing_role")]
        public string PlayingRole { get; set; } = string.Empty;

        [Column("height")]
        public string Height { get; set; } = string.Empty;

        [Column("image_src")]
        public string ImageSrc { get; set; } = string.Empty;

        [Column("contents")]
        public string[] Content { get; set; } = Array.Empty<string>();
    }
}
