namespace CricketService.Domain
{
    public class CricketMatchInfo
    {
        public CricketMatchInfo(
          string season,
          string series,
          string playerOfTheMatch,
          string matchNo,
          string matchDays,
          string matchTitle,
          string venue,
          string matchDate,
          string tossWinner,
          string tossDecision,
          string result,
          string tvUmpire,
          string matchReferee,
          string reserveUmpire,
          string[] umpires,
          string[] formatDebut,
          string[] internationalDebut)
        {
            Season = season;
            Series = series;
            PlayerOfTheMatch = playerOfTheMatch;
            MatchNo = matchNo;
            MatchDays = matchDays;
            MatchTitle = matchTitle;
            Venue = venue;
            MatchDate = matchDate;
            TossWinner = tossWinner;
            TossDecision = tossDecision;
            Result = result;
            TvUmpire = tvUmpire;
            MatchReferee = matchReferee;
            ReserveUmpire = reserveUmpire;
            Umpires = umpires;
            FormatDebut = formatDebut;
            InternationalDebut = internationalDebut;
        }

        public string Season { get; set; }

        public string Series { get; set; } = string.Empty;

        public string PlayerOfTheMatch { get; set; } = string.Empty;

        public string MatchNo { get; set; } = string.Empty;

        public string MatchDays { get; set; } = string.Empty;

        public string MatchTitle { get; set; } = string.Empty;

        public string Venue { get; set; } = string.Empty;

        public string MatchDate { get; set; } = string.Empty;

        public string TossWinner { get; set; } = string.Empty;

        public string TossDecision { get; set; } = string.Empty;

        public string Result { get; set; } = string.Empty;

        public string TvUmpire { get; set; } = string.Empty;

        public string MatchReferee { get; set; } = string.Empty;

        public string ReserveUmpire { get; set; } = string.Empty;

        public string[] Umpires { get; set; }

        public string[] FormatDebut { get; set; }

        public string[] InternationalDebut { get; set; }
    }
}
