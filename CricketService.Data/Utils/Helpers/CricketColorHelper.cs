using iTextSharp.text;

namespace CricketService.Data.Utils.Helpers
{
    public class CricketColorHelper : BaseColor
    {
        public CricketColorHelper(int red, int green, int blue)
            : base(red, green, blue)
        {
        }

        public static readonly BaseColor T20I_PLAYER_RECORD_CELL_BGCOLOR = new BaseColor(153, 228, 240);

        public static readonly BaseColor ODI_PLAYER_RECORD_CELL_BGCOLOR = new BaseColor(253, 190, 160);

        public static readonly BaseColor TEST_PLAYER_RECORD_CELL_BGCOLOR = new BaseColor(164, 166, 255);
    }
}
