using System;
using System.Collections.Generic;
using SnackMachine.Domain.Utils;

namespace SnackMachine.Domain.ValueObjects
{
    public class Cash : ValueObject
    {
        private readonly Money money;

        protected Cash(int quantity, Money money)
        {
            if (quantity < 0) throw new ArgumentOutOfRangeException(nameof(quantity));
            this.Quantity = quantity;
            this.money = money ?? throw new ArgumentNullException(nameof(money));
        }

        public Money Total => this.money * this.Quantity;

        public int Quantity { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Total.Value;
            yield return this.Quantity;
        }
    }
}