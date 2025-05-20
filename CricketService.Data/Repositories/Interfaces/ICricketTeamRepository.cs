using CricketService.Data.Entities;
using CricketService.Domain;
using CricketService.Domain.Enums;

namespace CricketService.Data.Repositories.Interfaces
{
    public interface ICricketTeamRepository
    {
        IEnumerable<Guid> GetAllTeamsUuid();

        CricketTeamInfoResponse GetTeamByUuid(Guid teamUuid);

        CricketTeamInfoResponse GetTeamByName(string teamName);

        IEnumerable<PlayerTeam> GetAllTeamDetails();

        IEnumerable<string> GetAllTeamNames(CricketFormat format);

        IEnumerable<RecordAgainstTeam> GetAllAgainstRecordsByTeamT20I(Guid teamUuid);

        IEnumerable<RecordAgainstTeam> GetAllAgainstRecordsByTeamODI(Guid teamUuid);

        IEnumerable<RecordAgainstTeam> GetAllAgainstRecordsByTeamTest(Guid teamUuid);

        CricketTeamInfoResponse GetTeamStatistics(CricketTeamInfoDTO team, CricketFormat? format = null, int? matchUntil = null, CricketTeamInfoDTO? opponentTeam = null);

        Task GeneratedPDFForTeams();
    }
}
