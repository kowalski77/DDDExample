using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SnackMachine.Domain.SnackAggregate;
using SnackMachine.Domain.Utils;
using SnackMachine.Domain.ValueObjects;

namespace SnackMachine.Domain.MachineAggregate
{
    public sealed class Account : Entity
    {
        private readonly List<Coin> originalCoins;
        private List<Coin> coins;

        public Account(IReadOnlyList<Coin> coinCollection)
        {
            if (coinCollection == null) throw new ArgumentNullException(nameof(coinCollection));

            this.coins = coinCollection.ToList();
            this.originalCoins = this.coins.GetClone();

            this.MoneyInTransaction = Money.Zero;
        }

        public Money MoneyInTransaction { get; private set; }

        public IReadOnlyList<Coin> Coins => this.coins.ToImmutableList();

        public Result<AccountCode> CanInsertMoney(Money money)
        {
            return this.coins.Any(x=>x.Money == money) ? 
                Result<AccountCode>.Ok() : 
                Result<AccountCode>.Fail(AccountCode.CoinNotRegistered);
        }

        public void InsertMoney(Money money)
        {
            if (!this.CanInsertMoney(money).Success)
            {
                throw new InvalidOperationException($"Coin/Note with value: {money.Value} not allowed");
            }

            var coin = this.coins.First(x => x.Money == money);
            coin.IncreaseQuantity(1);

            this.MoneyInTransaction += money;
        }

        public Result<AccountCode> CanBuy(Snack snack)
        {
            return this.MoneyInTransaction >= snack.Price ? 
                Result<AccountCode>.Ok() : 
                Result<AccountCode>.Fail(AccountCode.NotEnoughMoney);
        }

        public void Buy(Snack snack)
        {
            if (!this.CanBuy(snack).Success)
            {
                throw new InvalidOperationException($"Cannot buy snack: {snack.Name}");
            }

            this.MoneyInTransaction = Money.Zero;
        }

        public void UpdateMoney(Money money, int quantity)
        {
            var coin = this.coins.First(x => x.Money == money);
            coin.IncreaseQuantity(quantity);
        }

        public void CancelTransaction()
        {
            this.MoneyInTransaction = Money.Zero;

            this.coins = this.originalCoins;
        }
    }
}