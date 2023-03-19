using CricketService.Api.Controllers;
using CricketService.Api.UnitTests.Mocks;
using CricketService.Data.Repositories.Interfaces;
using CricketService.Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CricketService.Api.UnitTests.Controllers
{
    public class CricketMatchControllerTest
    {
        private Mock<ICricketMatchRepository> mockCricketMatchRepository = new Mock<ICricketMatchRepository>();

        //[Fact]
        //public void Ctor_WhenNullRepository_ThenThrowsException()
        //{
        //    // Arrange
        //    ICricketMatchRepository nullRepository = null!;

        //    // Act
        //    Action act = () => new CricketMatchController(nullRepository);

        //    // Assert
        //    act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'cricketMatchRepository is null')");
        //}

        //[Fact]
        //public void GetAllMatchesT20I_WhenEmptyMatchesList_Then200OKEmpty()
        //{
        //    // Arrange
        //    var cmController = CreateCricketMatchController(out mockCricketMatchRepository);

        //    mockCricketMatchRepository
        //        .Setup(repo => repo.GetAllMatchesT20I())
        //        .Returns(new List<CricketMatchInfoResponse>().AsEnumerable());

        //    // Act
        //    var response = cmController.GetAllMatchesT20I();

        //    // Assert
        //    response.Should().BeOfType<ActionResult<IEnumerable<CricketMatchInfoResponse>>>();
        //    response.Result.Should().NotBeNull();
        //    var result = response.Result.Should().BeOfType<OkObjectResult>();
        //    result.Which.StatusCode.Should().Be(200);
        //    result.Which.Value.Should().NotBeNull();
        //    result.Which.Value.Should().BeEquivalentTo(new List<CricketMatchInfoResponse>());
        //    cmController.Response.Headers.Keys.Select(k => k.Should().Contain("total-t20I-matches"));
        //    cmController.Response.Headers["total-t20I-matches"].ToString().Should().BeSameAs("0");
        //}

        //[Fact]
        //public void GetAllMatchesT20I_WhenNonEmptyMatchesList_Then200OKNonEmpty()
        //{
        //    // Arrange
        //    var cmController = CreateCricketMatchController(out mockCricketMatchRepository);

        //    var matches = CricketMatchInfoResponseMock.GetCricketMatchInfoResponses();

        //    mockCricketMatchRepository
        //        .Setup(repo => repo.GetAllMatchesT20I())
        //        .Returns(matches);

        //    // Act
        //    var response = cmController.GetAllMatchesT20I();

        //    // Assert
        //    response.Should().BeOfType<ActionResult<IEnumerable<CricketMatchInfoResponse>>>();
        //    response.Result.Should().NotBeNull();
        //    var result = response.Result.Should().BeOfType<OkObjectResult>();
        //    result.Which.StatusCode.Should().Be(200);
        //    result.Which.Value.Should().BeEquivalentTo(matches);
        //    cmController.Response.Headers.Keys.Select(k => k.Should().Contain("total-t20I-matches"));
        //    cmController.Response.Headers["total-t20I-matches"].ToString().Should().BeSameAs("2");
        //}

        //[Fact]
        //public void GetAllMatchesODI_WhenEmptyMatchesList_Then200OKEmptyList()
        //{
        //    // Arrange
        //    var cmController = CreateCricketMatchController(out mockCricketMatchRepository);

        //    mockCricketMatchRepository
        //        .Setup(repo => repo.GetAllMatchesODI())
        //        .Returns(new List<CricketMatchInfoResponse>().AsEnumerable());

        //    // Act
        //    var response = cmController.GetAllMatchesODI();

        //    // Assert
        //    response.Should().BeOfType<ActionResult<IEnumerable<CricketMatchInfoResponse>>>();
        //    response.Result.Should().NotBeNull();
        //    var result = response.Result.Should().BeOfType<OkObjectResult>();
        //    result.Which.StatusCode.Should().Be(200);
        //    result.Which.Value.Should().NotBeNull();
        //    result.Which.Value.Should().BeEquivalentTo(new List<CricketMatchInfoResponse>());
        //    cmController.Response.Headers.Keys.Select(k => k.Should().Contain("total-ODI-matches"));
        //    cmController.Response.Headers["total-ODI-matches"].ToString().Should().BeSameAs("0");
        //}

        //[Fact]
        //public void GetAllMatchesODI_WhenNonEmptyMatchesList_Then200OKNonEmptyList()
        //{
        //    // Arrange
        //    var cmController = CreateCricketMatchController(out mockCricketMatchRepository);

        //    var matches = CricketMatchInfoResponseMock.GetCricketMatchInfoResponses(3);

        //    mockCricketMatchRepository
        //        .Setup(repo => repo.GetAllMatchesODI())
        //        .Returns(matches);

        //    // Act
        //    var response = cmController.GetAllMatchesODI();

        //    // Assert
        //    response.Should().BeOfType<ActionResult<IEnumerable<CricketMatchInfoResponse>>>();
        //    response.Result.Should().NotBeNull();
        //    var result = response.Result.Should().BeOfType<OkObjectResult>();
        //    result.Which.StatusCode.Should().Be(200);
        //    result.Which.Value.Should().BeEquivalentTo(matches);
        //    cmController.Response.Headers.Keys.Select(k => k.Should().Contain("total-ODI-matches"));
        //    cmController.Response.Headers["total-ODI-matches"].ToString().Should().BeSameAs("3");
        //}

        // [Fact]
        // public async void GetMatchByMNumberT20I_WhenEmptyMatch_Then200OKEmpty()
        // {
        //    // Arrange
        //    var cmController = CreateCricketMatchController(out mockCricketMatchRepository);

        // mockCricketMatchRepository
        //        .Setup(repo => repo.GetMatchByMNumberT20I(It.IsAny<int>()))
        //        .Returns(Task.FromResult(CricketMatchInfoResponseMock.GetCricketMatchInfoResponses(0)));

        // // Act
        //    var response = await cmController.GetMatchByMNumberT20I(300);

        // // Assert
        //    response.Should().BeOfType<ActionResult<IEnumerable<CricketMatchInfoResponse>>>();
        //    response.Result.Should().NotBeNull();
        //    var result = response.Result.Should().BeOfType<OkObjectResult>();
        //    result.Which.StatusCode.Should().Be(200);
        //    result.Which.Value.Should().NotBeNull();
        //    result.Which.Value.Should().BeEquivalentTo(new List<CricketMatchInfoResponse>());
        // }
        private CricketMatchController CreateCricketMatchController(out Mock<ICricketMatchRepository> cricketMatchRepository)
        {
            cricketMatchRepository = new Mock<ICricketMatchRepository>();

            var cricketMatchController = new CricketMatchController(cricketMatchRepository.Object)
            {
                ControllerContext = new ControllerContext(),
            };

            cricketMatchController.ControllerContext.HttpContext = new DefaultHttpContext();

            return cricketMatchController;
        }
    }
}
