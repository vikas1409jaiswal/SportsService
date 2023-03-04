using CricketService.Domain;

namespace CricketService.Data.Extensions
{
    public static class EntityExtensions
    {
        public static Entities.T20ICricketMatchInfo ToEntityT20I(this CricketMatchInfoRequest cricketMatchInfo)
        {
            return new Entities.T20ICricketMatchInfo
            {
                Uuid = Guid.NewGuid(),
                Season = cricketMatchInfo.Season,
                Series = cricketMatchInfo.Series,
                MatchDate = cricketMatchInfo.MatchDate,
                MatchDays = cricketMatchInfo.MatchDays,
                TossWinner = cricketMatchInfo.TossWinner,
                TossDecision = cricketMatchInfo.TossDecision,
                MatchNo = cricketMatchInfo.MatchNo,
                MatchReferee = cricketMatchInfo.MatchReferee,
                MatchTitle = cricketMatchInfo.MatchTitle,
                Venue = cricketMatchInfo.Venue,
                PlayerOfTheMatch = cricketMatchInfo.PlayerOfTheMatch,
                Result = cricketMatchInfo.Result,
                Team1 = cricketMatchInfo.Team1,
                Team2 = cricketMatchInfo.Team2,
                TvUmpire = cricketMatchInfo.TvUmpire,
                ReserveUmpire = cricketMatchInfo.ReserveUmpire,
                Umpires = cricketMatchInfo.Umpires,
                FormatDebut = cricketMatchInfo.FormatDebut,
                InternationalDebut = cricketMatchInfo.InternationalDebut,
            };
        }

        public static Entities.ODICricketMatchInfo ToEntityODI(this CricketMatchInfoRequest cricketMatchInfo)
        {
            return new Entities.ODICricketMatchInfo
            {
                Uuid = Guid.NewGuid(),
                Season = cricketMatchInfo.Season,
                Series = cricketMatchInfo.Series,
                MatchDate = cricketMatchInfo.MatchDate,
                MatchDays = cricketMatchInfo.MatchDays,
                TossWinner = cricketMatchInfo.TossWinner,
                TossDecision = cricketMatchInfo.TossDecision,
                MatchNo = cricketMatchInfo.MatchNo,
                MatchReferee = cricketMatchInfo.MatchReferee,
                MatchTitle = cricketMatchInfo.MatchTitle,
                Venue = cricketMatchInfo.Venue,
                PlayerOfTheMatch = cricketMatchInfo.PlayerOfTheMatch,
                Result = cricketMatchInfo.Result,
                Team1 = cricketMatchInfo.Team1,
                Team2 = cricketMatchInfo.Team2,
                TvUmpire = cricketMatchInfo.TvUmpire,
                ReserveUmpire = cricketMatchInfo.ReserveUmpire,
                Umpires = cricketMatchInfo.Umpires,
                FormatDebut = cricketMatchInfo.FormatDebut,
                InternationalDebut = cricketMatchInfo.InternationalDebut,
            };
        }

        public static CricketMatchInfoResponse ToDomain(this Entities.T20ICricketMatchInfo cricketMatchInfo)
        {
            var team1BattingScorecard = new List<BattingScoreCard>();
            var team2BattingScorecard = new List<BattingScoreCard>();
            var team1BowlingScorecard = new List<BowlingScoreCard>();
            var team2BowlingScorecard = new List<BowlingScoreCard>();

            foreach (var card in cricketMatchInfo.Team1.BattingScoreCard)
            {
                team1BattingScorecard.Add(new BattingScoreCard(
                    card.PlayerName,
                    card.OutStatus,
                    card.RunsScored,
                    card.BallsFaced,
                    card.Fours,
                    card.Sixes,
                    card.Minutes));
            }

            foreach (var card in cricketMatchInfo.Team1.BowlingScoreCard)
            {
                team1BowlingScorecard.Add(new BowlingScoreCard(
                   card.PlayerName,
                   card.OversBowled,
                   card.Maidens,
                   card.RunsConceded,
                   card.Wickets,
                   card.WideBall,
                   card.NoBall,
                   card.Dots,
                   card.Fours,
                   card.Sixes));
            }

            foreach (var card in cricketMatchInfo.Team2.BattingScoreCard)
            {
                team2BattingScorecard.Add(new BattingScoreCard(
                    card.PlayerName,
                    card.OutStatus,
                    card.RunsScored,
                    card.BallsFaced,
                    card.Fours,
                    card.Sixes,
                    card.Minutes));
            }

            foreach (var card in cricketMatchInfo.Team2.BowlingScoreCard)
            {
                team2BowlingScorecard.Add(new BowlingScoreCard(
                   card.PlayerName,
                   card.OversBowled,
                   card.Maidens,
                   card.RunsConceded,
                   card.Wickets,
                   card.WideBall,
                   card.NoBall,
                   card.Dots,
                   card.Fours,
                   card.Sixes));
            }

            return new CricketMatchInfoResponse(
                cricketMatchInfo.Uuid,
                cricketMatchInfo.Season,
                cricketMatchInfo.Series,
                cricketMatchInfo.PlayerOfTheMatch,
                cricketMatchInfo.MatchNo,
                cricketMatchInfo.MatchDays,
                cricketMatchInfo.MatchTitle,
                cricketMatchInfo.Venue,
                cricketMatchInfo.MatchDate,
                cricketMatchInfo.TossWinner,
                cricketMatchInfo.TossDecision,
                cricketMatchInfo.Result,
                new TeamScoreDetails(
                    cricketMatchInfo.Team1.TeamName,
                    team1BattingScorecard,
                    team1BowlingScorecard,
                    cricketMatchInfo.Team1.Extras,
                    cricketMatchInfo.Team1.FallOfWickets,
                    cricketMatchInfo.Team1.DidNotBat),
                new TeamScoreDetails(
                    cricketMatchInfo.Team2.TeamName,
                    team2BattingScorecard,
                    team2BowlingScorecard,
                    cricketMatchInfo.Team2.Extras,
                    cricketMatchInfo.Team2.FallOfWickets,
                    cricketMatchInfo.Team2.DidNotBat),
                cricketMatchInfo.TvUmpire,
                cricketMatchInfo.MatchReferee,
                cricketMatchInfo.ReserveUmpire,
                cricketMatchInfo.Umpires,
                cricketMatchInfo.FormatDebut,
                cricketMatchInfo.InternationalDebut);
        }

