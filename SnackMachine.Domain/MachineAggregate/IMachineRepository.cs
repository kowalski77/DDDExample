using System.Threading.Tasks;

namespace SnackMachine.Domain.MachineAggregate
{
    public interface IMachineRepository
    {
        Task CreateAsync(Machine machine);

        Task<Machine> GetMainMachineAsync();

        Task SaveAsync(Machine machine);
    }
}