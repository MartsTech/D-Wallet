namespace Infrastructure.DataAccess;

using Domain.Accounts;
using Domain.Credits;
using Domain.Debits;
using Domain.ValueObjects;
using System.Collections.ObjectModel;

public sealed class DataContextFake
{
    public DataContextFake()
    {
        Credit credit = new(
           new CreditId(Guid.NewGuid()),
           SeedData.DefaultAccountId,
           DateTime.Now,
           800,
           Currency.Dollar.Code);

        Debit debit = new(
             new DebitId(Guid.NewGuid()),
             SeedData.DefaultAccountId,
             DateTime.Now,
             300,
             Currency.Dollar.Code);

        Account account = new(
            SeedData.DefaultAccountId,
            SeedData.DefaultExternalUserId,
            Currency.Dollar);

        account.CreditsCollection.Add(credit);
        account.DebitsCollection.Add(debit);

        Accounts.Add(account);
        Credits.Add(credit);
        Debits.Add(debit);

        Account account2 = new(
            SeedData.SecondAccountId,
            SeedData.SecondExternalUserId,
            Currency.Dollar);

        Accounts.Add(account2);
    }

    public Collection<Account> Accounts { get; } = new Collection<Account>();

    public Collection<Credit> Credits { get; } = new Collection<Credit>();

    public Collection<Debit> Debits { get; } = new Collection<Debit>();
}

