using System.Runtime.Serialization;

namespace CricketService.Domain.Enums
{
    public enum CricketFormat : byte
    {
        [EnumMember(Value = "T20I")]
        T20I,
        [EnumMember(Value = "ODI")]
        ODI,
        [EnumMember(Value = "TestCricket")]
        TestCricket,
        [EnumMember(Value = "All")]
        All,
    }
}
