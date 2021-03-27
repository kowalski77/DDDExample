using System;
using System.Linq;
using System.Net.Http;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SnackMachine.Domain.SnackAggregate;
using SnackMachine.MongoDbPersistence;

namespace SnackMachine.IntegrationTests
{
    public class TestWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        private Lazy<IMongoClient>? mongoClientLazy;
        private IServiceProvider? serviceProvider;

        public TestWebApplicationFactory()
        {
            this.HttpClient = this.CreateClient();
        }

        public HttpClient HttpClient { get; }

        public IFixture Fixture { get; } = new Fixture().Customize(new AutoMoqCustomization());

        public IMongoCollection<Snack> SnacksCollection { get; private set; } = null!;

        private IMongoClient MongoClient => this.mongoClientLazy?.Value ?? 
                                            throw new InvalidOperationException("MongoClient has not been initialized");

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
                if (mongoConfigurationDescriptor != null)
                {
                    services.Remove(mongoConfigurationDescriptor);
                }

                services.Configure<MongoDbConfiguration>(context.Configuration.GetSection(nameof(MongoDbConfiguration)));
                this.serviceProvider = services.BuildServiceProvider();

                this.CreateMongoClient();
                this.InitializeCollections();
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.DropMongoDbTestDatabase();

                switch (this.serviceProvider)
                {
                    case null:
                        return;
                    case IDisposable disposable:
                        disposable.Dispose();
                        break;
                }
            }

            base.Dispose(disposing);
        }

        private void CreateMongoClient()
        {
            var options = (this.serviceProvider ?? 
                           throw new InvalidOperationException($"{nameof(IServiceProvider)} is null")).GetRequiredService<IOptions<MongoDbConfiguration>>();

            this.mongoClientLazy = new Lazy<IMongoClient>(() => new MongoClient(options.Value.ConnectionString));
        }

        private void InitializeCollections()
        {
            var options = (this.serviceProvider ?? 
                           throw new InvalidOperationException($"{nameof(IServiceProvider)} is null")).GetRequiredService<IOptions<MongoDbConfiguration>>();

            this.SnacksCollection = this.MongoClient.GetDatabase(options.Value.DatabaseName).GetCollection<Snack>(options.Value.SnacksCollectionName);
        }

        private void DropMongoDbTestDatabase()
        {
            var options = this.serviceProvider?.GetRequiredService<IOptions<MongoDbConfiguration>>() ?? 
                          throw new ArgumentNullException($"Could not resolve {nameof(IOptions<MongoDbConfiguration>)}");

            this.MongoClient.DropDatabase(options.Value.DatabaseName);
        }
    }
}