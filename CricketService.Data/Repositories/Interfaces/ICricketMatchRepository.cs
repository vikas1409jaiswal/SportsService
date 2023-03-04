using CricketService.Domain;

namespace CricketService.Data.Repositories.Interfaces;
public interface ICricketMatchRepository
{
    IEnumerable<CricketMatchInfoResponse> GetAllMatchesT20I();

    IEnumerable<CricketMatchInfoResponse> GetAllMatchesODI();

    Task<CricketMatchInfoResponse> GetMatchByMNumberT20I(int matchNumber);

    Task<CricketMatchInfoResponse> GetMatchByMNumberODI(int matchNumber);

    IEnumerable<CricketMatchInfoResponse> GetMatchByTeamT20I(string teamName);

    IEnumerable<CricketMatchInfoResponse> GetMatchByTeamODI(string teamName);

    Task<CricketMatchInfoResponse> AddMatchT20I(CricketMatchInfoRequest match);

    Task<CricketMatchInfoResponse> AddMatchODI(CricketMatchInfoRequest match);
}
