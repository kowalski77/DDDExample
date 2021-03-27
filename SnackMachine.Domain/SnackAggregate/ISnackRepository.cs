using System;
using System.Threading.Tasks;
using SnackMachine.Domain.Utils;

namespace SnackMachine.Domain.SnackAggregate
{
    public interface ISnackRepository
    {
        public Task<Snack> AddSnack(Snack snack);

        public Task<Maybe<Snack>> GetSnackAsync(Guid id);
    }
}