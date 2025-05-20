using CricketService.Data.Entities;
using CricketService.Domain.Enums;
using System.Globalization;

namespace CricketService.Data.QualityTests
{
    public class CricketMatchDetailsQualityTest : IClassFixture<TestContext>
    {
        private readonly TestContext context;
        private readonly Dictionary<CricketFormat, IQueryable<LimitedOverInternationalMatchInfoDTO>> allMatches = new();

        public CricketMatchDetailsQualityTest(TestContext context)
        {
           this.context = context;

           var limitedOverInternatonalMatchesInfo = this.context.DbContext.LimitedOverInternationalMatchesInfo;
           var allODIMatches = limitedOverInternatonalMatchesInfo.Where(x => x.MatchNumber.Contains("ODI no."));
           var allT20IMatches = limitedOverInternatonalMatchesInfo.Where(x => x.MatchNumber.Contains("T20I no."));
           allMatches[CricketFormat.ODI] = allODIMatches;
           allMatches[CricketFormat.T20I] = allT20IMatches;
        }

        [Fact]
        public void AllMatchesShouldHaveCorrectCount()
        {
            allMatches[CricketFormat.ODI].Should().HaveCount(4870);
            allMatches[CricketFormat.T20I].Should().HaveCount(3182);
        }

        [Fact]
        public void AllMatchUuidsShouldHaveCorrectFormat()
        {
            var loiMatchUuids = allMatches[CricketFormat.ODI]
                .Concat(allMatches[CricketFormat.T20I]).Select(x => x.Uuid);

            loiMatchUuids.Should().HaveCount(8052);

            foreach (var loiMatchUuid in loiMatchUuids)
            {
                loiMatchUuid.Should().NotBeEmpty();
            }
        }

        [Fact]
        public void AllMatchTitlesShouldHaveCorrectFormat()
        {
            var odiMatchTitles = allMatches[CricketFormat.ODI].Select(x => x.MatchTitle);
            var t20iMatchTitles = allMatches[CricketFormat.T20I].Select(x => x.MatchTitle);

            foreach (var loiMatchTitle in odiMatchTitles.Concat(t20iMatchTitles))
            {
                loiMatchTitle.Should().NotBeNullOrEmpty();

                loiMatchTitle.Should().Contain(" vs ");
                loiMatchTitle.Split(" vs ").Should().HaveCount(2);
            }
        }

        [Fact]
        public void AllMatchNumbersShouldHaveCorrectFormat()
        {
            var odiMatchNumbers = allMatches[CricketFormat.ODI].Select(x => x.MatchNumber);
            var t20iMatchNumbers = allMatches[CricketFormat.T20I].Select(x => x.MatchNumber);

            foreach (var odiMatchNumber in odiMatchNumbers)
            {
                odiMatchNumber.Should().NotBeNullOrEmpty();

                var words = odiMatchNumber.Split(" ");
                words.ElementAt(0).Should().Be("ODI");
                words.ElementAt(1).Should().Be("no.");
                words.ElementAt(2).Invoking(s => int.Parse(s))
                     .Should().NotThrow<FormatException>();
            }

            foreach (var t20iMatchNumber in t20iMatchNumbers)
            {
                t20iMatchNumber.Should().NotBeNullOrEmpty();

                var words = t20iMatchNumber.Split(" ");
                words.ElementAt(0).Should().Be("T20I");
                words.ElementAt(1).Should().Be("no.");
                words.ElementAt(2).Invoking(s => int.Parse(s))
                     .Should().NotThrow<FormatException>();
            }
        }

