using System.Threading.Tasks;
using SnackMachine.Domain.MachineAggregate;

namespace SnackMachine.MongoDbRepository
{
    public class MachineRepository : IMachineRepository
    {
        public async Task CreateAsync(Machine machine)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Machine> GetMainMachineAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task SaveAsync(Machine machine)
        {
            throw new System.NotImplementedException();
        }
    }
}