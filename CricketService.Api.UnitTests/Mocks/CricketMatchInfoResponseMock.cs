using AutoFixture;
using CricketService.Domain;

namespace CricketService.Api.UnitTests.Mocks
{
    public static class CricketMatchInfoResponseMock
    {
        public static IEnumerable<CricketMatchInfoResponse> GetCricketMatchInfoResponses(int? matches = 2)
        {
            var fixture = new Fixture();

            fixture.Customize<TeamScoreDetails>(tsd => tsd
                .With(ts => ts.TeamName, "Team A")
                .With(ts => ts.Extras, "1b 1lb 0wd 0nb 0p"));

            fixture.Customize<BattingScoreCard>(bsc => bsc
                .With(bs => bs.PlayerName, fixture.Create<Player>())
                .With(bs => bs.OutStatus, "not out")
                .With(bs => bs.RunsScored, 42)
                .With(bs => bs.BallsFaced, 20)
                .With(bs => bs.Fours, 5)
                .With(bs => bs.Sixes, 2));

            fixture.Customize<BowlingScoreCard>(bwc => bwc
                .With(bw => bw.PlayerName, fixture.Create<Player>())
                .With(bw => bw.OversBowled, 2.5)
                .With(bw => bw.Maidens, 1)
                .With(bw => bw.RunsConceded, 16)
                .With(bw => bw.Wickets, 3)
                .With(bw => bw.WideBall, 1)
                .With(bw => bw.NoBall, 0)
                .Without(bw => bw.Dots)
                .Without(bw => bw.Fours)
                .Without(bw => bw.Sixes));

            fixture.Customize<Player>(p => p
                .With(pp => pp.Name, "Virat Kohli")
                .With(pp => pp.Href, "/virat_kohli"));

            var matchUuid = Guid.NewGuid();
            var team1 = fixture.Create<TeamScoreDetails>();
            var team2 = fixture.Create<TeamScoreDetails>();

            var responses = fixture.Build<CricketMatchInfoResponse>()
                .With(r => r.MatchUuid, matchUuid)
                .With(r => r.Series, "India vs Pakistan Series")
                .With(r => r.Season, "2010/11")
                .With(r => r.MatchDate, "20 Oct, 2010")
                .With(r => r.PlayerOfTheMatch, "MS Dhoni")
                .With(r => r.MatchDays, "50-over-match")
                .With(r => r.TossDecision, "India, elected bat first")
                .With(r => r.TossWinner, "India")
                .With(r => r.MatchNo, "ODI no. 1500")
                .With(r => r.MatchTitle, "India vs Pakistan")
                .With(r => r.Result, "India won by 100 runs.")
                .With(r => r.MatchReferee, "referee")
                .With(r => r.TvUmpire, "tv empire")
                .With(r => r.Venue, "Eden Gardens (Kolkata)")
                .With(r => r.Team1, team1)
                .With(r => r.Team2, team2)
                .CreateMany((int)matches!);

            return responses;
        }
    }
}
