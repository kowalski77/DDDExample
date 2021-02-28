using System.Threading.Tasks;
using SnackMachine.Domain.Utils;

namespace SnackMachine.Domain.AccountAggregate
{
    public interface IAccountRepository
    {
        public Task<Maybe<Account>> GetAccountAsync();

        public Task UpdateAccountAsync(Account account);
    }
}