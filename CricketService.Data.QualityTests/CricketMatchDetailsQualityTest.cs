using System.Globalization;
using CricketService.Data.Entities;
using CricketService.Domain.BaseDomains;
using CricketService.Domain.Enums;

namespace CricketService.Data.QualityTests
{
    public class CricketMatchDetailsQualityTest : IClassFixture<TestContext>
    {
        private const int ODIMatchCount = 4897;
        private const int WODIMatchCount = 0;
        private const int T20IMatchCount = 0;
        private const int WT20IMatchCount = 0;
        private const int TestMatchCount = 0;

        private readonly TestContext context;
        private readonly Dictionary<CricketFormat, IQueryable<LimitedOverInternationalMatchInfoDTO>> allLoiMatches = new();
        private readonly IEnumerable<TestCricketMatchInfoDTO> allTestMatches = new List<TestCricketMatchInfoDTO>();

        public CricketMatchDetailsQualityTest(TestContext context)
        {
           this.context = context;

           var limitedOverInternatonalMatchesInfo = this.context.DbContext.LimitedOverInternationalMatchesInfo;
           var testMatchesInfo = this.context.DbContext.TestCricketMatchInfo;

           var allODIMatches = limitedOverInternatonalMatchesInfo.Where(x => x.MatchNumber.StartsWith("ODI no."));
           var allWODIMatches = limitedOverInternatonalMatchesInfo.Where(x => x.MatchNumber.StartsWith("WODI no."));
           var allT20IMatches = limitedOverInternatonalMatchesInfo.Where(x => x.MatchNumber.StartsWith("T20I no."));
           var allWT20IMatches = limitedOverInternatonalMatchesInfo.Where(x => x.MatchNumber.StartsWith("WT20I no."));

           allTestMatches = testMatchesInfo.Where(x => x.MatchNumber.StartsWith("Test no. "));

           allLoiMatches[CricketFormat.All] = limitedOverInternatonalMatchesInfo;
           allLoiMatches[CricketFormat.ODI] = allODIMatches;
           allLoiMatches[CricketFormat.WODI] = allWODIMatches;
           allLoiMatches[CricketFormat.T20I] = allT20IMatches;
           allLoiMatches[CricketFormat.WT20I] = allWT20IMatches;
        }

        [Theory]
        [InlineData(CricketFormat.ODI, ODIMatchCount)]
        [InlineData(CricketFormat.WODI, WODIMatchCount)]
        [InlineData(CricketFormat.T20I, T20IMatchCount)]
        [InlineData(CricketFormat.WT20I, WT20IMatchCount)]
        [InlineData(CricketFormat.TestCricket, TestMatchCount)]
        public void AllInternationalMatches_MatchesCount_ShouldHaveCorrectValue(CricketFormat format, int tmc)
        {
            if (format == CricketFormat.TestCricket)
            {
                allTestMatches.Should().HaveCount(tmc);
            }
            else
            {
                allLoiMatches[format].Should().HaveCount(tmc);
            }
        }

        [Fact]
        public void AllInternationalMatches_MatchUuids_ShouldHaveCorrectFormat()
        {
            var loiMatchUuids = allLoiMatches[CricketFormat.All]
                .Select(x => x.Uuid);

            var testMatchUuids = allTestMatches.Select(x => x.Uuid);

            var allUuids = loiMatchUuids.Concat(testMatchUuids);

            var totalIntMatchCount = ODIMatchCount + WODIMatchCount + T20IMatchCount + WT20IMatchCount + TestMatchCount;

            allUuids.Should().HaveCount(totalIntMatchCount);
            allUuids.Distinct().Should().HaveCount(totalIntMatchCount);

            foreach (var matchUuid in allUuids)
            {
                matchUuid.Should().NotBeEmpty();
            }
        }

        [Fact]
        public void AllInternationalMatches_MatchTitles_ShouldHaveCorrectFormat()
        {
            var loiMatchTitles = allLoiMatches[CricketFormat.All].Select(x => x.MatchTitle);
            var testMatchTitles = allTestMatches.Select(x => x.MatchTitle);

            var allMatchTitles = loiMatchTitles.Concat(testMatchTitles);

            foreach (var matchTitle in allMatchTitles)
            {
                matchTitle.Should().NotBeNullOrEmpty();

                matchTitle.Should().Contain(" vs ");
                var teamNames = matchTitle.Split(" vs ");
                teamNames.Should().HaveCount(2);
                foreach (var teamName in teamNames)
                {
                    teamName.Should().NotStartWith(" ");
                    teamName.Should().NotEndWith(" ");
                }
            }

            var loiWomenMatchTitles = allLoiMatches[CricketFormat.WT20I]
                .Concat(allLoiMatches[CricketFormat.WODI])
                .Select(x => x.MatchTitle);

            foreach (var loiWomenMatchTitle in loiWomenMatchTitles)
            {
                var teamNames = loiWomenMatchTitle.Split(" vs ");

                foreach (var teamName in teamNames)
                {
                    teamName.Should().EndWith(" Women");
                    teamName.Replace(" Women", string.Empty).Length.Should().BeGreaterThan(3);
                }
            }
        }

