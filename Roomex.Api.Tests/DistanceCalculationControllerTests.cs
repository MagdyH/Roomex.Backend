using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Roomex.Api.Controllers;
using Roomex.Backend.Data;
using Roomex.Backend.Data.Dtos;
using Roomex.Backend.Services.Interfaces;
using Xunit;

namespace Roomex.Api.Tests
{
    public class DistanceCalculationControllerTests
    {
        private readonly Mock<IDistanceCalculationService> _distanceCalculationServiceMock;
        private readonly DistanceCalculationController _distanceCalculationController;
        private readonly Mock<ILogger<DistanceCalculationController>> _logger;
        private readonly Fixture _fixture;

        public DistanceCalculationControllerTests()
        {
            _distanceCalculationServiceMock = new Mock<IDistanceCalculationService>();
            _logger = new Mock<ILogger<DistanceCalculationController>>();
            _fixture = new Fixture();

            _distanceCalculationController = new DistanceCalculationController(_distanceCalculationServiceMock.Object, _logger.Object);
        }

        [Fact(DisplayName = "CalculateDistance - should return success result")]
        public void CalculateDistance_ShouldReturnSuccessResult()
        {
            //Arange
            var distanceCalculationResponse = _fixture.Create<double>();
            var request = _fixture.Create<RequestDto>();
            var response = _fixture.Create<ResponseDto>();
            response.Distance = distanceCalculationResponse;
            _distanceCalculationServiceMock.Setup(_ => _.CalculateDistanceInKM(It.IsAny<Point>(), It.IsAny<Point>())).Returns(distanceCalculationResponse);

            //Act
            var result = _distanceCalculationController.CalculateDistance(request) as OkObjectResult;

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(response);
        }
    }
}