        [Fact]
        public void AllSeasonsShouldHaveCorrectFormat()
        {
            var odiSeasons = allMatches[CricketFormat.ODI].Select(x => x.Season);
            var t20iSeasons = allMatches[CricketFormat.T20I].Select(x => x.Season);

            foreach (var odiSeason in odiSeasons)
            {
                odiSeason.Should().NotBeNullOrEmpty();

                if (odiSeason.Length == 4)
                {
                    odiSeason.Invoking(s => int.Parse(s))
                     .Should().NotThrow<FormatException>();
                }
                else if (odiSeason.Length == 7)
                {
                    odiSeason.Should().Contain("/");
                    var odiSeasonArr = odiSeason.Split("/");
                    odiSeasonArr[0].Length.Should().Be(4);
                    odiSeasonArr[0].Invoking(s => int.Parse(s))
                     .Should().NotThrow<FormatException>();
                    int.Parse(odiSeasonArr[0]).Should().BeInRange(1970, 2024);
                    odiSeasonArr[1].Length.Should().Be(2);
                    odiSeasonArr[1].Invoking(s => int.Parse(s))
                     .Should().NotThrow<FormatException>();
                }
                else
                {
                    throw new FormatException($"Invalid season type for {odiSeason}");
                }
            }

            foreach (var t20iSeason in t20iSeasons)
            {
                t20iSeason.Should().NotBeNullOrEmpty();

                if (t20iSeason.Length == 4)
                {
                    t20iSeason.Invoking(s => int.Parse(s))
                     .Should().NotThrow<FormatException>();
                }
                else if (t20iSeason.Length == 7)
                {
                    t20iSeason.Should().Contain("/");
                    var t20iSeasonArr = t20iSeason.Split("/");
                    t20iSeasonArr[0].Length.Should().Be(4);
                    t20iSeasonArr[0].Invoking(s => int.Parse(s))
                     .Should().NotThrow<FormatException>();
                    int.Parse(t20iSeasonArr[0]).Should().BeInRange(2004, 2024);
                    t20iSeasonArr[1].Length.Should().Be(2);
                    t20iSeasonArr[1].Invoking(s => int.Parse(s))
                     .Should().NotThrow<FormatException>();
                }
                else
                {
                    throw new FormatException($"Invalid season type for {t20iSeason}");
                }
            }
        }

        [Fact]
        public void AllMatchTypesShouldHaveCorrectFormatAndValue()
        {
            var odiMatchTypes = allMatches[CricketFormat.ODI].Select(x => x.MatchType);

            foreach (var odiMatchType in odiMatchTypes)
            {
                odiMatchType.Should().NotBeNullOrEmpty();
            }

            odiMatchTypes.Count(x => x.EndsWith("(35-over match)")).Should().Be(10);
            odiMatchTypes.Count(x => x.EndsWith("(40-over match)")).Should().Be(61);
            odiMatchTypes.Count(x => x.EndsWith("(45-over match)")).Should().Be(49);
            odiMatchTypes.Count(x => x.EndsWith("(50-over match)")).Should().Be(4621);
            odiMatchTypes.Count(x => x.EndsWith("(55-over match)")).Should().Be(73);
            odiMatchTypes.Count(x => x.EndsWith("(60-over match)")).Should().Be(56);

            odiMatchTypes.Count(x => x.StartsWith("day ")).Should().Be(2972);
            odiMatchTypes.Count(x => x.StartsWith("daynight ")).Should().Be(1860);
            odiMatchTypes.Count(x => x.StartsWith("night ")).Should().Be(21);
            //Need to check why 17 matches has no prefix

            var t20iMatchTypes = allMatches[CricketFormat.T20I].Select(x => x.MatchType);

            foreach (var t20iMatchType in t20iMatchTypes)
            {
                t20iMatchType.Should().NotBeNullOrEmpty();
            }

            t20iMatchTypes.Count(x => x.Equals(" (20-over match)")).Should().Be(1);
            t20iMatchTypes.Count(x => x.Equals("day (20-over match)")).Should().Be(2280);
            t20iMatchTypes.Count(x => x.Equals("night (20-over match)")).Should().Be(725);
            t20iMatchTypes.Count(x => x.Equals("daynight (20-over match)")).Should().Be(176);
        }