        [Theory]
        [InlineData(CricketFormat.ODI, ODIMatchCount)]
        [InlineData(CricketFormat.WODI, WODIMatchCount)]
        [InlineData(CricketFormat.T20I, T20IMatchCount)]
        [InlineData(CricketFormat.WT20I, WT20IMatchCount)]
        [InlineData(CricketFormat.TestCricket, TestMatchCount)]
        public void AllInternationalMatches_MatchNumbers_ShouldHaveCorrectFormat(CricketFormat format, int tmc)
        {
            IEnumerable<string> allMatchNumbers = new List<string>();

            if (format == CricketFormat.TestCricket)
            {
               allMatchNumbers = allTestMatches.Select(x => x.MatchNumber);
            }
            else
            {
               allMatchNumbers = allLoiMatches[format].Select(x => x.MatchNumber);
            }

            var modFormat = format.ToString().Replace("TestCricket", "Test");

            foreach (var matchNumber in allMatchNumbers)
            {
                matchNumber.Should().NotBeNullOrEmpty();

                matchNumber.Should().StartWith($"{modFormat} no. ");

                var matchNumberInt = matchNumber.Replace($"{modFormat} no. ", string.Empty);

                matchNumberInt
                    .Invoking(s => int.Parse(s))
                    .Should().NotThrow<FormatException>();
                int.Parse(matchNumberInt).Should().BeInRange(1, tmc);
            }

            allMatchNumbers.Should().HaveCount(allMatchNumbers.Distinct().Count());
        }

        [Theory]
        [InlineData(CricketFormat.ODI, 1970, 2025)]
        [InlineData(CricketFormat.WODI, 0, 0)]
        [InlineData(CricketFormat.T20I, 2004, 2025)]
        [InlineData(CricketFormat.WT20I, 2004, 2025)]
        [InlineData(CricketFormat.TestCricket, 1876, 2025)]
        public void AllInternationalMatches_Seasons_ShouldHaveCorrectFormat(CricketFormat format, int startYear, int endYear)
        {
            IEnumerable<string> allSeasons = new List<string>();

            if (format == CricketFormat.TestCricket)
            {
                allSeasons = allTestMatches.Select(x => x.Season);
            }
            else
            {
                allSeasons = allLoiMatches[format].Select(x => x.Season);
            }

            foreach (var season in allSeasons)
            {
                season.Should().NotBeNullOrEmpty();

                if (season.Length == 4)
                {
                    season.Invoking(s => int.Parse(s))
                     .Should().NotThrow<FormatException>();
                    int.Parse(season).Should().BeInRange(startYear, endYear);
                }
                else if (season.Length == 7)
                {
                    season.Should().Contain("/");
                    var loiSeasonArr = season.Split("/");
                    loiSeasonArr[0].Length.Should().Be(4);
                    loiSeasonArr[0].Invoking(s => int.Parse(s))
                     .Should().NotThrow<FormatException>();
                    int.Parse(loiSeasonArr[0]).Should().BeInRange(startYear, endYear);
                    loiSeasonArr[1].Length.Should().Be(2);
                    loiSeasonArr[1].Invoking(s => int.Parse(s))
                     .Should().NotThrow<FormatException>();
                }
                else
                {
                    throw new FormatException($"Invalid season type for {season}");
                }
            }

        }

