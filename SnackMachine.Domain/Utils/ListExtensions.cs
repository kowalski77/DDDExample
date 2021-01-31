using System.Collections.Generic;
using System.Linq;
using SnackMachine.Domain.MachineAggregate;

namespace SnackMachine.Domain.Utils
{
    public static class ListExtensions
    {
        public static List<Coin> GetClone(this IEnumerable<Coin> source)
        {
            return source.Select(item => (Coin)item.Clone()).ToList();
        }
    }
}