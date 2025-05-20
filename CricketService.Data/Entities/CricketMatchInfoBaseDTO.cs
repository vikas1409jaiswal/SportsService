using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CricketService.Domain.BaseDomains;

namespace CricketService.Data.Entities
{
    public class CricketMatchInfoBaseDTO
    {
        [Key]
        [Column("uuid")]
        public Guid Uuid { get; set; }

        [Column("match_number")]
        public string MatchNumber { get; set; } = string.Empty;

        [Column("match_type")]
        public string MatchType { get; set; } = string.Empty;

        [Column("match_date")]
        public string MatchDate { get; set; } = string.Empty;

        [Column("season")]
        public string Season { get; set; } = string.Empty;

        [Column("series")]
        public string Series { get; set; } = string.Empty;

        [Column("series_result")]
        public string? SeriesResult { get; set; }

        [Column("match_title")]
        public string MatchTitle { get; set; } = string.Empty;

        [Column("player_of_the_match")]
        public PlayerOfTheMatch? PlayerOfTheMatch { get; set; } = null!;

        [Column("toss_winner")]
        public string TossWinner { get; set; } = string.Empty;

        [Column("toss_decision")]
        public string TossDecision { get; set; } = string.Empty;

        [Column("result")]
        public string Result { get; set; } = string.Empty;

        [Column("venue")]
        public string Venue { get; set; } = string.Empty;
    }
}
