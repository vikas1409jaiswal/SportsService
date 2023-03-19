using AutoMapper;
using CricketService.Domain;

namespace CricketService.Data.Extensions
{
    public static class EntityExtensions
    {
        public static CricketMatchInfoResponse ToDomain(this Entities.T20ICricketMatchInfo cricketMatchInfo, IMapper mapper)
        {
            if (cricketMatchInfo is null)
            {
                return null!;
            }

            return new CricketMatchInfoResponse(
                cricketMatchInfo.Uuid,
                cricketMatchInfo.Season,
                cricketMatchInfo.Series,
                cricketMatchInfo.PlayerOfTheMatch,
                cricketMatchInfo.MatchNo,
                cricketMatchInfo.MatchDays,
                cricketMatchInfo.MatchTitle,
                cricketMatchInfo.Venue,
                cricketMatchInfo.MatchDate,
                cricketMatchInfo.TossWinner,
                cricketMatchInfo.TossDecision,
                cricketMatchInfo.Result,
                mapper.Map<TeamScoreDetails>(cricketMatchInfo.Team1),
                mapper.Map<TeamScoreDetails>(cricketMatchInfo.Team2),
                cricketMatchInfo.TvUmpire,
                cricketMatchInfo.MatchReferee,
                cricketMatchInfo.ReserveUmpire,
                cricketMatchInfo.Umpires,
                cricketMatchInfo.FormatDebut,
                cricketMatchInfo.InternationalDebut);
        }

        public static CricketMatchInfoResponse ToDomain(this Entities.ODICricketMatchInfo cricketMatchInfo, IMapper mapper)
        {
            if (cricketMatchInfo is null)
            {
                return null!;
            }

            return new CricketMatchInfoResponse(
                cricketMatchInfo.Uuid,
                cricketMatchInfo.Season,
                cricketMatchInfo.Series,
                cricketMatchInfo.PlayerOfTheMatch,
                cricketMatchInfo.MatchNo,
                cricketMatchInfo.MatchDays,
                cricketMatchInfo.MatchTitle,
                cricketMatchInfo.Venue,
                cricketMatchInfo.MatchDate,
                cricketMatchInfo.TossWinner,
                cricketMatchInfo.TossDecision,
                cricketMatchInfo.Result,
                mapper.Map<TeamScoreDetails>(cricketMatchInfo.Team1),
                mapper.Map<TeamScoreDetails>(cricketMatchInfo.Team2),
                cricketMatchInfo.TvUmpire,
                cricketMatchInfo.MatchReferee,
                cricketMatchInfo.ReserveUmpire,
                cricketMatchInfo.Umpires,
                cricketMatchInfo.FormatDebut,
                cricketMatchInfo.InternationalDebut);
        }

        public static TestCricketMatchInfoResponse ToDomain(this Entities.TestCricketMatchInfo cricketMatchInfo, IMapper mapper)
        {
            if (cricketMatchInfo is null)
            {
                return null!;
            }

            return new TestCricketMatchInfoResponse(
                cricketMatchInfo.Uuid,
                cricketMatchInfo.Season,
                cricketMatchInfo.Series,
                cricketMatchInfo.PlayerOfTheMatch,
                cricketMatchInfo.MatchNo,
                cricketMatchInfo.MatchDays,
                cricketMatchInfo.MatchTitle,
                cricketMatchInfo.Venue,
                cricketMatchInfo.MatchDates,
                cricketMatchInfo.TossWinner,
                cricketMatchInfo.TossDecision,
                cricketMatchInfo.Result,
                mapper.Map<TestTeamScoreDetails>(cricketMatchInfo.Team1),
                mapper.Map<TestTeamScoreDetails>(cricketMatchInfo.Team2),
                cricketMatchInfo.TvUmpire,
                cricketMatchInfo.MatchReferee,
                cricketMatchInfo.ReserveUmpire,
                cricketMatchInfo.Umpires,
                cricketMatchInfo.FormatDebut,
                cricketMatchInfo.InternationalDebut);
        }
    }
}