        #region Check Match Types
        [Fact]
        public void ODI_AllInternationalMatches_MatchTypes_ShouldHaveCorrectFormatAndValue()
        {
            var odiMatchTypes = allLoiMatches[CricketFormat.ODI].Select(x => x.MatchType);

            foreach (var odiMatchType in odiMatchTypes)
            {
                odiMatchType.Should().NotBeNullOrEmpty();
            }

            odiMatchTypes.Count(x => x.Equals("day (35-over match)")).Should().Be(10);
            odiMatchTypes.Count(x => x.Equals("day (40-over match)")).Should().Be(61);
            odiMatchTypes.Count(x => x.Equals("day (45-over match)")).Should().Be(49);
            odiMatchTypes.Count(x => x.Equals("day (55-over match)")).Should().Be(73);
            odiMatchTypes.Count(x => x.Equals("day (60-over match)")).Should().Be(56);

            odiMatchTypes.Count(x => x.Equals(" (50-over match)")).Should().Be(17);
            odiMatchTypes.Count(x => x.Equals("day (50-over match)")).Should().Be(2736);
            odiMatchTypes.Count(x => x.Equals("daynight (50-over match)")).Should().Be(1862);
            odiMatchTypes.Count(x => x.Equals("night (50-over match)")).Should().Be(21);
        }

        [Fact]
        public void T20I_AllInternationalMatches_MatchTypes_ShouldHaveCorrectFormatAndValue()
        {
            var t20iMatchTypes = allLoiMatches[CricketFormat.T20I].Select(x => x.MatchType);

            foreach (var t20iMatchType in t20iMatchTypes)
            {
                t20iMatchType.Should().NotBeNullOrEmpty();
            }

            t20iMatchTypes.Count(x => x.Equals(" (20-over match)")).Should().Be(1);
            t20iMatchTypes.Count(x => x.Equals("day (20-over match)")).Should().Be(2284);
            t20iMatchTypes.Count(x => x.Equals("night (20-over match)")).Should().Be(728);
            t20iMatchTypes.Count(x => x.Equals("daynight (20-over match)")).Should().Be(176);
        }

        [Fact]
        public void WT20I_AllInternationalMatches_MatchTypes_ShouldHaveCorrectFormatAndValue()
        {
            var t20iMatchTypes = allLoiMatches[CricketFormat.WT20I].Select(x => x.MatchType);

            foreach (var t20iMatchType in t20iMatchTypes)
            {
                t20iMatchType.Should().NotBeNullOrEmpty();
            }

            t20iMatchTypes.Count(x => x.Equals("day (20-over match)")).Should().Be(1943);
            t20iMatchTypes.Count(x => x.Equals("night (20-over match)")).Should().Be(239);
            t20iMatchTypes.Count(x => x.Equals("daynight (20-over match)")).Should().Be(119);
        }

        [Fact]
        public void TestCricket_AllInternationalMatches_MatchTypes_ShouldHaveCorrectFormatAndValue()
        {
            var testMatchTypes = allTestMatches.Select(x => x.MatchType);

            foreach (var testMatchType in testMatchTypes)
            {
                testMatchType.Should().NotBeNullOrEmpty();
            }

            testMatchTypes.Count(x => x.Equals("day (0-day match)")).Should().Be(100);
            testMatchTypes.Count(x => x.Equals("day (3-day match)")).Should().Be(121);
            testMatchTypes.Count(x => x.Equals("day (4-day match)")).Should().Be(134);
            testMatchTypes.Count(x => x.Equals("day (6-day match)")).Should().Be(78);
            testMatchTypes.Count(x => x.Equals("day (5-day match)")).Should().Be(2120);
            testMatchTypes.Count(x => x.Equals("daynight (5-day match)")).Should().Be(22);
            testMatchTypes.Count(x => x.Equals("daynight (4-day match)")).Should().Be(1);
            testMatchTypes.Count(x => x.Equals(" (5-day match)")).Should().Be(8);
        }
        #endregion

        [Theory]
        [InlineData(CricketFormat.ODI, new int[] { 1971, 1, 5 })]
        [InlineData(CricketFormat.WODI, new int[] { 2025, 5, 31 })]
        [InlineData(CricketFormat.T20I, new int[] { 2005, 2, 17 })]
        [InlineData(CricketFormat.WT20I, new int[] { 2004, 8, 5 })]
        [InlineData(CricketFormat.TestCricket, new int[] { 1877, 3, 15 })]
        public void AllInternationalMatches_MatchDates_ShouldHaveCorrectFormatAndValue(CricketFormat format, int[] firstMatchDate)
        {
            IEnumerable<string> allMatchDates = new List<string>();

            if (format == CricketFormat.TestCricket)
            {
                allMatchDates = allTestMatches.Select(x => x.MatchDate);
            }
            else
            {
                allMatchDates = allLoiMatches[format].Select(x => x.MatchDate);
            }

            foreach (var matchDate in allMatchDates)
            {
                matchDate.Should().NotBeNullOrEmpty();

                DateTime.ParseExact(matchDate, "MMM d yyyy", CultureInfo.InvariantCulture)
                       .Should()
                       .BeOnOrBefore(DateTime.Today)
                       .And.BeOnOrAfter(new DateTime(firstMatchDate[0], firstMatchDate[1], firstMatchDate[2]));
            }
        }

