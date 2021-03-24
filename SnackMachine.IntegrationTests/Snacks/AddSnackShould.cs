using System.Threading.Tasks;
using SnackMachine.API;
using Xunit;

namespace SnackMachine.IntegrationTests.Snacks
{
    public class AddSnackShould : IClassFixture<SnackTestWebApplicationFactory<Startup>>
    {
        private readonly SnackTestWebApplicationFactory<Startup> factory;

        public AddSnackShould(SnackTestWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task Test1()
        {
            const string url = "/api/v1/Snack";

            var client = this.factory.CreateClient();
            var response = await client.GetAsync($"{url}/{this.factory.Snack.Id}");

            //var response = await client.PostAsync();
        }
    }
}