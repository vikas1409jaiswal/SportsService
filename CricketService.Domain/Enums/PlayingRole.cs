using System.Runtime.Serialization;

namespace CricketService.Domain.Enums
{
    public enum PlayingRole : byte
    {
        [EnumMember(Value = "Batter")]
        Batter,
        [EnumMember(Value = "Bowler")]
        Bowler,
        [EnumMember(Value = "Allrounder")]
        Allrounder,
        [EnumMember(Value = "Wicketkeeper")]
        Wicketkeeper,
        [EnumMember(Value = "Opening Batter")]
        OpeningBatter,
        [EnumMember(Value = "Top order Batter")]
        TopOrderBatter,
        [EnumMember(Value = "Middle order Batter")]
        MiddleOrderBatter,
    }
}
