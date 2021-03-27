using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SnackMachine.API.UseCases.BuySnack;
using SnackMachine.Domain.AccountAggregate;
using SnackMachine.Domain.DomainServices;
using SnackMachine.Domain.SnackAggregate;
using SnackMachine.Domain.Utils;
using Xunit;

namespace SnackMachine.API.Tests
{
    public class BuySnackShould
    {
        [Theory, ControllerDataSource]
        public async Task Create_BadRequest_result_when_request_is_null(
            [Frozen] Mock<IAccountRepository> accountRepositoryMock,
            AccountController sut)
        {
            // Act
            var result = await sut.BuySnack(null!);

            // Assert
            accountRepositoryMock.Verify(x => x.UpdateAccountAsync(It.IsAny<Account>()), Times.Never);
            result.Should().BeOfType<BadRequestObjectResult>();
            ((ObjectResult)result).Value.ToString().Should().Be($"Request {nameof(BuySnackModel.Request)} is null");
        }

        [Theory, ControllerDataSource]
        public async Task Create_NotFound_result_when_no_account_is_available(
            [Frozen] Mock<IAccountRepository> accountRepositoryMock,
            BuySnackModel.Request request,
            AccountController sut)
        {
            // Act
            var result = await sut.BuySnack(request);

            // Assert
            accountRepositoryMock.Verify(x => x.UpdateAccountAsync(It.IsAny<Account>()), Times.Never);
            result.Should().BeOfType<NotFoundObjectResult>();
            ((ObjectResult)result).Value.ToString().Should().Be("No account available");
        }

        [Theory, AccountDataSource]
        public async Task Create_NotFound_result_when_snack_does_not_exists(
            [Frozen] Mock<IAccountRepository> accountRepositoryMock,
            BuySnackModel.Request request,
            AccountController sut)
        {
            // Act
            var result = await sut.BuySnack(request);

            // Assert
            accountRepositoryMock.Verify(x => x.UpdateAccountAsync(It.IsAny<Account>()), Times.Never);
            result.Should().BeOfType<NotFoundObjectResult>();
            ((ObjectResult)result).Value.ToString().Should().Be($"Snack with id: {request.SnackId} does not exists");
        }

        [Theory, MachineWithSnacksDataSource]
        public async Task Create_BadRequest_result_when_transaction_cannot_be_completed(
            [Frozen] Mock<IAccountRepository> accountRepositoryMock,
            [Frozen] Mock<IAccountService> accountServiceMock,
            BuySnackModel.Request request,
            AccountController sut)
        {
            // Arrange
            accountServiceMock.Setup(x => x.BuyWithExchange(It.IsAny<Account>(), It.IsAny<Snack>()))
                .Returns(() => Result<AccountCode>.Fail(AccountCode.NotEnoughChange));

            // Act
            var result = await sut.BuySnack(request);

            // Assert
            accountRepositoryMock.Verify(x => x.UpdateAccountAsync(It.IsAny<Account>()), Times.Never);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Theory, MachineWithSnacksDataSource]
        public async Task Create_Ok_result_when_transaction_is_completed(
            [Frozen] Mock<IAccountRepository> accountRepositoryMock,
            [Frozen] Mock<IAccountService> accountServiceMock,
            BuySnackModel.Request request,
            AccountController sut)
        {
            // Arrange
            accountServiceMock.Setup(x => x.BuyWithExchange(It.IsAny<Account>(), It.IsAny<Snack>()))
                .Returns(() => Result<AccountCode>.Ok(AccountCode.Ok));

            // Act
            var result = await sut.BuySnack(request);

            // Assert
            accountRepositoryMock.Verify(x => x.UpdateAccountAsync(It.IsAny<Account>()), Times.Once);
            result.Should().BeOfType<NoContentResult>();
        }
    }
}