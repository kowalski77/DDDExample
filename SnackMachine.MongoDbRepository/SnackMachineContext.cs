using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SnackMachine.Domain.MachineAggregate;
using SnackMachine.Domain.SnackAggregate;

namespace SnackMachine.MongoDbRepository
{
    public class SnackMachineContext
    {
        private readonly IMongoDatabase database;
        private readonly MongoDbConfiguration configuration;

        public SnackMachineContext(IOptions<MongoDbConfiguration> options)
        {
            this.configuration = options.Value;
            var client = new MongoClient(this.configuration.ConnectionString);

            this.database = client.GetDatabase(this.configuration.DatabaseName);
        }

        public IMongoCollection<Snack> SnacksCollection =>
            this.database.GetCollection<Snack>(this.configuration.SnacksCollectionName);

        public IMongoCollection<Machine> MachinesCollection =>
            this.database.GetCollection<Machine>(this.configuration.MachineCollectionName);
    }
}