using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SnackMachine.API.UseCases.InsertMoney;
using SnackMachine.Domain.AccountAggregate;
using Xunit;

namespace SnackMachine.API.Tests
{
    public class InsertMoneyShould
    {
        [Theory, ControllerDataSource]
        public async Task Create_BadRequest_result_when_request_is_null(
            [Frozen] Mock<IAccountRepository> accountRepositoryMock,
            AccountController sut)
        {
            // Act
            var result = await sut.InsertMoney(null!);

            // Assert
            accountRepositoryMock.Verify(x => x.UpdateAccountAsync(It.IsAny<Account>()), Times.Never);
            result.Should().BeOfType<BadRequestObjectResult>();
            ((ObjectResult)result).Value.ToString().Should().Be($"Request {nameof(InsertMoneyRequest)} is null");
        }

        [Theory, ControllerDataSource]
        public async Task Create_NotFound_result_when_no_account_is_available(
            [Frozen] Mock<IAccountRepository> accountRepositoryMock,
            InsertMoneyRequest request,
            AccountController sut)
        {
            // Act
            var result = await sut.InsertMoney(request);

            // Assert
            accountRepositoryMock.Verify(x => x.UpdateAccountAsync(It.IsAny<Account>()), Times.Never);
            result.Should().BeOfType<NotFoundObjectResult>();
            ((ObjectResult)result).Value.ToString().Should().Be("No account available");
        }

        [Theory, AccountDataSource]
        public async Task Create_BadRequest_result_when_inserting_not_allowed_money(
            [Frozen] Mock<IAccountRepository> accountRepositoryMock,
            AccountController sut)
        {
            // Arrange
            var request = new InsertMoneyRequest(0.75m);

            // Act
            var result = await sut.InsertMoney(request);

            // Assert
            accountRepositoryMock.Verify(x => x.UpdateAccountAsync(It.IsAny<Account>()), Times.Never);
            result.Should().BeOfType<BadRequestObjectResult>();
            ((ObjectResult)result).Value.ToString().Should().Be($"{AccountCode.CoinNotRegistered}: coin not registered");
        }

        [Theory, AccountDataSource]
        public async Task Create_Ok_result_when_inserting_allowed_money(
            [Frozen] Mock<IAccountRepository> accountRepositoryMock,
            AccountController sut)
        {
            // Arrange
            var request = new InsertMoneyRequest(0.50m);

            // Act
            var result = await sut.InsertMoney(request);

            // Assert
            accountRepositoryMock.Verify(x => x.UpdateAccountAsync(It.IsAny<Account>()), Times.Once);
            result.Should().BeOfType<NoContentResult>();
        }
    }
}