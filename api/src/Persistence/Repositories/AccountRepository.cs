namespace Persistence.Repositories;

using Domain.Accounts;
using Domain.Credits;
using Domain.Debits;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

public sealed class AccountRepository : IAccountRepository
{
    private readonly DataContext _context;

    public AccountRepository(DataContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task Add(Account account, Credit credit)
    {
        await _context
            .Accounts
            .AddAsync(account)
            .ConfigureAwait(false);

        await _context
            .Credits
            .AddAsync(credit)
            .ConfigureAwait(false);
    }

    public async Task Delete(AccountId accountId)
    {
        await _context
            .Database
            .ExecuteSqlRawAsync("DELETE FROM Debit WHERE AccountId=@p0", accountId.Id)
            .ConfigureAwait(false);

        await _context
          .Database
          .ExecuteSqlRawAsync("DELETE FROM Credit WHERE AccountId=@p0", accountId.Id)
          .ConfigureAwait(false);

        await _context
            .Database
            .ExecuteSqlRawAsync("DELETE FROM Account WHERE AccountId=@p0", accountId.Id)
            .ConfigureAwait(false);
    }

    public async Task Update(Account account, Credit credit)
    {
        await _context
            .Credits
            .AddAsync(credit)
            .ConfigureAwait(false);
    }

    public async Task Update(Account account, Debit debit)
    {
        await _context
            .Debits
            .AddAsync(debit)
            .ConfigureAwait(false);
    }

    public async Task<IAccount> Find(AccountId accountId, string externalUserId)
    {
        Account? account = await _context
           .Accounts
           .Where(e => e.ExternalUserId == externalUserId && e.AccountId == accountId)
           .Select(e => e)
           .SingleOrDefaultAsync()
           .ConfigureAwait(false);

        if (account is Account findAccount)
        {
            await LoadTransactions(findAccount)
                .ConfigureAwait(false);

            return account;
        }

        return AccountNull.Instance;
    }

    public async Task<IAccount> GetAccount(AccountId accountId)
    {
        Account? account = await _context
            .Accounts
            .Where(e => e.AccountId == accountId)
            .Select(e => e)
            .SingleOrDefaultAsync()
            .ConfigureAwait(false);

        if (account is Account findAccount)
        {
            await LoadTransactions(findAccount)
                .ConfigureAwait(false);

            return account;
        }

        return AccountNull.Instance;
    }

    public async Task<IList<Account>> GetAccounts(string externalUserId)
    {
        List<Account> accounts = await _context
            .Accounts
            .Where(e => e.ExternalUserId == externalUserId)
            .ToListAsync()
            .ConfigureAwait(false);

        foreach (Account findAccount in accounts)
        {
            await LoadTransactions(findAccount)
                .ConfigureAwait(false);
        }

        return accounts;
    }

    private async Task LoadTransactions(Account account)
    {
        await _context
           .Credits
           .Where(e => e.AccountId.Equals(account.AccountId))
           .ToListAsync()
           .ConfigureAwait(false);


        await _context
            .Debits
            .Where(e => e.AccountId.Equals(account.AccountId))
            .ToListAsync()
            .ConfigureAwait(false);
    }
}