        [Fact]
        public void AllMatchDatesShouldHaveCorrectFormatAndValue()
        {
            var odiMatchDates = allMatches[CricketFormat.ODI].Select(x => x.MatchDate);

            foreach (var odiMatchDate in odiMatchDates)
            {
                odiMatchDate.Should().NotBeNullOrEmpty();

                DateTime.ParseExact(odiMatchDate, "MMM d yyyy", CultureInfo.InvariantCulture)
                       .Should()
                       .BeOnOrBefore(DateTime.Today)
                       .And.BeAfter(new DateTime(1971, 1, 4));
            }

            var t20iMatchDates = allMatches[CricketFormat.T20I].Select(x => x.MatchDate);

            foreach (var t20iMatchDate in t20iMatchDates)
            {
                t20iMatchDate.Should().NotBeNullOrEmpty();

                DateTime.ParseExact(t20iMatchDate, "MMM d yyyy", CultureInfo.InvariantCulture)
                       .Should()
                       .BeOnOrBefore(DateTime.Today)
                       .And.BeAfter(new DateTime(2005, 2, 16));
            }
        }

        [Fact]
        public void AllSeriesesShouldHaveCorrectFormatAndValue()
        {
            var odiSerieses = allMatches[CricketFormat.ODI].Select(x => x.Series);

            foreach (var odiSeries in odiSerieses)
            {
                odiSeries.Should().NotBeNullOrEmpty();
            }

            odiSerieses.Count(x => x.Equals("Prudential World Cup")).Should().Be(56);
            odiSerieses.Count(x => x.Equals("Benson & Hedges World Series Cup")).Should().Be(149);
            odiSerieses.Count(x => x.Equals("Benson & Hedges World Series")).Should().Be(105);
            odiSerieses.Count(x => x.Equals("Benson & Hedges World Cup")).Should().Be(39);
            odiSerieses.Count(x => x.Equals("Benson & Hedges World Championship of Cricket")).Should().Be(13);
            odiSerieses.Count(x => x.Equals("ICC Champions Trophy")).Should().Be(110);
            odiSerieses.Count(x => x.Equals("ICC World Cup")).Should().Be(145);
            odiSerieses.Count(x => x.Equals("Commonwealth Bank Series")).Should().Be(43);
            odiSerieses.Count(x => x.Contains(" tour of ")).Should().Be(2477);

            var t20iSerieses = allMatches[CricketFormat.T20I].Select(x => x.Series);

            foreach (var t20iSeries in t20iSerieses)
            {
                t20iSeries.Should().NotBeNullOrEmpty();
            }

            t20iSerieses.Count(x => x.Equals("ICC Men's T20 World Cup")).Should().Be(139);
            t20iSerieses.Count(x => x.Equals("ICC World Twenty20")).Should().Be(108);
            t20iSerieses.Count(x => x.Equals("ICC Men's T20 World Cup Qualifier")).Should().Be(71);
            t20iSerieses.Count(x => x.Contains(" tour of ")).Should().Be(1171);
        }

        [Fact]
        public void AllPlayerOfTheMatchsShouldHaveCorrectFormatAndValue()
        {
            var odiPotms = allMatches[CricketFormat.ODI].Select(x => x.PlayerOfTheMatch);
            var notNullPotmCount = 4545;

            odiPotms.Count(x => x == null).Should().Be(325);
            odiPotms.Count(x => x != null).Should().Be(notNullPotmCount);
            odiPotms.Count(x => x != null
                      && !string.IsNullOrEmpty(x.Href)
                      && x.Href.StartsWith("/cricketers/")
                      && !string.IsNullOrEmpty(x.PlayerName)).Should().Be(notNullPotmCount);

            var t20iPotms = allMatches[CricketFormat.T20I].Select(x => x.PlayerOfTheMatch);
            var notNullPotmCount2 = 2852;

            t20iPotms.Count(x => x == null).Should().Be(330);
            t20iPotms.Count(x => x != null).Should().Be(notNullPotmCount2);
            t20iPotms.Count(x => x != null
                      && !string.IsNullOrEmpty(x.Href)
                      && x.Href.StartsWith("/cricketers/")
                      && !string.IsNullOrEmpty(x.PlayerName)).Should().Be(notNullPotmCount2);
        }

        [Fact]
        public void AllTossWinnersShouldHaveCorrectFormatAndValue()
        {
            var odiTossWinners = allMatches[CricketFormat.ODI].Select(x => x.TossWinner);

            foreach (var odiTossWinner in odiTossWinners)
            {
                odiTossWinner.Should().NotBeNullOrEmpty();
            }

            odiTossWinners.Distinct().Count().Should().Be(29);

            var t20iTossWinners = allMatches[CricketFormat.T20I].Select(x => x.TossWinner);

            foreach (var t20iTossWinner in t20iTossWinners)
            {
                t20iTossWinner.Should().NotBeNullOrEmpty();
            }

            t20iTossWinners.Distinct().Count().Should().Be(109);
        }

