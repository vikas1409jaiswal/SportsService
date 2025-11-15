using System;

namespace CricketService.Domain
{
    public class PlayerBattingRecord
    {
        public PlayerBattingRecord(
            Guid uuid,
            string matchNumber,
            DateTime matchDate,
            int inning,
            int battingPosition,
            string playerHref,
            string playerName,
            int runScored,
            int ballsFaced,
            int sixesInInning,
            int foursInInning,
            string outStatus,
            string teamName,
            string oppositionTeamName)
        {
            Uuid = uuid;
            MatchNumber = matchNumber;
            MatchDate = matchDate;
            Inning = inning;
            BattingPosition = battingPosition;
            PlayerHref = playerHref;
            PlayerName = playerName;
            RunScored = runScored;
            BallsFaced = ballsFaced;
            SixesInInning = sixesInInning;
            FoursInInning = foursInInning;
            OutStatus = outStatus;
            TeamName = teamName;
            OppositionTeamName = oppositionTeamName;
        }

        public Guid Uuid { get; set; }

        public string MatchNumber { get; set; }

        public DateTime MatchDate { get; set; }

        public int Inning { get; set; }

        public int BattingPosition { get; set; }

        public string PlayerHref { get; set; }

        public string PlayerName { get; set; }

        public int? RunScored { get; set; }

        public int? BallsFaced { get; set; }

        public int? SixesInInning { get; set; }

        public int? FoursInInning { get; set; }

        public string OutStatus { get; set; }

        public string TeamName { get; set; }

        public string OppositionTeamName { get; set; }
    }
}