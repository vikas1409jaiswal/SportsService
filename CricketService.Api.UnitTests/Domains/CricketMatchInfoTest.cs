using AutoFixture.Xunit2;
using CricketService.Domain;
using FluentAssertions;
using Xunit;

namespace CricketService.Api.UnitTests.Domains
{
    public class CricketMatchInfoTest
    {
        [Theory]
        [InlineAutoData]
        public void FieldsShouldNotBeNullOrWhiteSpace(CricketMatchInfo cmInfo)
        {
            cmInfo.Season.Should().NotBeNullOrWhiteSpace();
            cmInfo.Series.Should().NotBeNullOrWhiteSpace();
            cmInfo.MatchNo.Should().NotBeNullOrWhiteSpace();
            cmInfo.MatchTitle.Should().NotBeNullOrWhiteSpace();
            cmInfo.Venue.Should().NotBeNullOrWhiteSpace();
            cmInfo.MatchDate.Should().NotBeNullOrWhiteSpace();
            cmInfo.Result.Should().NotBeNullOrWhiteSpace();
        }
    }
}
