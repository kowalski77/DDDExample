using System.Threading.Tasks;
using SnackMachine.Domain;
using SnackMachine.Domain.SnackAggregate;

namespace SnackMachine.API.Repositories
{
    public interface ISnackRepository
    {
        public Task<Snack> GetSnackAsync(long id);
    }
}