        [Fact] //Do it later
        public void AllInternationalMatches_Serieses_ShouldHaveCorrectFormatAndValue()
        {
            var odiSerieses = allLoiMatches[CricketFormat.ODI].Select(x => x.Series);

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
            odiSerieses.Count(x => x.Contains(" tour of ")).Should().Be(2483);

            var t20iSerieses = allLoiMatches[CricketFormat.T20I].Select(x => x.Series);

            foreach (var t20iSeries in t20iSerieses)
            {
                t20iSeries.Should().NotBeNullOrEmpty();
            }

            t20iSerieses.Count(x => x.Equals("ICC Men's T20 World Cup")).Should().Be(139);
            t20iSerieses.Count(x => x.Equals("ICC World Twenty20")).Should().Be(108);
            t20iSerieses.Count(x => x.Equals("ICC Men's T20 World Cup Qualifier")).Should().Be(71);
            t20iSerieses.Count(x => x.Contains(" tour of ")).Should().Be(1178);
        }

        [Theory]
        [InlineData(CricketFormat.ODI, 4559, ODIMatchCount)]
        [InlineData(CricketFormat.WODI, 0, WODIMatchCount)]
        [InlineData(CricketFormat.T20I, 2859, T20IMatchCount)]
        [InlineData(CricketFormat.WT20I, 1921, WT20IMatchCount)]
        [InlineData(CricketFormat.TestCricket, 1680, TestMatchCount)]
        public void AllInternationalMatches_PlayerOfTheMatchs_ShouldHaveCorrectFormatAndValue(CricketFormat format, int potmCount, int tcm)
        {
            IEnumerable<PlayerOfTheMatch> allPotms = new List<PlayerOfTheMatch>();

            if (format == CricketFormat.TestCricket)
            {
               allPotms = allTestMatches.Select(x => x.PlayerOfTheMatch);
            }
            else
            {
                allPotms = allLoiMatches[format].Select(x => x.PlayerOfTheMatch);
            }

            allPotms.Count(x => x == null).Should().Be(tcm - potmCount);
            allPotms.Count(x => x != null).Should().Be(potmCount);
            allPotms.Count(x => x != null
                      && !string.IsNullOrEmpty(x.Href)
                      && x.Href.StartsWith("/cricketers/")
                      && !string.IsNullOrEmpty(x.PlayerName)).Should().Be(potmCount);
        }

        [Theory]
        [InlineData(CricketFormat.ODI, 29, 0)]
        [InlineData(CricketFormat.WODI, 0, 0)]
        [InlineData(CricketFormat.T20I, 108, 1)]
        [InlineData(CricketFormat.WT20I, 91, 2)]
        [InlineData(CricketFormat.TestCricket, 12, 0)]
        public void AllInternationalMatches_TossWinners_ShouldHaveCorrectFormatAndValue(CricketFormat format, int distinctTeamsCount, int noTossCount)
        {
            IEnumerable<string> allTossWinners = new List<string>();

            if (format == CricketFormat.TestCricket)
            {
               allTossWinners = allTestMatches.Select(x => x.TossWinner);
            }
            else
            {
                allTossWinners = allLoiMatches[format].Select(x => x.TossWinner);
            }

            foreach (var tossWinner in allTossWinners)
            {
                tossWinner.Should().NotBeNullOrEmpty();
                tossWinner.Should().NotStartWith(" ");
                tossWinner.Should().NotEndWith(" ");
            }

            allTossWinners.Count(x => x.Contains("no toss")).Should().Be(noTossCount);
            allTossWinners.Where(x => !x.Contains("no toss")).Distinct().Should().HaveCount(distinctTeamsCount);
        }

