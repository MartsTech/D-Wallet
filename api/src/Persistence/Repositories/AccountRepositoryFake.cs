using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    using Domain.Accounts;
    using Domain.Credits;
    using Domain.Debits;
    using Domain.ValueObjects;

    public sealed class AccountRepositoryFake : IAccountRepository
    {
        private readonly DataContextFake _context;

        public AccountRepositoryFake(DataContextFake context)
        {
            _context = context;
        }

        public async Task Add(Account account, Credit credit)
        {
            _context.Accounts.Add(account);

            _context.Credits.Add(credit);

            await Task.CompletedTask
                .ConfigureAwait(false);
        }

        public async Task Delete(AccountId accountId)
        {
            Account accountOld = _context
               .Accounts
               .SingleOrDefault(e => e.AccountId.Equals(accountId));

            if (accountOld == null)
            {
                return;
            }

            _context
                .Accounts
                .Remove(accountOld);

            await Task.CompletedTask
                .ConfigureAwait(false);
        }

        public async Task<IAccount> Find(AccountId accountId, string externalUserId)
        {
            Account account = _context
               .Accounts
               .Where(e => e.ExternalUserId == externalUserId && e.AccountId.Equals(accountId))
               .Select(e => e)
               .SingleOrDefault();

            if (account == null)
            {
                return AccountNull.Instance;
            }

            return await Task.FromResult(account)
                .ConfigureAwait(false);
        }

        public async Task<IAccount> GetAccount(AccountId accountId)
        {
            Account account = _context
                .Accounts
                .SingleOrDefault(e => e.AccountId.Equals(accountId));

            if (account == null)
            {
                return AccountNull.Instance;
            }

            return await Task.FromResult(account)
                .ConfigureAwait(false);
        }

        public async Task<IList<Account>> GetAccounts(string externalUserId)
        {
            List<Account> accounts = _context
              .Accounts
              .Where(e => e.ExternalUserId == externalUserId)
              .ToList();

            return await Task.FromResult(accounts)
                .ConfigureAwait(false);
        }

        public async Task Update(Account account, Credit credit)
        {
            Account accountOld = _context
               .Accounts
               .SingleOrDefault(e => e.AccountId.Equals(account.AccountId));

            if (accountOld != null)
            {
                _context.Accounts.Remove(accountOld);
            }

            _context.Accounts.Add(account);
            _context.Credits.Add(credit);

            await Task.CompletedTask
               .ConfigureAwait(false);
        }

        public async Task Update(Account account, Debit debit)
        {
            Account accountOld = this._context
                .Accounts
                .SingleOrDefault(e => e.AccountId.Equals(account.AccountId));

            if (accountOld != null)
            {
                _context.Accounts.Remove(accountOld);
                _context.Accounts.Add(account);
            }

            _context.Debits.Add(debit);

            await Task.CompletedTask
                .ConfigureAwait(false);
        }
    }
}
