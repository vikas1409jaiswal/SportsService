namespace CricketService.Domain.BaseDomains
{
    public class InternationalCricketMatch : CricketMatchBase
    {
        public InternationalCricketMatch(
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
          List<CricketPlayer> internationalDebut,
          PlayerOfTheMatch? playerOfTheMatch = null)
          : base(
            matchUuid,
            season,
            series,
            seriesResult,
            matchNumber,
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
            InternationalDebut = internationalDebut;
        }

        public List<CricketPlayer> InternationalDebut { get; set; }
    }
}
