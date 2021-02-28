using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SnackMachine.API.UseCases.AddSnack;
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
            ((ObjectResult)result).Value.ToString().Should().Be($"Request {nameof(AddSnackRequest)} is null");
        }

        [Theory, AccountDataSource]
        public async Task Create_Ok_result_when_transaction_completed_successfully(
            AddSnackRequest request,
            SnackController sut)
        {
            // Act
            var result = await sut.AddSnack(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}