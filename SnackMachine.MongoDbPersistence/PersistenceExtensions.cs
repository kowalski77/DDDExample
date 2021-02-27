using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SnackMachine.Domain.MachineAggregate;
using SnackMachine.Domain.SnackAggregate;

namespace SnackMachine.MongoDbPersistence
{
    public static class PersistenceExtensions
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbConfiguration>(configuration.GetSection(nameof(MongoDbConfiguration)));
            services.AddSingleton<SnackMachineContext>();
            services.AddSingleton<IMachineRepository, MachineRepository>();
            services.AddSingleton<ISnackRepository, SnackRepository>();

            return services;
        }
    }
}
