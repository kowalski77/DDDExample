using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
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

        public AddSnackShould(BaseTestWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            this.httpClient = this.factory.CreateClient();
        }

        [Fact]
        public async Task Test1()
        {
            // Arrange
            var snack = this.factory.Fixture.Create<Snack>();
            await this.factory.SnacksCollection.InsertOneAsync(snack);
            var storedSnack = (await this.factory.SnacksCollection.FindAsync<Snack>(Builders<Snack>.Filter.Eq(x=>x.Name, snack.Name))).FirstOrDefault();

            var response = await this.httpClient.GetAsync($"{IntegrationTestConstants.SnackUrl}/{storedSnack.Id}");

        }
    }
}