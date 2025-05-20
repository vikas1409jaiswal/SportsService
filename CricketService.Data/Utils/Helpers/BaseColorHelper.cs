using iTextSharp.text;

namespace CricketService.Data.Utils.Helpers
{
    public class BaseColorHelper : BaseColor
    {
        public BaseColorHelper(int red, int green, int blue)
            : base(red, green, blue)
        {
        }

        public static readonly BaseColor LIGHT_KHAKI = new BaseColor(234, 255, 128);

        public static readonly BaseColor PEACH_ORANGE = new BaseColor(255, 187, 153);

        public static readonly BaseColor PALE_LAVENDAR = new BaseColor(203, 204, 255);

        public static readonly BaseColor MEDIUM_SPRING_BUD = new BaseColor(204, 255, 153);

        public static readonly BaseColor BLIZZARD_BLUE = new BaseColor(179, 229, 255);

        public static readonly BaseColor SAFFRON = new BaseColor(255, 187, 51);

        public static readonly BaseColor BRIGHT_UBE = new BaseColor(204, 153 , 255);
    }
}
