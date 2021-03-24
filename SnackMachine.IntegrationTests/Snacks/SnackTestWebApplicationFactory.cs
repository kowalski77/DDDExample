using AutoFixture;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SnackMachine.Domain.SnackAggregate;

namespace SnackMachine.IntegrationTests.Snacks
{
    public class SnackTestWebApplicationFactory<TStartup> : BaseTestWebApplicationFactory<TStartup>
        where TStartup : class
    {
        public SnackTestWebApplicationFactory()
        {
            this.Snack = this.Fixture.Create<Snack>();
        }

        public Snack Snack { get; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureServices((_, services) =>
            {
                var serviceProvider = services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();

                var snackRepository = scope.ServiceProvider.GetRequiredService<ISnackRepository>();
                snackRepository.AddSnack(this.Snack).Wait();
            });
        }
    }
}