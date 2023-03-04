using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CricketService.Domain;
using CricketService.Domain.Enums;

namespace CricketService.Data.Entities
{
        [Table("cricket_players_info")]
        public class CricketPlayerInfo
        {
            [Key]
            [Column("uuid")]
            public Guid Uuid { get; set; } = Guid.NewGuid();

            [Column("full_name")]
            public string FullName { get; set; } = string.Empty;

            [Column("href")]
            public string Href { get; set; } = string.Empty;

            [Column("team_name")]
            public string TeamName { get; set; } = string.Empty;

            [Column("date_of_birth")]
            public DateTime DateOfBirth { get; set; } = default(DateTime);

            [Column("birth_place")]
            public string BirthPlace { get; set; } = string.Empty;

            [Column("formats")]
            public IEnumerable<CricketFormat> Formats { get; set; } = new List<CricketFormat>();
        }
}
