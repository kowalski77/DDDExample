using System;
using System.Net;
using System.Net.Http;
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
    public class AddSnackShould : IClassFixture<TestWebApplicationFactory<Startup>>
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
        public async Task Return_success_and_correct_snack_when_existing_snackId()
        {
            // Arrange
            var snack = this.factory.Fixture.Create<Snack>();
            await this.factory.SnacksCollection.InsertOneAsync(snack);
            var storedSnack = (await this.factory.SnacksCollection.FindAsync<Snack>(Builders<Snack>.Filter.Eq(x => x.Name, snack.Name))).FirstOrDefault();

            // Act
            var response = await this.factory.HttpClient.GetAsync($"{IntegrationTestConstants.SnackUrl}/{storedSnack.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseSnack = JsonSerializer.Deserialize<GetSnackModel.SnackResponse>(await response.Content.ReadAsStringAsync(), this.jsonSerializerOptions);
            responseSnack?.Name.Should().Be(snack.Name.Value);
            responseSnack?.Price.Should().Be(snack.Price.Value);
        }

        [Fact]
        public async Task Return_not_found_when_non_existing_snackId()
        {
            // Act
            var response = await this.factory.HttpClient.GetAsync($"{IntegrationTestConstants.SnackUrl}/{Guid.NewGuid()}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}