        [Fact]
        public void AllTossDecisionsShouldHaveCorrectFormatAndValue()
        {
            var odiTossDecisions = allMatches[CricketFormat.ODI].Select(x => x.TossDecision);

            foreach (var odiTossDecision in odiTossDecisions)
            {
                odiTossDecision.Should().NotBeNullOrEmpty();
            }

            odiTossDecisions.Count(x => x.EndsWith(", elected to field first")).Should().Be(2428);
            odiTossDecisions.Count(x => x.EndsWith(", elected to bat first")).Should().Be(2438);

            var t20iTossDecisions = allMatches[CricketFormat.T20I].Select(x => x.TossDecision);

            foreach (var t20iTossDecision in t20iTossDecisions)
            {
                t20iTossDecision.Should().NotBeNullOrEmpty();
            }

            t20iTossDecisions.Count(x => x.EndsWith(", elected to field first")).Should().Be(1680);
            t20iTossDecisions.Count(x => x.EndsWith(", elected to bat first")).Should().Be(1491);
            t20iTossDecisions.Count(x => x.Equals("no toss")).Should().Be(1);
        }

        [Fact]
        public void AllVenuesShouldHaveCorrectFormatAndValue()
        {
            var odiVenues = allMatches[CricketFormat.ODI].Select(x => x.Venue);

            foreach (var odiVenue in odiVenues)
            {
                odiVenue.Should().NotBeNullOrEmpty();
            }

            odiVenues.Distinct().Should().HaveCount(282);

            var t20iVenues = allMatches[CricketFormat.T20I].Select(x => x.Venue);

            foreach (var t20iVenue in t20iVenues)
            {
                t20iVenue.Should().NotBeNullOrEmpty();
            }

            t20iVenues.Distinct().Should().HaveCount(249);

            odiVenues.Concat(t20iVenues).Distinct().Should().HaveCount(392);
        }

        [Fact]
        public void AllSeriesResultsShouldHaveCorrectFormatAndValue()
        {
            var odiSeriesResults = allMatches[CricketFormat.ODI].Select(x => x.SeriesResult);

            foreach (var odiSeriesResult in odiSeriesResults)
            {
                odiSeriesResult.Should().NotBeNull();
            }

            odiSeriesResults.Count(x => x == string.Empty).Should().Be(2019);
            odiSeriesResults.Count(x => x.Length > 0).Should().Be(2851);

            var t20iSeriesResults = allMatches[CricketFormat.T20I].Select(x => x.SeriesResult);

            foreach (var t20iSeriesResult in t20iSeriesResults)
            {
                t20iSeriesResult.Should().NotBeNull();
            }

            t20iSeriesResults.Count(x => x == string.Empty).Should().Be(1766);
            t20iSeriesResults.Count(x => x.Length > 0).Should().Be(1416);
        }

        [Fact]
        public void AllTvUmpiresShouldHaveCorrectFormatAndValue()
        {
            var odiTvUmpires = allMatches[CricketFormat.ODI].Select(x => x.TvUmpire);

            foreach (var odiTvUmpire in odiTvUmpires)
            {
                odiTvUmpire.Should().NotBeNull();
            }

            odiTvUmpires.Count(x => x == string.Empty).Should().Be(1388);
            odiTvUmpires.Count(x => x.Length > 0).Should().Be(3482);

            var t20iTvUmpires = allMatches[CricketFormat.T20I].Select(x => x.TvUmpire);

            foreach (var t20iTvUmpire in t20iTvUmpires)
            {
                t20iTvUmpire.Should().NotBeNull();
            }

            t20iTvUmpires.Count(x => x == string.Empty).Should().Be(1796);
            t20iTvUmpires.Count(x => x.Length > 0).Should().Be(1386);
        }

