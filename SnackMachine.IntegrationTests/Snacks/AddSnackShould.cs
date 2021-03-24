using System.Threading.Tasks;
using SnackMachine.API;
using Xunit;

namespace SnackMachine.IntegrationTests.Snacks
{
    public class AddSnackShould : IClassFixture<BaseTestWebApplicationFactory<Startup>>
    {
        private readonly BaseTestWebApplicationFactory<Startup> factory;

        public AddSnackShould(BaseTestWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task Test1()
        {
            const string url = "/api/v1/Snack";

            var client = this.factory.CreateClient();

            var response = await client.GetAsync("/{758E005A-4792-42DB-A207-72ED8E0C023F}");

            //var response = await client.PostAsync();
        }
    }
}