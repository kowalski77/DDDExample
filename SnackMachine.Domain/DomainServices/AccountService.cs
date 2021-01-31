using System;
using SnackMachine.Domain.MachineAggregate;
using SnackMachine.Domain.SnackAggregate;
using SnackMachine.Domain.Utils;

namespace SnackMachine.Domain.DomainServices
{
    public sealed class AccountService
    {
        private readonly IExchangeBox exchangeBox;

        public AccountService(IExchangeBox exchangeBox)
        {
            this.exchangeBox = exchangeBox ?? throw new ArgumentNullException(nameof(exchangeBox));
        }

        public Result<AccountCode> BuyWithExchange(Account account, Snack snack)
        {
            this.exchangeBox.LoadMoney(account.Coins);

            var canBuyResult = account.CanBuy(snack);
            if (!canBuyResult.Success)
            {
                return canBuyResult;
            }

            var change = account.MoneyInTransaction - snack.Price;
            if (!this.exchangeBox.TryGetChange(change, out var changeDictionary))
            {
                account.CancelTransaction();
                return Result<AccountCode>.Fail(AccountCode.NotEnoughChange);
            }

            account.Buy(snack);

            foreach (var (money, quantity) in changeDictionary)
            {
                account.UpdateMoney(money, quantity);
            }

            return Result<AccountCode>.Ok(AccountCode.Ok);
        }
    }
}