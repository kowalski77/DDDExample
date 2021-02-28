using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SnackMachine.API.UseCases.AddSnack;
using Xunit;

namespace SnackMachine.API.Tests
{
    public class AddSnackShould
    {
        [Theory, ControllerDataSource]
        public async Task Create_BadRequest_result_when_no_machine_is_registered(
            AddSnackRequest request,
            SnackController sut)
        {
            // Act
            var result = await sut.AddSnack(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            ((ObjectResult)result).Value.ToString().Should().Be("There is no main machine registered");
        }

        [Theory, MachineWithNoSnacksDataSource]
        public async Task Create_BadRequest_result_when_snack_does_not_exist(
            AddSnackRequest request,
            SnackController sut)
        {
            // Act
            var result = await sut.AddSnack(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            ((ObjectResult)result).Value.ToString().Should().Be($"Snack with id: {request.SnackId} does not exists");
        }

        [Theory, PileOneRequestDataSource]
        public async Task Create_OK_result_when_created(
            [Frozen] Mock<IMachineRepository> machineRepositoryMock,
            AddSnackRequest request,
            SnackController sut)
        {
            // Act
            var result = await sut.AddSnack(request);

            // Assert
            machineRepositoryMock.Verify(x => x.SaveAsync(It.IsAny<Machine>()), Times.Once);
            result.Should().BeOfType<OkResult>();
        }

        [Theory, PileZeroRequestDataSource]
        public async Task Create_BadRequest_result_when_there_is_not_enought_space(
            AddSnackRequest request,
            SnackController sut)
        {
            // Act
            var result = await sut.AddSnack(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            ((ObjectResult)result).Value.ToString().Should().Be($"Cannot add more snacks in pile: {request.Pile}");
        }
    }
}