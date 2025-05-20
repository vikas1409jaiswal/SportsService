using System.Reflection;

namespace CricketService.Domain
{
    public class RecordAgainstTeam
    {
        public RecordAgainstTeam(
            string opponent,
            int matches,
            int won,
            int lost,
            int tied,
            int nRorDraw,
            IEnumerable<MatchesBySeason> matchesBySeason)
        {
            Opponent = opponent;
            Matches = matches;
            Won = won;
            Lost = lost;
            Tied = tied;
            MatchesBySeason = matchesBySeason;
            NRorDraw = nRorDraw;
        }

        public string Opponent { get; set; }

        public int Matches { get; set; }

        public int Won { get; set; }

        public int Lost { get; set; }

        public int Tied { get; set; }

        public int NRorDraw { get; set; }

        public IEnumerable<MatchesBySeason> MatchesBySeason { get; set; }
    }

    public class MatchesBySeason
    {
        public MatchesBySeason(
            string season,
            IEnumerable<MatchDetail> matches)
        {
            Season = season;
            Matches = matches;
        }

        public string Season { get; set; }

        public IEnumerable<MatchDetail> Matches { get; set; }
    }

    public class MatchDetail
    {
        public MatchDetail(
            Guid matchUuid,
            string title,
            string result,
            string matchNo,
            string date)
        {
            MatchUuid = matchUuid;
            Title = title;
            Result = result;
            MatchNo = matchNo;
            Date = date;
        }

        public Guid MatchUuid { get; set; }

        public string Title { get; set; }

        public string Result { get; set; }

        public string MatchNo { get; set; }

        public string Date { get; set; }
    }
}
