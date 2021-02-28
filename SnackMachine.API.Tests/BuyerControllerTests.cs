using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SnackMachine.API.Contracts;
using SnackMachine.API.UseCases.InsertMoney;
using SnackMachine.Domain.AccountAggregate;
using Xunit;

namespace SnackMachine.API.Tests
{
    public class BuyerControllerTests
    {
        [Theory]
        [AccountDataSource]
        public async Task InsertMoneyAsync_NotAllowedCoin_ShouldReturnBadRequest(AccountController sut)
        {
            // Arrange
            var request = new InsertMoneyRequest(0.75m);

            // Act
            var result = await sut.InsertMoney(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            ((ObjectResult) result).Value.ToString().Should().Be($"{AccountCode.CoinNotRegistered}: coin not registered");
        }

        [Theory]
        [AccountDataSource]
        public async Task InsertMoneyAsync_AllowedCoin_ShouldReturnOk(
            [Frozen] Mock<IMachineRepository> machineRepositoryMock,
            AccountController sut)
        {
            // Arrange
            var request = new InsertMoneyRequest(0.50m);

            // Act
            var result = await sut.InsertMoney(request);

            // Assert
            machineRepositoryMock.Verify(x => x.SaveAsync(It.IsAny<Machine>()), Times.Once);
            result.Should().BeOfType<OkResult>();
        }
    }
}