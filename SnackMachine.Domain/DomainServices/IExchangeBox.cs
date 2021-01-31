using System.Collections.Generic;
using SnackMachine.Domain.MachineAggregate;
using SnackMachine.Domain.ValueObjects;

namespace SnackMachine.Domain.DomainServices
{
    public interface IExchangeBox
    {
        void LoadMoney(IEnumerable<Coin> coins);

        bool TryGetChange(Money amount, out SortedList<Money, int> changeDictionary);
    }
}