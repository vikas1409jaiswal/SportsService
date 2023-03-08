using CricketService.Domain;
using CricketService.Domain.Enums;

namespace CricketService.Data.Repositories.Interfaces
{
    public interface ICricketTeamRepository
    {
        CricketTeamInfoResponse GetTeamByUuid(Guid teamUuid);

        IEnumerable<Team> GetAllTeamDetails();

        IEnumerable<string> GetAllTeamNames(CricketFormat format);

        CricketTeamInfoResponse GetTeamRecordsByName(string teamName, bool isSingle);

        object GetAllAgainstRecordsByNameT20I(string teamName);

        object GetAllAgainstRecordsByNameODI(string teamName);
    }
}
