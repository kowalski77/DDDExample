using System;
using SnackMachine.Domain.Utils;
using SnackMachine.Domain.ValueObjects;

namespace SnackMachine.Domain.SnackAggregate
{
    public class Snack : Entity
    {
        public Snack(Name name, Money price)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Price = price ?? throw new ArgumentNullException(nameof(price));
        }

        public Name Name { get; }

        public Money Price { get; }
    }
}