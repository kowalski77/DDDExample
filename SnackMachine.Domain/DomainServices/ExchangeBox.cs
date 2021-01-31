using System;
using System.Collections.Generic;
using System.Linq;
using SnackMachine.Domain.MachineAggregate;
using SnackMachine.Domain.Utils;
using SnackMachine.Domain.ValueObjects;

namespace SnackMachine.Domain.DomainServices
{
    public sealed class ExchangeBox : IExchangeBox
    {
        private readonly SortedList<Money, int> availableMoney =
            new(new ReverseComparer<Money>(Comparer<Money>.Default));

        public void LoadMoney(IEnumerable<Coin> coins)
        {
            foreach (var coin in coins)
            {
                this.availableMoney.Add(coin.Money, 0);
                this.availableMoney[coin.Money] += coin.Quantity;
            }
        }

        public bool TryGetChange(Money amount, out SortedList<Money, int> changeDictionary)
        {
            changeDictionary = new SortedList<Money, int>(new ReverseComparer<Money>(Comparer<Money>.Default));

            if (amount == Money.Zero)
            {
                return true;
            }

            var availableCoins = new SortedList<Money, int>(this.availableMoney,
                new ReverseComparer<Money>(Comparer<Money>.Default));

            var index = 0;
            this.CalculateChange(amount.Value, availableCoins, changeDictionary, ref index);

            if (index == int.MaxValue)
            {
                return true;
            }

            changeDictionary.Clear();

            return false;
        }

        private void CalculateChange(
            decimal amount,
            IDictionary<Money, int> availableCoins,
            IDictionary<Money, int> changeDictionary, ref int index)
        {
            while (index < availableCoins.Count)
            {
                var denomination = availableCoins.ElementAt(index++).Key;

                if (amount < denomination.Value)
                {
                    continue;
                }

                var count = (int)Math.Min(amount / denomination.Value, availableCoins[denomination]);

                changeDictionary.Add(denomination, count);
                availableCoins[denomination] -= count;

                var remainder = amount - count * denomination.Value;

                if (remainder == 0)
                {
                    index = int.MaxValue;
                    return;
                }

                this.CalculateChange(remainder, availableCoins, changeDictionary, ref index);
            }
        }
    }
}