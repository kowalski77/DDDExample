using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using SnackMachine.Domain.AccountAggregate;
using SnackMachine.Domain.Utils;

namespace SnackMachine.MongoDbPersistence
{
    public class AccountRepository : IAccountRepository
    {
        private readonly SnackMachineContext context;

        public AccountRepository(SnackMachineContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Maybe<Account>> GetAccountAsync()
        {
            return (await this.context.AccountsCollection.FindAsync(Builders<Account>.Filter.Empty)).FirstOrDefault();
        }

        public async Task UpdateAccountAsync(Account account)
        {
            await this.context.AccountsCollection.ReplaceOneAsync(Builders<Account>.Filter.Empty, account);
        }
    }
}