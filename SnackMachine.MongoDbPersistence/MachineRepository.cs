using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using SnackMachine.Domain.MachineAggregate;
using SnackMachine.Domain.Utils;

namespace SnackMachine.MongoDbPersistence
{
    public class MachineRepository : IMachineRepository
    {
        private readonly SnackMachineContext context;

        public MachineRepository(SnackMachineContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateAsync(Machine machine)
        {
            await this.context.MachinesCollection.InsertOneAsync(machine);
        }

        public async Task<Maybe<Machine>> GetMainMachineAsync()
        {
            return (await this.context.MachinesCollection.FindAsync(Builders<Machine>.Filter.Empty)).FirstOrDefault();
        }

        public async Task SaveAsync(Machine machine)
        {
            await this.context.MachinesCollection.ReplaceOneAsync(Builders<Machine>.Filter.Eq(x => x.Id, machine.Id), machine);
        }
    }
}