        [Theory]
        [InlineData(CricketFormat.ODI, 2443, 2438, 0)]
        [InlineData(CricketFormat.WODI, 0, 0, 0)]
        [InlineData(CricketFormat.T20I, 1494, 1684, 1)]
        [InlineData(CricketFormat.WT20I, 1147, 1149, 2)]
        [InlineData(CricketFormat.TestCricket, 1870, 714, 0)]
        public void AllInternationalMatches_TossDecisions_ShouldHaveCorrectFormatAndValue(CricketFormat format, int eleBatFirstCount, int eleFieldFirstCount, int noTossCount)
        {

            IEnumerable<string> allTossDecisions = new List<string>();

            if (format == CricketFormat.TestCricket)
            {
                allTossDecisions = allTestMatches.Select(x => x.TossDecision);
            }
            else
            {
                allTossDecisions = allLoiMatches[format].Select(x => x.TossDecision);
            }

            foreach (var tossDecision in allTossDecisions)
            {
                tossDecision.Should().NotBeNullOrEmpty();
            }

            allTossDecisions.Count(x => x.EndsWith(", elected to field first")).Should().Be(eleFieldFirstCount);
            allTossDecisions.Count(x => x.EndsWith(", elected to bat first")).Should().Be(eleBatFirstCount);
            allTossDecisions.Count(x => x.Equals("no toss")).Should().Be(noTossCount);
        }

        [Theory]
        [InlineData(CricketFormat.ODI, 282)]
        [InlineData(CricketFormat.WODI, 0)]
        [InlineData(CricketFormat.T20I, 251)]
        [InlineData(CricketFormat.WT20I, 282)]
        [InlineData(CricketFormat.TestCricket, 162)]
        public void AllInternationalMatches_Venues_ShouldHaveCorrectFormatAndValue(CricketFormat format, int distinctVenueCounts)
        {
            IEnumerable<string> allVenues = new List<string>();

            if (format == CricketFormat.TestCricket)
            {
                allVenues = allTestMatches.Select(x => x.Venue);
            }
            else
            {
                allVenues = allLoiMatches[format].Select(x => x.Venue);
            }

            foreach (var venue in allVenues)
            {
                venue.Should().NotBeNullOrEmpty();
            }

            allVenues.Distinct().Should().HaveCount(distinctVenueCounts);
        }

        [Fact] //Do it later
        public void AllSeriesResultsShouldHaveCorrectFormatAndValue()
        {
            var odiSeriesResults = allLoiMatches[CricketFormat.ODI].Select(x => x.SeriesResult);

            foreach (var odiSeriesResult in odiSeriesResults)
            {
                odiSeriesResult.Should().NotBeNull();
            }

            odiSeriesResults.Count(x => x == string.Empty).Should().Be(2019);
            odiSeriesResults.Count(x => x.Length > 0).Should().Be(2851);

            var t20iSeriesResults = allLoiMatches[CricketFormat.T20I].Select(x => x.SeriesResult);

            foreach (var t20iSeriesResult in t20iSeriesResults)
            {
                t20iSeriesResult.Should().NotBeNull();
            }

            t20iSeriesResults.Count(x => x == string.Empty).Should().Be(1766);
            t20iSeriesResults.Count(x => x.Length > 0).Should().Be(1416);
        }

        [Fact] //Do it later
        public void AllTvUmpiresShouldHaveCorrectFormatAndValue()
        {
            var odiTvUmpires = allLoiMatches[CricketFormat.ODI].Select(x => x.TvUmpire);

            foreach (var odiTvUmpire in odiTvUmpires)
            {
                odiTvUmpire.Should().NotBeNull();
            }

            odiTvUmpires.Count(x => x == string.Empty).Should().Be(1388);
            odiTvUmpires.Count(x => x.Length > 0).Should().Be(3482);

            var t20iTvUmpires = allLoiMatches[CricketFormat.T20I].Select(x => x.TvUmpire);

            foreach (var t20iTvUmpire in t20iTvUmpires)
            {
                t20iTvUmpire.Should().NotBeNull();
            }

            t20iTvUmpires.Count(x => x == string.Empty).Should().Be(1796);
            t20iTvUmpires.Count(x => x.Length > 0).Should().Be(1386);
        }

        #region Team Scoreboard Details
        [Theory]
        [InlineData(CricketFormat.ODI, ODIMatchCount)]
        [InlineData(CricketFormat.WODI, WODIMatchCount)]
        [InlineData(CricketFormat.T20I, T20IMatchCount)]
        [InlineData(CricketFormat.WT20I, WT20IMatchCount)]
        public void AllInternationalMatches_Teams_ShouldHaveCorrectFormatAndValue(CricketFormat format, int tmc)
        {
            var allTeams1 = allLoiMatches[format].Select(x => x.Team1);
            var allTeams2 = allLoiMatches[format].Select(x => x.Team2);
            var allTeams = allTeams1.Concat(allTeams2);

            foreach (var team in allTeams)
            {
                team.Should().NotBeNull();
            }

            allTeams.Should().HaveCount(2 * tmc);
        }

