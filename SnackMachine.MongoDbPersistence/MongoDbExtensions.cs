using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SnackMachine.Domain.MachineAggregate;

namespace SnackMachine.MongoDbPersistence
{
    public static class MongoDbExtensions
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbConfiguration>(configuration.GetSection(nameof(MongoDbConfiguration)));
            services.AddSingleton<SnackMachineContext>();
            services.AddSingleton<IMachineRepository, MachineRepository>();

            return services;
        }
    }
}
