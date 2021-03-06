using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MongoDB.Driver;
using SnackMachine.API;
using SnackMachine.API.UseCases.GetSnack;
using SnackMachine.Domain.SnackAggregate;
using Xunit;

namespace SnackMachine.IntegrationTests.Snacks
{
    [Collection(IntegrationTestConstants.TestWebApplicationFactoryCollection)]
    public class GetSnackShould
    {
        private readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly TestWebApplicationFactory<Startup> factory;

        public GetSnackShould(TestWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task Return_correct_snack_when_existing_snackId()
        {
            // Arrange
            var snack = this.factory.Fixture.Create<Snack>();
            await this.factory.SnacksCollection.InsertOneAsync(snack);
            var storedSnack = (await this.factory.SnacksCollection.FindAsync<Snack>(Builders<Snack>.Filter.Eq(x => x.Name, snack.Name))).FirstOrDefault();

            // Act
            var response = await this.factory.HttpClient.GetAsync($"{IntegrationTestConstants.SnackUrl}/{storedSnack.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseSnack = JsonSerializer.Deserialize<GetSnackModel.Response>(await response.Content.ReadAsStringAsync(), this.jsonSerializerOptions);
            responseSnack?.Name.Should().Be(snack.Name.Value);
            responseSnack?.Price.Should().Be(snack.Price.Value);
        }
    }
}