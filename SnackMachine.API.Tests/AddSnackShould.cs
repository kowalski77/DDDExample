using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SnackMachine.API.UseCases.AddSnack;
using SnackMachine.Domain.SnackAggregate;
using Xunit;

namespace SnackMachine.API.Tests
{
    public class AddSnackShould
    {
        [Theory, ControllerDataSource]
        public async Task Create_BadRequest_result_when_request_is_null(SnackController sut)
        {
            // Act
            var result = await sut.AddSnack(null!);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            ((ObjectResult)result).Value.ToString().Should().Be($"Request {nameof(AddSnackModel.Request)} is null");
        }

        [Theory, ControllerDataSource]
        public async Task Create_Ok_result_when_transaction_completed_successfully(
            [Frozen] Mock<ISnackRepository> snackRepositoryMock,
            AddSnackModel.Request request,
            Snack snack,
            SnackController sut)
        {
            // Arrange
            snackRepositoryMock.Setup(x => x.AddSnack(It.IsAny<Snack>())).ReturnsAsync(snack);

            // Act
            var result = await sut.AddSnack(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}