        #region Team Scoreboard Details
        [Fact]
        public void ODI_AllTeamsShouldHaveCorrectFormatAndValue()
        {
            var odiTeams1 = allMatches[CricketFormat.ODI].Select(x => x.Team1);
            var odiTeams2 = allMatches[CricketFormat.ODI].Select(x => x.Team2);
            var odiTeams = odiTeams1.Concat(odiTeams2);

            foreach (var odiTeam in odiTeams)
            {
                odiTeam.Should().NotBeNull();
            }

            odiTeams.Should().HaveCount(9740);
        }

        [Fact]
        public void T20I_AllTeamsShouldHaveCorrectFormatAndValue()
        {
            var t20iTeams1 = allMatches[CricketFormat.T20I].Select(x => x.Team1);
            var t20iTeams2 = allMatches[CricketFormat.T20I].Select(x => x.Team2);
            var t20iTeams = t20iTeams1.Concat(t20iTeams2);

            foreach (var t20iTeam in t20iTeams)
            {
                t20iTeam.Should().NotBeNull();
            }

            t20iTeams.Should().HaveCount(6364);
        }

        [Fact]
        public void ODI_AllTeam_TeamsShouldHaveCorrectFormatAndValue()
        {
            var allOdiMatches = allMatches[CricketFormat.ODI].ToList();
            var odiTeam1Teams = allOdiMatches.Select(x => x.Team1.Team);
            var odiTeam2Teams = allOdiMatches.Select(x => x.Team2.Team);
            var odiTeamTeams = odiTeam1Teams.Concat(odiTeam2Teams);

            foreach (var odiTeamTeam in odiTeamTeams)
            {
                odiTeamTeam.Should().NotBeNull();
                odiTeamTeam.Name.Should().NotBeNullOrEmpty();
                odiTeamTeam.Uuid.Should().NotBeEmpty();
                odiTeamTeam.LogoUrl.Should().NotBeNullOrEmpty();
            }

            odiTeamTeams.Select(x => x.Name).Distinct().Should().HaveCount(29);
            odiTeamTeams.Select(x => x.Uuid).Distinct().Should().HaveCount(29);
            odiTeamTeams.Select(x => x.LogoUrl).Distinct().Should().HaveCount(29);
        }

        [Fact]
        public void T20I_AllTeam_TeamsShouldHaveCorrectFormatAndValue()
        {
            var allT20iMatches = allMatches[CricketFormat.T20I].ToList();
            var t20iTeam1Teams = allT20iMatches.Select(x => x.Team1.Team);
            var t20iTeam2Teams = allT20iMatches.Select(x => x.Team2.Team);
            var t20iTeamTeams = t20iTeam1Teams.Concat(t20iTeam2Teams);

            foreach (var t20iTeamTeam in t20iTeamTeams)
            {
                t20iTeamTeam.Should().NotBeNull();
                t20iTeamTeam.Name.Should().NotBeNullOrEmpty();
                t20iTeamTeam.Uuid.Should().NotBeEmpty();
                t20iTeamTeam.LogoUrl.Should().NotBeNullOrEmpty();
            }

            t20iTeamTeams.Select(x => x.Name).Distinct().Should().HaveCount(109);
            t20iTeamTeams.Select(x => x.Uuid).Distinct().Should().HaveCount(109);
            t20iTeamTeams.Select(x => x.LogoUrl).Distinct().Should().HaveCount(109);
        }

        [Fact]
        public void ODI_AllTeam_BattingScoreboardsShouldHaveCorrectFormatAndValue()
        {
            var allOdiMatches = allMatches[CricketFormat.ODI].ToList();
            var odiTeam1BSBs = allOdiMatches.Select(x => x.Team1.BattingScoreCard);
            var odiTeam2BSBs = allOdiMatches.Select(x => x.Team2.BattingScoreCard);
            var odiTeamBSBs = odiTeam1BSBs.Concat(odiTeam2BSBs);

            odiTeamBSBs.Count(x => x == null).Should().Be(0);
            odiTeamBSBs.Count(x => x.Count > 12).Should().Be(0);
            odiTeamBSBs.Count(x => x.Count == 12).Should().Be(3);
            odiTeam1BSBs.Count(x => x.Count == 0).Should().Be(6);
            odiTeam2BSBs.Count(x => x.Count == 0).Should().Be(104);
        }

