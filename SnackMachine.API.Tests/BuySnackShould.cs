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
        public async Task Create_BadRequest_result_when_no_machine_is_registered(
            [Frozen] Mock<IMachineRepository> machineRepositoryMock,
            BuySnackRequest request,
            AccountController sut)
        {
            // Act
            var result = await sut.BuySnack(request);

            // Assert
            machineRepositoryMock.Verify(x => x.SaveAsync(It.IsAny<Machine>()), Times.Never);
            result.Should().BeOfType<BadRequestObjectResult>();
            ((ObjectResult)result).Value.ToString().Should().Be("There is no main machine registered");
        }

        [Theory, MachineWithNoSnacksDataSource]
        public async Task Create_BadRequest_result_when_snack_does_not_exists(
            [Frozen] Mock<IMachineRepository> machineRepositoryMock,
            BuySnackRequest request,
            AccountController sut)
        {
            // Act
            var result = await sut.BuySnack(request);

            // Assert
            machineRepositoryMock.Verify(x => x.SaveAsync(It.IsAny<Machine>()), Times.Never);
            result.Should().BeOfType<BadRequestObjectResult>();
            ((ObjectResult)result).Value.ToString().Should().Be($"Snack with id: {request.SnackId} does not exists");
        }

        [Theory, MachineWithSnacksDataSource]
        public async Task Create_BadRequest_result_when_transaction_cannot_be_completed(
            [Frozen] Mock<IMachineRepository> machineRepositoryMock,
            [Frozen] Mock<IAccountService> accountServiceMock,
            BuySnackRequest request,
            AccountController sut)
        {
            // Arrange
            accountServiceMock.Setup(x => x.BuyWithExchange(It.IsAny<Account>(), It.IsAny<Snack>()))
                .Returns(() => Result<AccountCode>.Fail(AccountCode.NotEnoughChange));

            // Act
            var result = await sut.BuySnack(request);

            // Assert
            machineRepositoryMock.Verify(x => x.SaveAsync(It.IsAny<Machine>()), Times.Never);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Theory, MachineWithSnacksDataSource]
        public async Task Create_Ok_result_when_transaction_is_completed(
            [Frozen] Mock<IMachineRepository> machineRepositoryMock,
            [Frozen] Mock<IAccountService> accountServiceMock,
            BuySnackRequest request,
            AccountController sut)
        {
            // Arrange
            accountServiceMock.Setup(x => x.BuyWithExchange(It.IsAny<Account>(), It.IsAny<Snack>()))
                .Returns(() => Result<AccountCode>.Ok(AccountCode.Ok));

            // Act
            var result = await sut.BuySnack(request);

            // Assert
            machineRepositoryMock.Verify(x => x.SaveAsync(It.IsAny<Machine>()), Times.Once);
            result.Should().BeOfType<OkResult>();
        }
    }
}