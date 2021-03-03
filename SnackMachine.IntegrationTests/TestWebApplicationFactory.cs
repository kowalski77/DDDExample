using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SnackMachine.MongoDbPersistence;

namespace SnackMachine.IntegrationTests
{
    public sealed class TestWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                var mongoConfigurationDescriptor = services.SingleOrDefault(x => x.ServiceType == typeof(IConfigureOptions<MongoDbConfiguration>));
                services.Remove(mongoConfigurationDescriptor);

                services.Configure<MongoDbConfiguration>(context.Configuration.GetSection(nameof(MongoDbConfiguration)));
            });
        }
    }
}