        [Fact]
        public void T20I_AllTeam_BattingScoreboardsShouldHaveCorrectFormatAndValue()
        {
            var allT20iMatches = allMatches[CricketFormat.T20I].ToList();
            var t20iTeam1BSBs = allT20iMatches.Select(x => x.Team1.BattingScoreCard);
            var t20iTeam2BSBs = allT20iMatches.Select(x => x.Team2.BattingScoreCard);
            var t20iTeamBSBs = t20iTeam1BSBs.Concat(t20iTeam2BSBs);

            t20iTeamBSBs.Count(x => x == null).Should().Be(0);
            t20iTeamBSBs.Count(x => x.Count > 12).Should().Be(0);
            t20iTeamBSBs.Count(x => x.Count == 12).Should().Be(2);
            t20iTeam1BSBs.Count(x => x.Count == 0).Should().Be(12);
            t20iTeam2BSBs.Count(x => x.Count == 0).Should().Be(56);
        }

        [Theory]
        [InlineData(CricketFormat.ODI)]
        [InlineData(CricketFormat.T20I)]
        public void AllTeam_BattingScoreboard_PlayerNamesShouldHaveCorrectFormatAndValue(CricketFormat format)
        {
            var allfilteredMatches = allMatches[format].ToList();
            var matchTeam1BSBs = allfilteredMatches.Select(x => x.Team1.BattingScoreCard);
            var matchTeam2BSBs = allfilteredMatches.Select(x => x.Team2.BattingScoreCard);
            var matchTeamBSBs = matchTeam1BSBs.Concat(matchTeam2BSBs);

            foreach (var matchTeamBSB in matchTeamBSBs)
            {
                foreach (var item in matchTeamBSB)
                {
                    item.PlayerName.Name.Should().NotBeNullOrEmpty();
                    item.PlayerName.Href.Should().NotBeNullOrEmpty();
                    item.PlayerName.Href.Should().StartWith("/cricketers/");
                }

                matchTeamBSB.Count(x => x.PlayerName.Name.Contains("(c)")).Should().BeInRange(0, 1);
                matchTeamBSB.Count(x => x.PlayerName.Name.Contains('†')).Should().BeInRange(0, 1);
            }
        }

        [Fact]
        public void ODI_AllTeam_BattingScoreboard_RunsScoredShouldHaveCorrectFormatAndValue()
        {
            var allOdiMatches = allMatches[CricketFormat.ODI].ToList();
            var odiTeam1BSBs = allOdiMatches.Select(x => x.Team1.BattingScoreCard);
            var odiTeam2BSBs = allOdiMatches.Select(x => x.Team2.BattingScoreCard);
            var odiTeamBSBs = odiTeam1BSBs.Concat(odiTeam2BSBs);

            foreach (var odiTeamBSB in odiTeamBSBs)
            {
                foreach (var item in odiTeamBSB)
                {
                    if (new string[] { "absent", "absent hurt", "absent ill" }.Contains(item.OutStatus.TrimEnd()))
                    {
                        item.RunsScored.Should().BeNull();
                    }
                    else
                    {
                        item.RunsScored.Should().NotBeNull();
                        item.RunsScored.Should().BeInRange(0, 264);
                    }
                }
            }
        }

        [Fact]
        public void T20I_AllTeam_BattingScoreboard_RunsScoredShouldHaveCorrectFormatAndValue()
        {
            var allT20IMatches = allMatches[CricketFormat.T20I].ToList();
            var t20iTeam1BSBs = allT20IMatches.Select(x => x.Team1.BattingScoreCard);
            var t20iTeam2BSBs = allT20IMatches.Select(x => x.Team2.BattingScoreCard);
            var t20iTeamBSBs = t20iTeam1BSBs.Concat(t20iTeam2BSBs);

            foreach (var t20iTeamBSB in t20iTeamBSBs)
            {
                foreach (var item in t20iTeamBSB)
                {
                    if (new string[] { "absent hurt" }.Contains(item.OutStatus.TrimEnd()))
                    {
                        item.RunsScored.Should().BeNull();
                    }
                    else
                    {
                        item.RunsScored.Should().NotBeNull();
                        item.RunsScored.Should().BeInRange(0, 172);
                    }
                }
            }
        }

