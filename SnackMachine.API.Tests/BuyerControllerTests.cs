using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SnackMachine.API.Contracts;
using SnackMachine.API.UseCases;
using SnackMachine.Domain.MachineAggregate;
using Xunit;

namespace SnackMachine.API.Tests
{
    public class BuyerControllerTests
    {
        [Theory]
        [BuyerDataSource]
        public async Task InsertMoneyAsync_NotAllowedCoin_ShouldReturnBadRequest(BuyerController sut)
        {
            // Arrange
            var request = new InsertMoneyRequest(0.75m);

            // Act
            var result = await sut.Insert(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            ((ObjectResult) result).Value.ToString().Should().Be($"{AccountCode.CoinNotRegistered}: coin not registered");
        }

        [Theory]
        [BuyerDataSource]
        public async Task InsertMoneyAsync_AllowedCoin_ShouldReturnOk(
            [Frozen] Mock<IMachineRepository> machineRepositoryMock,
            BuyerController sut)
        {
            // Arrange
            var request = new InsertMoneyRequest(0.50m);

            // Act
            var result = await sut.Insert(request);

            // Assert
            machineRepositoryMock.Verify(x => x.SaveAsync(It.IsAny<Machine>()), Times.Once);
            result.Should().BeOfType<OkResult>();
        }
    }
}