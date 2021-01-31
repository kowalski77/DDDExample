using System;
using SnackMachine.Domain.Utils;
using SnackMachine.Domain.ValueObjects;

namespace SnackMachine.Domain.MachineAggregate
{
    public class Coin : Entity, ICloneable
    {
        public Coin(int quantity, Money money)
        {
            if (quantity < 0) throw new ArgumentOutOfRangeException(nameof(quantity));
            this.Quantity = quantity;
            this.Money = money ?? throw new ArgumentNullException(nameof(money));
        }

        public int Quantity { get; private set; }

        public Money Money { get; }

        public void IncreaseQuantity(int count)
        {
            this.Quantity += count;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}