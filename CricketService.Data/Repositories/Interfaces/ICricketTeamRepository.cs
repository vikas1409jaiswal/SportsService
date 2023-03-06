using CricketService.Domain;

namespace CricketService.Data.Repositories.Interfaces
{
    public interface ICricketTeamRepository
    {
        IEnumerable<string> GetAllTeamNamesT20I();

        IEnumerable<string> GetAllTeamNamesODI();

        CricketTeamInfoResponse GetTeamRecordsByName(string teamName, bool isSingle);

        object GetAllAgainstRecordsByNameT20I(string teamName);

        object GetAllAgainstRecordsByNameODI(string teamName);
    }
}
