using System.ComponentModel.DataAnnotations.Schema;
using CricketService.Domain.BaseDomains;
using CricketService.Domain.ResponseDomains;

namespace CricketService.Data.Entities
{
    [Table("test_cricket_matches")]
    public class TestCricketMatchInfoDTO : CricketMatchInfoBaseDTO
    {
        [Column("team1_details")]
        public DoubleInningTeamScoreboardResponse Team1 { get; set; } = null!;

        [Column("team2_details")]
        public DoubleInningTeamScoreboardResponse Team2 { get; set; } = null!;

        [Column("tv_umpire")]
        public string TvUmpire { get; set; } = string.Empty;

        [Column("match_referee")]
        public string MatchReferee { get; set; } = string.Empty;

        [Column("reserve_umpire")]
        public string ReserveUmpire { get; set; } = string.Empty;

        [Column("umpires")]
        public string[] Umpires { get; set; } = Array.Empty<string>();

        [Column("international_debut")]
        public List<CricketPlayer> InternationalDebut { get; set; } = new List<CricketPlayer>();
    }
}
