using SnackMachine.Domain.AccountAggregate;
using SnackMachine.Domain.SnackAggregate;
using SnackMachine.Domain.Utils;

namespace SnackMachine.Domain.DomainServices
{
    public interface IAccountService
    {
        Result<AccountCode> BuyWithExchange(Account account, Snack snack);
    }
}