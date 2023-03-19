using CricketService.Domain;
using CricketService.Domain.Enums;

namespace CricketService.Data.Repositories.Interfaces;
public interface ICricketMatchRepository
{
    IEnumerable<CricketMatchInfoResponse> GetAllMatchesT20I();

    IEnumerable<CricketMatchInfoResponse> GetAllMatchesODI();

    IEnumerable<TestCricketMatchInfoResponse> GetAllMatchesTest();

    IEnumerable<object> GetMatchesByTeamUuid(Guid teamUuid, CricketFormat format);

    IEnumerable<object> GetMatchesByTournament(CricketTournament tournament, CricketFormat format);

    Task<CricketMatchInfoResponse> GetMatchByMNumberT20I(int matchNumber);

    Task<CricketMatchInfoResponse> GetMatchByMNumberODI(int matchNumber);

    Task<TestCricketMatchInfoResponse> GetMatchByMNumberTest(int matchNumber);

    Task<CricketMatchInfoResponse> AddMatchT20I(CricketMatchInfoRequest match);

    Task<CricketMatchInfoResponse> AddMatchODI(CricketMatchInfoRequest match);

    Task<TestCricketMatchInfoResponse> AddMatchTest(TestCricketMatchInfoRequest match);
}
