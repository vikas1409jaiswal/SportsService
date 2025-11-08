using System.Runtime.Serialization;

namespace CricketService.Domain.Enums
{
    public enum PlayersCategory : byte
    {
        [EnumMember(Value = "All")]
        All,
        [EnumMember(Value = "Captains")]
        Captains,
        [EnumMember(Value = "WicketKeeper")]
        WicketKeepers,
    }
}
