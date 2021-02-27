using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using SnackMachine.Domain.Utils;
using SnackMachine.Domain.ValueObjects;

namespace SnackMachine.Domain.MachineAggregate
{
    public class Machine : Entity
    {
        private const int MaxPilesAllowed = 5;
        private readonly List<Pile> piles = new(MaxPilesAllowed);

        public Machine(Name name, Account account)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Account = account ?? throw new ArgumentNullException(nameof(account));
        }

        public Name Name { get; }

        public IReadOnlyList<Pile> Piles => this.piles.ToImmutableList();

        public Account Account { get; }

        public bool CanAddPile()
        {
            return this.piles.Count < MaxPilesAllowed;
        }

        public void AddPile(Pile pile)
        {
            if (!this.CanAddPile())
            {
                throw new InvalidOperationException($"The maximum number of piles is {MaxPilesAllowed}");
            }

            this.piles.Add(pile);
        }
    }
}