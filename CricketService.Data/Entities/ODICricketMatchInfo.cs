using CricketService.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CricketService.Data.Entities
{
    [Table("one_day_international_matches")]
    public class ODICricketMatchInfo
    {
        [Key]
        [Column("uuid")]
        public Guid Uuid { get; set; }

        [Column("season")]
        public string Season { get; set; } = string.Empty;

        [Column("series")]
        public string Series { get; set; } = string.Empty;

        [Column("player_of_the_match")]
        public string PlayerOfTheMatch { get; set; } = string.Empty;

        [Column("match_no")]
        public string MatchNo { get; set; } = string.Empty;

        [Column("match_days")]
        public string MatchDays { get; set; } = string.Empty;

        [Column("match_title")]
        public string MatchTitle { get; set; } = string.Empty;

        [Column("venue")]
        public string Venue { get; set; } = string.Empty;

        [Column("match_date")]
        public string MatchDate { get; set; } = string.Empty;

        [Column("toss_winner")]
        public string TossWinner { get; set; } = string.Empty;

        [Column("toss_decision")]
        public string TossDecision { get; set; } = string.Empty;

        [Column("result")]
        public string Result { get; set; } = string.Empty;

        [Column("team1_details")]
        public TeamScoreDetailsRequest Team1 { get; set; } = null!;

        [Column("team2_details")]
        public TeamScoreDetailsRequest Team2 { get; set; } = null!;

        [Column("tv_umpire")]
        public string TvUmpire { get; set; } = string.Empty;

        [Column("match_referee")]
        public string MatchReferee { get; set; } = string.Empty;

        [Column("reserve_umpire")]
        public string ReserveUmpire { get; set; } = string.Empty;

        [Column("umpires")]
        public string[] Umpires { get; set; } = Array.Empty<string>();

        [Column("format_debut")]
        public string[] FormatDebut { get; set; } = Array.Empty<string>();

        [Column("international_debut")]
        public string[] InternationalDebut { get; set; } = Array.Empty<string>();
    }
}
