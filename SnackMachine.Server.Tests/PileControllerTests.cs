using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SnackMachine.API.Contracts;
using SnackMachine.API.Controllers;
using SnackMachine.Domain.MachineAggregate;
using Xunit;

namespace SnackMachine.Server.Tests
{
    public class PileControllerTests
    {
        [Theory]
        [ControllerDataSource]
        public async Task AddSnack_WithNotExistingSnackId_ShouldReturnBadRequest(
            AddSnackRequest request,
            PileController sut)
        {
            // Act
            var result = await sut.AddSnack(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            ((ObjectResult) result).Value.ToString().Should().Be($"Snack with id: {request.SnackId} does not exists");
        }

        [Theory]
        [MachineWithPilesDataSource]
        public async Task AddSnack_WithFullPile_ShouldReturnBadRequest(
            AddSnackRequest request,
            PileController sut)
        {
            // Arrange
            request.Pile = 0;

            // Act
            var result = await sut.AddSnack(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            ((ObjectResult) result).Value.ToString().Should().Be($"Cannot add more snacks in pile: {request.Pile}");
        }

        [Theory]
        [MachineWithPilesDataSource]
        public async Task AddSnack_WithPileWithSpaceAvailable_ShouldAddSnackAndSaveTheData(
            [Frozen] Mock<IMachineRepository> machineRepositoryMock,
            AddSnackRequest request,
            PileController sut)
        {
            // Arrange
            request.Pile = 1;

            // Act
            var result = await sut.AddSnack(request);

            // Assert
            machineRepositoryMock.Verify(x => x.SaveAsync(It.IsAny<Machine>()), Times.Once);
            result.Should().BeOfType<OkResult>();
        }
    }
}