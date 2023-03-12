using System.Runtime.Serialization;

namespace CricketService.Domain.Enums
{
    public enum InfoFormats : byte
    {
        [EnumMember(Value = "BirthInfo")]
        BirthInfo,
        [EnumMember(Value = "DeathInfo")]
        DeathInfo,
    }
}
