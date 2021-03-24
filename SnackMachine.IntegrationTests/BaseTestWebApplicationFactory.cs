using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SnackMachine.MongoDbPersistence;

namespace SnackMachine.IntegrationTests
{
    public class BaseTestWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        protected IFixture Fixture { get; } = new Fixture().Customize(new AutoMoqCustomization());

        protected override IHostBuilder CreateHostBuilder()
        {
            return base.CreateHostBuilder()
                .ConfigureHostConfiguration(config =>
                {
                    config.AddJsonFile("testing.json", false);
                    config.AddEnvironmentVariables("ASPNETCORE");
                })
                .UseEnvironment("Testing");
        }

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