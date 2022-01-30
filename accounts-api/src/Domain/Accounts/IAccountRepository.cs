using Domain.Credits;
using Domain.Debits;
using Domain.ValueObjects;

namespace Domain.Accounts;

public interface IAccountRepository
{
    Task<IAccount> GetAccount(AccountId accountId);

    Task<IList<Account>> GetAccounts(string externalUserId);

    Task Add(Account account, Credit credit);

    Task Update(Account account, Credit credit);

    Task Update(Account account, Debit debit);

    Task Delete(AccountId accountId);

    Task<IAccount> Find(AccountId accountId, string externalUserId);
}
