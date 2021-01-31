using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using SnackMachine.Domain.SnackAggregate;
using SnackMachine.Domain.Utils;
using SnackMachine.Domain.ValueObjects;

namespace SnackMachine.Domain.MachineAggregate
{
    public class Pile : Entity
    {
        private readonly List<long> piledSnacks = new();

        public Pile(Capacity capacity)
        {
            this.Capacity = capacity ?? throw new ArgumentNullException(nameof(capacity));
            this.SnacksCount = 0;
        }

        public int SnacksCount { get; private set; }

        public Capacity Capacity { get; }

        public IReadOnlyList<long> PiledSnacks => this.piledSnacks.ToImmutableList();

        public bool CanAddSnack()
        {
            return this.SnacksCount != this.Capacity.Value;
        }

        public void AddSnack(Snack snack)
        {
            if (snack == null) throw new ArgumentNullException(nameof(snack));

            if (!this.CanAddSnack())
            {
                throw new InvalidOperationException("Not enough capacity in this pile");
            }

            this.piledSnacks.Add(snack.Id);
            this.SnacksCount++;
        }
    }
}