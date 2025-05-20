using System.Text.Json.Serialization;
using CricketService.Domain.Attributes.ValidationAttributes;
using CricketService.Domain.Common;

namespace CricketService.Domain.BaseDomains
{   
    public class CricketMatchBase
    {
        private const string Source = "CricketService.Domain.BaseDomains.CricketMatchBase";

        public CricketMatchBase(
          Guid matchUuid,
          string season,
          string series,
          string seriesResult,
          string matchNumber,
          string matchDate,
          string matchDays,
          string matchTitle,
          string venue,
          string tossWinner,
          string tossDecision,
          string result,
          string tvUmpire,
          string matchReferee,
          string reserveUmpire,
          string[] umpires,
          PlayerOfTheMatch? playerOfTheMatch = null)
        {
            MatchUuid = matchUuid;
            Season = ModelValidationPrecondition.IsNotNullOrWhitespace(season, nameof(season), Source);
            Series = ModelValidationPrecondition.IsNotNullOrWhitespace(series, nameof(series), Source);
            SeriesResult = seriesResult;
            PlayerOfTheMatch = playerOfTheMatch;
            MatchNumber = ModelValidationPrecondition.IsNotNullOrWhitespace(matchNumber, nameof(matchNumber), Source);
            MatchDate = ModelValidationPrecondition.IsNotNullOrWhitespace(matchDate, nameof(matchDate), Source);
            MatchType = matchDays.Contains(" - ") ? matchDays.Split(" - ")[1] : matchDays;
            MatchTitle = ModelValidationPrecondition.IsNotNullOrWhitespace(matchTitle, nameof(matchTitle), Source);
            Venue = ModelValidationPrecondition.IsNotNullOrWhitespace(venue, nameof(venue), Source);
            TossWinner = tossWinner;
            TossDecision = tossDecision;
            Result = ModelValidationPrecondition.IsNotNullOrWhitespace(result, nameof(result), Source);
            TvUmpire = tvUmpire;
            MatchReferee = matchReferee;
            ReserveUmpire = reserveUmpire;
            Umpires = umpires;
        }

        [JsonPropertyOrder(-13)]
        public Guid MatchUuid { get; set; }

        [CricketSeason]
        [JsonPropertyOrder(-12)]
        public string Season { get; set; }

        [JsonPropertyOrder(-11)]
        public string Series { get; set; }

        [JsonPropertyOrder(-10)]
        public string SeriesResult { get; set; }

        [CricketMatchNumber]
        [JsonPropertyOrder(-9)]
        public string MatchNumber { get; set; }

        [JsonPropertyOrder(-8)]
        public string MatchType { get; set; }

        [JsonPropertyOrder(-7)]
        public string MatchTitle { get; set; }

        [JsonPropertyOrder(-6)]
        public string Venue { get; set; }

        [JsonPropertyOrder(-5)]
        public string MatchDate { get; set; }

        [JsonPropertyOrder(-4)]
        public string TossWinner { get; set; }

        [JsonPropertyOrder(-3)]
        public string TossDecision { get; set; }

        [JsonPropertyOrder(-2)]
        public string Result { get; set; }

        [JsonPropertyOrder(-1)]
        public PlayerOfTheMatch? PlayerOfTheMatch { get; set; }

        public string TvUmpire { get; set; }

        public string MatchReferee { get; set; }

        public string ReserveUmpire { get; set; }

        public string[] Umpires { get; set; }
    }
}
