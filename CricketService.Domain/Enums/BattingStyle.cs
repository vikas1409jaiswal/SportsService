using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CricketService.Domain.Enums
{
    public enum BattingStyle
    {
        [Display(Name = "Left Hand Bat", Description = "Batter who use left hand for batting.")]
        LeftHandBat,
        [Display(Name = "Right Hand Bat", Description = "Batter who use right hand for batting.")]
        RightHandBat,
    }

    public static class EnumExtension
    {
        public static IEnumerable<DisplayAttribute> ToDisplayValues(this BattingStyle battingStyle)
        {
           return battingStyle.GetType().GetMember(battingStyle.ToString()).Select(x => x.GetCustomAttribute<DisplayAttribute>())!;
        }
    }
}
