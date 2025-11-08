using System.Runtime.Serialization;

namespace CricketService.Domain.Enums
{
    public enum CricketFormat : byte
    {
        [EnumMember(Value = "T20I")]
        T20I,
        [EnumMember(Value = "WT20I")]
        WT20I,
        [EnumMember(Value = "ODI")]
        ODI,
        [EnumMember(Value = "WODI")]
        WODI,
        [EnumMember(Value = "TestCricket")]
        TestCricket,
        [EnumMember(Value = "Twenty20")]
        Twenty20,
        [EnumMember(Value = "All")]
        All,
    }
}
