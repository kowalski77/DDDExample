using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SnackMachine.MongoDbRepository
{
    public static class MongoDbExtensions
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbConfiguration>(configuration.GetSection(nameof(MongoDbConfiguration)));

            return services;
        }
    }
}
