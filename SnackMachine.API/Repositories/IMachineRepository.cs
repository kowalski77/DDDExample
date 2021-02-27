using System.Threading.Tasks;
using SnackMachine.Domain.MachineAggregate;

namespace SnackMachine.API.Repositories
{
    public interface IMachineRepository
    {
        Task CreateAsync(Machine machine);

        Task<Machine> GetMainMachineAsync();

        Task<int> SaveAsync();
    }
}