        [Fact]
        public void ODI_AllTeam_BattingScoreboard_BallFacedShouldHaveCorrectFormatAndValue()
        {
            var allOdiMatches = allMatches[CricketFormat.ODI].ToList();
            var odiTeam1BSBs = allOdiMatches.Select(x => x.Team1.BattingScoreCard);
            var odiTeam2BSBs = allOdiMatches.Select(x => x.Team2.BattingScoreCard);
            var odiTeamBSBs = odiTeam1BSBs.Concat(odiTeam2BSBs);

            foreach (var odiTeamBSB in odiTeamBSBs)
            {
                foreach (var item in odiTeamBSB)
                {
                    if (new string[] { "absent", "absent hurt", "absent ill" }.Contains(item.OutStatus.TrimEnd()))
                    {
                        item.BallsFaced.Should().BeNull();
                    }
                    else
                    {
                        item.BallsFaced.Should().NotBeNull();
                        item.BallsFaced.Should().BeInRange(0, 201);
                    }
                }
            }
        }

        [Fact]
        public void T20I_AllTeam_BattingScoreboard_BallFacedShouldHaveCorrectFormatAndValue()
        {
            var allT20IMatches = allMatches[CricketFormat.T20I].ToList();
            var t20iTeam1BSBs = allT20IMatches.Select(x => x.Team1.BattingScoreCard);
            var t20iTeam2BSBs = allT20IMatches.Select(x => x.Team2.BattingScoreCard);
            var t20iTeamBSBs = t20iTeam1BSBs.Concat(t20iTeam2BSBs);

            foreach (var t20iTeamBSB in t20iTeamBSBs)
            {
                foreach (var item in t20iTeamBSB)
                {
                    if (new string[] { "absent hurt" }.Contains(item.OutStatus.TrimEnd()))
                    {
                        item.BallsFaced.Should().BeNull();
                    }
                    else
                    {
                        item.BallsFaced.Should().NotBeNull();
                        item.BallsFaced.Should().BeInRange(0, 172);
                    }
                }
            }
        }

        [Fact]
        public void ODI_AllTeam_BattingScoreboard_OutStatusShouldHaveCorrectFormatAndValue()
        {
            var allOdiMatches = allMatches[CricketFormat.ODI].ToList();
            var odiTeam1BSBs = allOdiMatches.Select(x => x.Team1.BattingScoreCard);
            var odiTeam2BSBs = allOdiMatches.Select(x => x.Team2.BattingScoreCard);
            var odiTeamBSBs = odiTeam1BSBs.Concat(odiTeam2BSBs);

            foreach (var odiTeamBSB in odiTeamBSBs)
            {
                foreach (var item in odiTeamBSB)
                {
                    item.OutStatus.Should().NotBeNullOrEmpty();
                }
            }

            var flatBattingScoreboards = odiTeamBSBs.SelectMany(x => x);

            flatBattingScoreboards.Count(x => x.OutStatus.Equals("absent ill")).Should().Be(1);
            flatBattingScoreboards.Count(x => x.OutStatus.Equals("absent hurt")).Should().Be(41);
            flatBattingScoreboards.Count(x => x.OutStatus.Equals("absent ")).Should().Be(1);
            flatBattingScoreboards.Count(x => x.OutStatus.Equals("retired hurt ")).Should().Be(105);
            flatBattingScoreboards.Count(x => x.OutStatus.Equals("retired ill ")).Should().Be(1);
            flatBattingScoreboards.Count(x => x.OutStatus.Equals("retired not out ")).Should().Be(1);
            flatBattingScoreboards.Count(x => x.OutStatus.Equals("obstructing the field ")).Should().Be(8);
            flatBattingScoreboards.Count(x => x.OutStatus.Equals("handled the ball ")).Should().Be(3);
            flatBattingScoreboards.Count(x => x.OutStatus.Equals("timed out ")).Should().Be(1);
            flatBattingScoreboards.Count(x => x.OutStatus.Equals("not out ")).Should().Be(15527);
            flatBattingScoreboards.Count(x => x.OutStatus.Equals("run out ")).Should().Be(1735);

            flatBattingScoreboards.Count(x => x.OutStatus.StartsWith("hit wicket b ")).Should().Be(77);
            flatBattingScoreboards.Count(x => x.OutStatus.StartsWith("run out (")
                                              && x.OutStatus.EndsWith(")")).Should().BeInRange(4700, 5000);
            flatBattingScoreboards.Count(x => x.OutStatus.StartsWith("lbw b ")).Should().BeInRange(7200, 8000);
            flatBattingScoreboards.Count(x => x.OutStatus.StartsWith("c †")).Should().BeInRange(9900, 10000);
            flatBattingScoreboards.Count(x => x.OutStatus.StartsWith("c ")).Should().BeInRange(40000, 50000);
            flatBattingScoreboards.Count(x => x.OutStatus.StartsWith("c & b ")).Should().BeInRange(2000, 3000);
            flatBattingScoreboards.Count(x => x.OutStatus.StartsWith(" b ")).Should().BeInRange(13000, 15000);
            flatBattingScoreboards.Count(x => x.OutStatus.StartsWith("st †")).Should().BeInRange(1500, 2000);
            flatBattingScoreboards.Count(x => x.OutStatus.StartsWith("st sub (†")).Should().Be(2);
        }

