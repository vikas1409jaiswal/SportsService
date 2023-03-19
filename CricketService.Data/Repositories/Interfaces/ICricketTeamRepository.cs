using CricketService.Domain;
using CricketService.Domain.Enums;

namespace CricketService.Data.Repositories.Interfaces
{
    public interface ICricketTeamRepository
    {
        CricketTeamInfoResponse GetTeamByUuid(Guid teamUuid);

        CricketTeamInfoResponse GetTeamByName(string teamName);

        IEnumerable<Team> GetAllTeamDetails();

        IEnumerable<string> GetAllTeamNames(CricketFormat format);

        object GetAllAgainstRecordsByTeamT20I(Guid teamUuid);

        object GetAllAgainstRecordsByTeamODI(Guid teamUuid);

        object GetAllAgainstRecordsByTeamTest(Guid teamUuid);
    }
}
