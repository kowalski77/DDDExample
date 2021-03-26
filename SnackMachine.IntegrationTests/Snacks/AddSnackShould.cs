using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MongoDB.Driver;
using SnackMachine.API;
using SnackMachine.Domain.SnackAggregate;
using Xunit;

namespace SnackMachine.IntegrationTests.Snacks
{
    public class AddSnackShould : IClassFixture<BaseTestWebApplicationFactory<Startup>>
    {
        private readonly BaseTestWebApplicationFactory<Startup> factory;
        private readonly HttpClient httpClient;

        private readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public AddSnackShould(BaseTestWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            this.httpClient = this.factory.CreateClient();
        }

        [Fact]
        public async Task Return_success_and_correct_snack_when_existing_snackId()
        {
            // Arrange
            var snack = this.factory.Fixture.Create<Snack>();
            await this.factory.SnacksCollection.InsertOneAsync(snack);
            var storedSnack = (await this.factory.SnacksCollection.FindAsync<Snack>(Builders<Snack>.Filter.Eq(x => x.Name, snack.Name))).FirstOrDefault();

            // Act
            var response = await this.httpClient.GetAsync($"{IntegrationTestConstants.SnackUrl}/{storedSnack.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseSnack = JsonSerializer.Deserialize<SnackTestDto>(await response.Content.ReadAsStringAsync(), this.jsonSerializerOptions);
            responseSnack?.Name.Should().Be(snack.Name.Value);
            responseSnack?.Price.Should().Be(snack.Price.Value);
        }
    }
}