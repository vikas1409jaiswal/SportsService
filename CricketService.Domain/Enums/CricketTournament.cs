using System.Runtime.Serialization;

namespace CricketService.Domain.Enums
{
    public enum CricketTournament : byte
    {
        [EnumMember(Value = "ICCMensT20IWorldCup")]
        ICCMensT20IWorldCup,
        [EnumMember(Value = "ICCMensODIWorldCup")]
        ICCMensODIWorldCup,
    }
}
