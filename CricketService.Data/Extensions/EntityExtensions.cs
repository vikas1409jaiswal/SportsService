using AutoMapper;
using CricketService.Domain.ResponseDomains;

namespace CricketService.Data.Extensions
{
    public static class EntityExtensions
    {
        public static InternationalCricketMatchResponse ToDomain(this Entities.LimitedOverInternationalMatchInfoDTO cricketMatchInfo, IMapper mapper)
        {
            if (cricketMatchInfo is null)
            {
                return null!;
            }

            return new InternationalCricketMatchResponse(
                cricketMatchInfo.Uuid,
                cricketMatchInfo.Season,
                cricketMatchInfo.Series,
                cricketMatchInfo.SeriesResult!,
                cricketMatchInfo.MatchNumber,
                cricketMatchInfo.MatchDate,
                cricketMatchInfo.MatchType,
                cricketMatchInfo.MatchTitle,
                cricketMatchInfo.Venue,
                cricketMatchInfo.TossWinner,
                cricketMatchInfo.TossDecision,
                cricketMatchInfo.Result,
                mapper.Map<SingleInningTeamScoreboardResponse>(cricketMatchInfo.Team1),
                mapper.Map<SingleInningTeamScoreboardResponse>(cricketMatchInfo.Team2),
                cricketMatchInfo.TvUmpire,
                cricketMatchInfo.MatchReferee,
                cricketMatchInfo.ReserveUmpire,
                cricketMatchInfo.Umpires,
                cricketMatchInfo.InternationalDebut,
                cricketMatchInfo.PlayerOfTheMatch);
        }

        public static TestCricketMatchResponse ToDomain(this Entities.TestCricketMatchInfoDTO cricketMatchInfo, IMapper mapper)
        {
            if (cricketMatchInfo is null)
            {
                return null!;
            }

            return new TestCricketMatchResponse(
                cricketMatchInfo.Uuid,
                cricketMatchInfo.Season,
                cricketMatchInfo.Series,
                cricketMatchInfo.SeriesResult!,
                cricketMatchInfo.MatchNumber,
                cricketMatchInfo.MatchType,
                cricketMatchInfo.MatchDate,
                cricketMatchInfo.MatchTitle,
                cricketMatchInfo.Venue,
                cricketMatchInfo.TossWinner,
                cricketMatchInfo.TossDecision,
                cricketMatchInfo.Result,
                mapper.Map<DoubleInningTeamScoreboardResponse>(cricketMatchInfo.Team1),
                mapper.Map<DoubleInningTeamScoreboardResponse>(cricketMatchInfo.Team2),
                cricketMatchInfo.TvUmpire,
                cricketMatchInfo.MatchReferee,
                cricketMatchInfo.ReserveUmpire,
                cricketMatchInfo.Umpires,
                cricketMatchInfo.InternationalDebut,
                cricketMatchInfo.PlayerOfTheMatch);
        }

        public static DomesticCricketMatchResponse ToDomain(this Entities.T20MatchInfoDTO cricketMatchInfo, IMapper mapper)
        {
            if (cricketMatchInfo is null)
            {
                return null!;
            }

            return new DomesticCricketMatchResponse(
                cricketMatchInfo.Uuid,
                cricketMatchInfo.Season,
                cricketMatchInfo.Series,
                cricketMatchInfo.SeriesResult!,
                cricketMatchInfo.MatchType,
                cricketMatchInfo.MatchTitle,
                cricketMatchInfo.Venue,
                cricketMatchInfo.MatchDate,
                cricketMatchInfo.TossWinner,
                cricketMatchInfo.TossDecision,
                cricketMatchInfo.Result,
                mapper.Map<SingleInningTeamScoreboardResponse>(cricketMatchInfo.Team1),
                mapper.Map<SingleInningTeamScoreboardResponse>(cricketMatchInfo.Team2),
                cricketMatchInfo.TvUmpire,
                cricketMatchInfo.MatchReferee,
                cricketMatchInfo.ReserveUmpire,
                cricketMatchInfo.Umpires,
                cricketMatchInfo.FormatDebut,
                cricketMatchInfo.PlayerOfTheMatch);
        }
    }
}