        public static CricketMatchInfoResponse ToDomain(this Entities.ODICricketMatchInfo cricketMatchInfo)
        {
            var team1BattingScorecard = new List<BattingScoreCard>();
            var team2BattingScorecard = new List<BattingScoreCard>();
            var team1BowlingScorecard = new List<BowlingScoreCard>();
            var team2BowlingScorecard = new List<BowlingScoreCard>();

            foreach (var card in cricketMatchInfo.Team1.BattingScoreCard)
            {
                team1BattingScorecard.Add(new BattingScoreCard(
                    card.PlayerName,
                    card.OutStatus,
                    card.RunsScored,
                    card.BallsFaced,
                    card.Fours,
                    card.Sixes,
                    card.Minutes));
            }

            foreach (var card in cricketMatchInfo.Team1.BowlingScoreCard)
            {
                team1BowlingScorecard.Add(new BowlingScoreCard(
                   card.PlayerName,
                   card.OversBowled,
                   card.Maidens,
                   card.RunsConceded,
                   card.Wickets,
                   card.WideBall,
                   card.NoBall,
                   card.Dots,
                   card.Fours,
                   card.Sixes));
            }

            foreach (var card in cricketMatchInfo.Team2.BattingScoreCard)
            {
                team2BattingScorecard.Add(new BattingScoreCard(
                    card.PlayerName,
                    card.OutStatus,
                    card.RunsScored,
                    card.BallsFaced,
                    card.Fours,
                    card.Sixes,
                    card.Minutes));
            }

            foreach (var card in cricketMatchInfo.Team2.BowlingScoreCard)
            {
                team2BowlingScorecard.Add(new BowlingScoreCard(
                   card.PlayerName,
                   card.OversBowled,
                   card.Maidens,
                   card.RunsConceded,
                   card.Wickets,
                   card.WideBall,
                   card.NoBall,
                   card.Dots,
                   card.Fours,
                   card.Sixes));
            }

            return new CricketMatchInfoResponse(
                cricketMatchInfo.Uuid,
                cricketMatchInfo.Season,
                cricketMatchInfo.Series,
                cricketMatchInfo.PlayerOfTheMatch,
                cricketMatchInfo.MatchNo,
                cricketMatchInfo.MatchDays,
                cricketMatchInfo.MatchTitle,
                cricketMatchInfo.Venue,
                cricketMatchInfo.MatchDate,
                cricketMatchInfo.TossWinner,
                cricketMatchInfo.TossDecision,
                cricketMatchInfo.Result,
                new TeamScoreDetails(
                    cricketMatchInfo.Team1.TeamName,
                    team1BattingScorecard,
                    team1BowlingScorecard,
                    cricketMatchInfo.Team1.Extras,
                    cricketMatchInfo.Team1.FallOfWickets,
                    cricketMatchInfo.Team1.DidNotBat),
                new TeamScoreDetails(
                    cricketMatchInfo.Team2.TeamName,
                    team2BattingScorecard,
                    team2BowlingScorecard,
                    cricketMatchInfo.Team2.Extras,
                    cricketMatchInfo.Team2.FallOfWickets,
                    cricketMatchInfo.Team2.DidNotBat),
                cricketMatchInfo.TvUmpire,
                cricketMatchInfo.MatchReferee,
                cricketMatchInfo.ReserveUmpire,
                cricketMatchInfo.Umpires,
                cricketMatchInfo.FormatDebut,
                cricketMatchInfo.InternationalDebut);
        }
    }
}
