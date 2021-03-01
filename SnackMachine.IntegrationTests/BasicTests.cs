using System.Threading.Tasks;
using SnackMachine.API;
using Xunit;

namespace SnackMachine.IntegrationTests
{
    public class BasicTests : IClassFixture<TestWebApplicationFactory<Startup>>
    {
        private readonly TestWebApplicationFactory<Startup> factory;

        public BasicTests(TestWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task Test1()
        {
            const string url = "/api/v1/Snack";

            var client = this.factory.CreateClient();

            var response = client.GetAsync("/");

            //var response = await client.PostAsync();
        }
    }
}