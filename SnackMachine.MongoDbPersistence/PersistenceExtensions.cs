using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SnackMachine.Domain.AccountAggregate;
using SnackMachine.Domain.SnackAggregate;

namespace SnackMachine.MongoDbPersistence
{
    public static class PersistenceExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbConfiguration>(configuration.GetSection(nameof(MongoDbConfiguration)));
            services.AddSingleton<SnackMachineContext>();
            services.AddSingleton<IAccountRepository, AccountRepository>();
            services.AddSingleton<ISnackRepository, SnackRepository>();

            return services;
        }
    }
}
