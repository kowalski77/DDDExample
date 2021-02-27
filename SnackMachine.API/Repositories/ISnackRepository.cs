using System.Threading.Tasks;
using SnackMachine.Domain.SnackAggregate;
using SnackMachine.Domain.Utils;

namespace SnackMachine.API.Repositories
{
    public interface ISnackRepository
    {
        public Task<Maybe<Snack>> GetSnackAsync(long id);
    }
}