        [Fact]
        public void AllTeam_ExtrasShouldHaveCorrectFormatAndValue()
        {
            var odiTeam1Extras = allMatches[CricketFormat.ODI].Select(x => x.Team1.Extras);
            var odiTeam2Extras = allMatches[CricketFormat.ODI].Select(x => x.Team2.Extras);
            var odiTeamExtras = odiTeam1Extras.Concat(odiTeam2Extras);

            odiTeam1Extras.Count(x => x == null).Should().Be(0);
            odiTeam2Extras.Count(x => x == null).Should().Be(0);
            odiTeam1Extras.Count(x => x == string.Empty).Should().Be(6);
            odiTeam2Extras.Count(x => x == string.Empty).Should().Be(104);
            odiTeam1Extras.Count(x => x.Length > 0).Should().Be(4857);
            odiTeam2Extras.Count(x => x.Length > 0).Should().Be(4759);
            odiTeam1Extras.Count(x => x == "0").Should().Be(5);
            odiTeam2Extras.Count(x => x == "0").Should().Be(18);

            foreach (var odiTeamExtra in odiTeamExtras.Where(x => x.Length > 0))
            {
                odiTeamExtra.Invoking(s => int.Parse(s))
                     .Should().NotThrow<FormatException>();
            }
        }

        [Fact]
        public void AllTeam_DidNotBatShouldHaveCorrectFormatAndValue()
        {
            var odiTeam1Dnbs = allMatches[CricketFormat.ODI].Select(x => x.Team1.DidNotBat);
            var odiTeam2Dnbs = allMatches[CricketFormat.ODI].Select(x => x.Team2.DidNotBat);
            var odiTeamDnbs = odiTeam1Dnbs.Concat(odiTeam2Dnbs);

            odiTeam1Dnbs.Count(x => x == null).Should().Be(0);
            odiTeam2Dnbs.Count(x => x == null).Should().Be(0);
            odiTeam1Dnbs.Count(x => x.Length == 0).Should().Be(2021);
            odiTeam1Dnbs.Count(x => x.Length > 0).Should().Be(2842);
            odiTeam2Dnbs.Count(x => x.Length == 0).Should().Be(2084);
            odiTeam2Dnbs.Count(x => x.Length > 0).Should().Be(2779);

            foreach (var odiTeamDnb in odiTeamDnbs)
            {
                odiTeamDnb.Length.Should().BeInRange(0, 9);

                foreach (var item in odiTeamDnb.ToList())
                {
                    item.Name.Should().NotBeNullOrEmpty();
                    item.Href.Should().NotBeNullOrEmpty();
                    item.Href.Should().Contain("/cricketers/");
                }
            }
        }
        #endregion
    }
}