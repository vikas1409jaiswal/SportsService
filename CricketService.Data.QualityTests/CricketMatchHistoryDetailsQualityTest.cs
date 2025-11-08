using CricketService.Data.Entities;
using CricketService.Domain.Enums;
using Humanizer;

namespace CricketService.Data.QualityTests
{
    public class CricketMatchHistoryDetailsQualityTest : IClassFixture<TestContext>
    {
        private const int ODIMatchCount = 4884;
        private const int WODIMatchCount = 0;
        private const int T20IMatchCount = 0;
        private const int WT20IMatchCount = 0;
        private const int TestMatchCount = 0;

        private readonly TestContext context;
        private readonly Dictionary<CricketFormat, IQueryable<CricketTeamHistoryDTO>> allCricketTeamsHistory = new();

        public CricketMatchHistoryDetailsQualityTest(TestContext context)
        {
            this.context = context;

            var cricketMatchesHistory = this.context.DbContext.CricketTeamsHistory;

            allCricketTeamsHistory[CricketFormat.All] = cricketMatchesHistory;
            allCricketTeamsHistory[CricketFormat.ODI] = cricketMatchesHistory.Where(x => x.Format.Equals(CricketFormat.ODI.ToString()));
            allCricketTeamsHistory[CricketFormat.WODI] = cricketMatchesHistory.Where(x => x.Format.Equals(CricketFormat.WODI.ToString()));
            allCricketTeamsHistory[CricketFormat.T20I] = cricketMatchesHistory.Where(x => x.Format.Equals(CricketFormat.T20I.ToString()));
            allCricketTeamsHistory[CricketFormat.WT20I] = cricketMatchesHistory.Where(x => x.Format.Equals(CricketFormat.WT20I.ToString()));
            allCricketTeamsHistory[CricketFormat.TestCricket] = cricketMatchesHistory.Where(x => x.Format.Equals(CricketFormat.TestCricket.ToString()));
        }

        [Theory]
        [InlineData(CricketFormat.ODI, ODIMatchCount)]
        [InlineData(CricketFormat.WODI, WODIMatchCount)]
        [InlineData(CricketFormat.T20I, T20IMatchCount)]
        [InlineData(CricketFormat.WT20I, WT20IMatchCount)]
        [InlineData(CricketFormat.TestCricket, TestMatchCount)]
        public void AllInternationalTeamsHistory_MatchesCount_ShouldHaveCorrectValue(CricketFormat format, int tmc)
        {
            allCricketTeamsHistory[format].Should().HaveCount(tmc);
        }

        [Theory]
        [InlineData(CricketFormat.ODI, ODIMatchCount)]
        [InlineData(CricketFormat.WODI, WODIMatchCount)]
        [InlineData(CricketFormat.T20I, T20IMatchCount)]
        [InlineData(CricketFormat.WT20I, WT20IMatchCount)]
        [InlineData(CricketFormat.TestCricket, TestMatchCount)]
        public void AllInternationalTeamsHistory_MatchNumbers_ShouldHaveCorrectValue(CricketFormat format, int tmc)
        {
            foreach (var cth in allCricketTeamsHistory[format])
            {
                cth.MatchNumber.Should().BeGreaterThan(0)
                    .And.BeLessThanOrEqualTo(tmc);
            }

            allCricketTeamsHistory[format].Select(x => x.MatchNumber)
                .Distinct()
                .Should().HaveCount(tmc);
        }

        [Theory]
        [InlineData(CricketFormat.ODI, 29)]
        [InlineData(CricketFormat.WODI, 0)]
        [InlineData(CricketFormat.T20I, 0)]// 108)]
        [InlineData(CricketFormat.WT20I, 0)]// 91)]
        [InlineData(CricketFormat.TestCricket, 0)]// 12)]
        public void AllInternationalTeamsHistory_InstantTeamsRecords_ShouldHaveCorrectValue(CricketFormat format, int disTeamCount)
        {
            foreach (var cth in allCricketTeamsHistory[format])
            {
                cth.InstantTeamsRecords.Should().NotBeNull();
                cth.InstantTeamsRecords.Should().HaveCount(disTeamCount);
            }
        }

        [Theory]
        [InlineData(CricketFormat.ODI)]
        [InlineData(CricketFormat.WODI)]
        [InlineData(CricketFormat.T20I)]
        [InlineData(CricketFormat.WT20I)]
        [InlineData(CricketFormat.TestCricket)]
        public void AllInternationalTeamsHistory_InstantTeamsRecords_TeamName_ShouldHaveCorrectValue(CricketFormat format)
        {
            foreach (var cths in allCricketTeamsHistory[format])
            {
                foreach (var itr in cths.InstantTeamsRecords)
                {
                    itr.TeamName.Should().NotBeNullOrEmpty();
                }
            }

            if (allCricketTeamsHistory[format].Count() > 0)
            {
                var expectedTeamNames = allCricketTeamsHistory[format].First().InstantTeamsRecords.Select(t => t.TeamName);

                foreach (var cths in allCricketTeamsHistory[format])
                {
                    cths.InstantTeamsRecords.Select(x => x.TeamName).Should().Equal(expectedTeamNames);
                }
            }
        }
    }
}
