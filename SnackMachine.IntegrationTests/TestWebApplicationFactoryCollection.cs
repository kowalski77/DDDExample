using SnackMachine.API;
using Xunit;

namespace SnackMachine.IntegrationTests
{
    [CollectionDefinition(IntegrationTestConstants.TestWebApplicationFactoryCollection)]
    public class TestWebApplicationFactoryCollection : ICollectionFixture<TestWebApplicationFactory<Startup>> { }
}