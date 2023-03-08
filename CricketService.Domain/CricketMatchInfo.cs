using CricketService.Domain.Common;

namespace CricketService.Domain
{
    public class CricketMatchInfo
    {
        private const string Source = "CricketService.Domain.CricketMatchInfo";

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
            Season = ModelValidationPrecondition.IsNotNullOrWhitespace(season, nameof(season), Source);
            Series = ModelValidationPrecondition.IsNotNullOrWhitespace(series, nameof(series), Source);
            PlayerOfTheMatch = playerOfTheMatch;
            MatchNo = ModelValidationPrecondition.IsNotNullOrWhitespace(matchNo, nameof(matchNo), Source);
            MatchDays = matchDays;
            MatchTitle = ModelValidationPrecondition.IsNotNullOrWhitespace(matchTitle, nameof(matchTitle), Source);
            Venue = ModelValidationPrecondition.IsNotNullOrWhitespace(venue, nameof(venue), Source);
            MatchDate = ModelValidationPrecondition.IsNotNullOrWhitespace(matchDate, nameof(matchDate), Source);
            TossWinner = tossWinner;
            TossDecision = tossDecision;
            Result = ModelValidationPrecondition.IsNotNullOrWhitespace(result, nameof(result), Source);
            TvUmpire = tvUmpire;
            MatchReferee = matchReferee;
            ReserveUmpire = reserveUmpire;
            Umpires = umpires;
            FormatDebut = formatDebut;
            InternationalDebut = internationalDebut;
        }

        public string Season { get; set; }

        public string Series { get; set; }

        public string PlayerOfTheMatch { get; set; }

        public string MatchNo { get; set; }

        public string MatchDays { get; set; }

        public string MatchTitle { get; set; }

        public string Venue { get; set; }

        public string MatchDate { get; set; }

        public string TossWinner { get; set; }

        public string TossDecision { get; set; }

        public string Result { get; set; }

        public string TvUmpire { get; set; }

        public string MatchReferee { get; set; }

        public string ReserveUmpire { get; set; }

        public string[] Umpires { get; set; }

        public string[] FormatDebut { get; set; }

        public string[] InternationalDebut { get; set; }
    }
}
