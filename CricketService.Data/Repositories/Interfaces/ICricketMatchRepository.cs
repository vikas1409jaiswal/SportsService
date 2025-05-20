using CricketService.Domain.Common;
using CricketService.Domain.Enums;
using CricketService.Domain.RequestDomains;
using CricketService.Domain.ResponseDomains;

namespace CricketService.Data.Repositories.Interfaces;
public interface ICricketMatchRepository
{
    IEnumerable<InternationalCricketMatchResponse> GetAllLimitedOverInternationalMatches(MatchesFilters matchesFilters);

    IEnumerable<TestCricketMatchResponse> GetAllMatchesTest(MatchesFilters matchesFilters);

    IEnumerable<DomesticCricketMatchResponse> GetAllTwenty20Matches(MatchesFilters matchesFilters);

    IEnumerable<object> GetMatchesByTeamUuid(Guid teamUuid, CricketFormat format);

    IEnumerable<object> GetMatchesByTournament(CricketTournament tournament, CricketFormat format);

    Task<InternationalCricketMatchResponse> GetLimitedOverInternationalMatchByNumber(int matchNumber, CricketFormat format);

    Task<DomesticCricketMatchResponse> GetT20IMatchesByTitleAndDate(string title, string date);

    Task<TestCricketMatchResponse> GetMatchByMNumberTest(int matchNumber);

    //Create Methods
    Task<InternationalCricketMatchResponse> AddLimitedOverInternationalMatch(InternationalCricketMatchRequest match, CricketFormat format);

    Task<TestCricketMatchResponse> AddMatchTest(TestCricketMatchRequest match);

    Task<DomesticCricketMatchResponse> AddT20Match(DomesticCricketMatchRequest match);

    //Pdf Methods
    Task GeneratedPDFForMatches(IEnumerable<Guid> matchUuids, CricketFormat format);
}
