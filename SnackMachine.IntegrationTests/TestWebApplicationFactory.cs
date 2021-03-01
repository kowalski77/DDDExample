using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using SnackMachine.MongoDbPersistence;

namespace SnackMachine.IntegrationTests
{
    public sealed class TestWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var mongoConfigurationDescriptor = services.SingleOrDefault(x => x.ServiceType == typeof(MongoDbConfiguration));
                services.Remove(mongoConfigurationDescriptor);

                services.AddSingleton(new MongoDbConfiguration
                {
                    AccountCollectionName = "AccountTest",
                    SnacksCollectionName = "SnacksTests",
                    DatabaseName = "SnackMachinesTests"
                });
            });
        }
    }
}