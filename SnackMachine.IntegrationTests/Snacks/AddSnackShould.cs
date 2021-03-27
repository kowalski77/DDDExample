using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using SnackMachine.API;
using SnackMachine.API.UseCases.AddSnack;
using Xunit;

namespace SnackMachine.IntegrationTests.Snacks
{
    [Collection(IntegrationTestConstants.TestWebApplicationFactoryCollection)]
    public class AddSnackShould
    {
        private readonly TestWebApplicationFactory<Startup> factory;

        private readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public AddSnackShould(TestWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task store_a_snack_when_valid_request()
        {
            // Arrange
            var request = new AddSnackModel.Request(this.factory.Fixture.Create<string>(), this.factory.Fixture.Create<decimal>());
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            // Act
            var response = await this.factory.HttpClient.PostAsync(IntegrationTestConstants.SnackUrl, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseSnack = JsonSerializer.Deserialize<AddSnackModel.Response>(await response.Content.ReadAsStringAsync(), this.jsonSerializerOptions);
            responseSnack?.Name.Should().Be(request.Name);
            responseSnack?.Price.Should().Be(request.Price);
        }
    }
}