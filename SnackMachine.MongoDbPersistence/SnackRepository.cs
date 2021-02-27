using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using SnackMachine.Domain.SnackAggregate;
using SnackMachine.Domain.Utils;

namespace SnackMachine.MongoDbPersistence
{
    public class SnackRepository : ISnackRepository
    {
        private readonly SnackMachineContext context;

        public SnackRepository(SnackMachineContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Maybe<Snack>> GetSnackAsync(long id)
        {
            return (await this.context.SnacksCollection.FindAsync(Builders<Snack>.Filter.Eq(x => x.Id, id))).FirstOrDefault();
        }
    }
}