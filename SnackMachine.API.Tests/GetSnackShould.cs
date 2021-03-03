using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SnackMachine.API.UseCases.GetSnack;
using SnackMachine.Domain.SnackAggregate;
using Xunit;

namespace SnackMachine.API.Tests
{
    public class GetSnackShould
    {
        [Theory, ControllerDataSource]
        public async Task Create_OkResult_with_snack_response_when_snack_does_exist(
            [Frozen] Mock<ISnackRepository> snackRepositoryMock,
            Guid snackId,
            Snack snack,
            SnackController sut)
        {
            // Arrange
            snackRepositoryMock.Setup(x => x.GetSnackAsync(snackId)).ReturnsAsync(snack);

            // Act
            var result = await sut.GetSnack(snackId);

            // Arrange
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<GetSnackModel.GetSnackResponse>();
        }

        [Theory, ControllerDataSource]
        public async Task Create_BadRequest_response_when_snack_does_not_exist(
            Guid snackId,
            SnackController sut)
        {
            // Act
            var result = await sut.GetSnack(snackId);

            // Arrange
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}