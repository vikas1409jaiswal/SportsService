namespace CricketService.Domain.BaseDomains
{
    public class DomesticCricketMatch : CricketMatchBase
    {
        public DomesticCricketMatch(
          Guid matchUuid,
          string season,
          string series,
          string seriesResult,
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
          List<CricketPlayer> formatDebut,
          PlayerOfTheMatch? playerOfTheMatch = null)
          : base(
            matchUuid,
            season,
            series,
            seriesResult,
            "Domestic no 1000",
            matchDate,
            matchDays,
            matchTitle,
            venue,
            tossWinner,
            tossDecision,
            result,
            tvUmpire,
            matchReferee,
            reserveUmpire,
            umpires,
            playerOfTheMatch)
        {
            FormatDebut = formatDebut;
        }

        public List<CricketPlayer> FormatDebut { get; set; }
    }
}