        [Fact]
        public void TestCricket_AllInternationalMatches_Teams_ShouldHaveCorrectFormatAndValue()
        {
            var allTeams1 = allTestMatches.Select(x => x.Team1);
            var allTeams2 = allTestMatches.Select(x => x.Team2);
            var allTeams = allTeams1.Concat(allTeams2);

            foreach (var team in allTeams)
            {
                team.Should().NotBeNull();
            }

            allTeams.Should().HaveCount(2 * TestMatchCount);
        }

        [Fact]
        public void ODI_AllTeam_TeamsShouldHaveCorrectFormatAndValue()
        {
            var allOdiMatches = allLoiMatches[CricketFormat.ODI].ToList();
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
            var allT20iMatches = allLoiMatches[CricketFormat.T20I].ToList();
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
            var allOdiMatches = allLoiMatches[CricketFormat.ODI].ToList();
            var odiTeam1BSBs = allOdiMatches.Select(x => x.Team1.BattingScoreCard);
            var odiTeam2BSBs = allOdiMatches.Select(x => x.Team2.BattingScoreCard);
            var odiTeamBSBs = odiTeam1BSBs.Concat(odiTeam2BSBs);

            odiTeamBSBs.Count(x => x == null).Should().Be(0);
            odiTeamBSBs.Count(x => x.Count > 12).Should().Be(0);
            odiTeamBSBs.Count(x => x.Count == 12).Should().Be(3);
            odiTeam1BSBs.Count(x => x.Count == 0).Should().Be(6);
            odiTeam2BSBs.Count(x => x.Count == 0).Should().Be(105);
        }

        [Fact]
        public void T20I_AllTeam_BattingScoreboardsShouldHaveCorrectFormatAndValue()
        {
            var allT20iMatches = allLoiMatches[CricketFormat.T20I].ToList();
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
        [InlineData(CricketFormat.WODI)]
        [InlineData(CricketFormat.T20I)]
        [InlineData(CricketFormat.WT20I)]
        public void AllTeam_BattingScoreboard_PlayerNamesShouldHaveCorrectFormatAndValue(CricketFormat format)
        {
            var allfilteredMatches = allLoiMatches[format].ToList();
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
                matchTeamBSB.Count(x => x.PlayerName.Name.Contains('�')).Should().BeInRange(0, 1);
            }
        }

        [Fact]
        public void ODI_AllTeam_BattingScoreboard_RunsScoredShouldHaveCorrectFormatAndValue()
        {
            var allOdiMatches = allLoiMatches[CricketFormat.ODI].ToList();
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
            var allT20IMatches = allLoiMatches[CricketFormat.T20I].ToList();
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
            var allOdiMatches = allLoiMatches[CricketFormat.ODI].ToList();
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
            var allT20IMatches = allLoiMatches[CricketFormat.T20I].ToList();
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
            var allOdiMatches = allLoiMatches[CricketFormat.ODI].ToList();
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
            flatBattingScoreboards.Count(x => x.OutStatus.Equals("absent hurt")).Should().Be(43);
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
            flatBattingScoreboards.Count(x => x.OutStatus.StartsWith("c �")).Should().BeInRange(9900, 10000);
            flatBattingScoreboards.Count(x => x.OutStatus.StartsWith("c ")).Should().BeInRange(40000, 50000);
            flatBattingScoreboards.Count(x => x.OutStatus.StartsWith("c & b ")).Should().BeInRange(2000, 3000);
            flatBattingScoreboards.Count(x => x.OutStatus.StartsWith(" b ")).Should().BeInRange(13000, 15000);
            flatBattingScoreboards.Count(x => x.OutStatus.StartsWith("st �")).Should().BeInRange(1500, 2000);
            flatBattingScoreboards.Count(x => x.OutStatus.StartsWith("st sub (�")).Should().Be(2);
        }

        [Fact]
        public void AllTeam_ExtrasShouldHaveCorrectFormatAndValue()
        {
            var odiTeam1Extras = allLoiMatches[CricketFormat.ODI].Select(x => x.Team1.Extras);
            var odiTeam2Extras = allLoiMatches[CricketFormat.ODI].Select(x => x.Team2.Extras);
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
            var odiTeam1Dnbs = allLoiMatches[CricketFormat.ODI].Select(x => x.Team1.DidNotBat);
            var odiTeam2Dnbs = allLoiMatches[CricketFormat.ODI].Select(x => x.Team2.DidNotBat);
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