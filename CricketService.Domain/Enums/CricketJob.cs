using System.Runtime.Serialization;

namespace CricketService.Domain.Enums
{
    public enum CricketJob : byte
    {
        [EnumMember(Value = "CreateSeedCricketTeamHistoryTableJob")]
        CreateSeedCricketTeamHistoryTableJob,

        [EnumMember(Value = "CreateSeedCricketTeamHistoryH2HTableJob")]
        CreateSeedCricketTeamHistoryH2HTableJob,

        [EnumMember(Value = "UpdatePlayersCareerStatisticsJob")]
        UpdatePlayersCareerStatisticsJob,

        [EnumMember(Value = "UpdateTeamRecordsJob")]
        UpdateTeamRecordsJob,

        [EnumMember(Value = "CleanDatabaseJob")]
        CleanDatabaseJob,
    }
}
