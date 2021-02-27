using System.Threading.Tasks;
using SnackMachine.Domain.Utils;

namespace SnackMachine.Domain.MachineAggregate
{
    public interface IMachineRepository
    {
        Task CreateAsync(Machine machine);

        Task<Maybe<Machine>> GetMainMachineAsync();

        Task SaveAsync(Machine machine);
    }
}