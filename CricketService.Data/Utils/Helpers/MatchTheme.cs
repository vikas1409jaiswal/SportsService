using iTextSharp.text;

namespace CricketService.Data.Utils.Helpers
{
    public class MatchTheme
    {
        public MatchTheme(
            BaseColor battingScoreBoardHeaderBGColor,
            BaseColor bowlingScoreBoardHeaderBGColor,
            BaseColor battingScoreBoardBodyBGColor,
            BaseColor bowlingScoreBoardBodyBGColor)
        {
            BattingScoreBoardHeaderBGColor = battingScoreBoardHeaderBGColor;
            BowlingScoreBoardHeaderBGColor = bowlingScoreBoardHeaderBGColor;
            BattingScoreBoardBodyBGColor = battingScoreBoardBodyBGColor;
            BowlingScoreBoardBodyBGColor = bowlingScoreBoardBodyBGColor;
        }

        public BaseColor BattingScoreBoardHeaderBGColor { get; set; }

        public BaseColor BowlingScoreBoardHeaderBGColor { get; set; }

        public BaseColor BattingScoreBoardBodyBGColor { get; set; }

        public BaseColor BowlingScoreBoardBodyBGColor { get; set; }

        public BaseColor MatchDetailKeyBGColor { get; set; } = BaseColorHelper.BRIGHT_UBE;

        public BaseColor MatchDetailValueBGColor { get; set; } = BaseColorHelper.SAFFRON;
    }
}
