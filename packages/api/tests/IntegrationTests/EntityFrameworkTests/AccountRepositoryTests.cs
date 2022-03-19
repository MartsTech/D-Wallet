namespace IntegrationTests.EntityFrameworkTests;

using Domain.Accounts;
using Domain.Credits;
using Domain.Debits;
using Domain.ValueObjects;
using Persistence;
using Persistence.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public sealed class AccountRepositoryTests : IClassFixture<StandardFixture>
{
    private readonly StandardFixture _fixture;

    public AccountRepositoryTests(StandardFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Add()
    {
        AccountRepository accountRepository = new(_fixture.Context);

        Account account = new(
            new AccountId(Guid.NewGuid()),
            SeedData.DefaultUserId,
            Currency.Dollar
        );

        Credit credit = new(
            new CreditId(Guid.NewGuid()),
            account.AccountId,
            DateTime.Now,
            400,
            "USD"
        );

        await accountRepository
            .Add(account, credit)
            .ConfigureAwait(false);

        await _fixture
            .Context
            .SaveChangesAsync()
            .ConfigureAwait(false);

        bool hasAnyAccount = _fixture
            .Context
            .Accounts
            .Any(e => e.AccountId == account.AccountId);

        bool hasAnyCredit = _fixture
            .Context
            .Credits
            .Any(e => e.CreditId == credit.CreditId);

        Assert.True(hasAnyAccount && hasAnyCredit);
    }

    [Fact]
    public async Task Delete()
    {
        AccountRepository accountRepository = new(_fixture.Context);

        Account account = new(
            new AccountId(Guid.NewGuid()),
            SeedData.DefaultUserId,
            Currency.Dollar
        );

        Credit credit = new(
            new CreditId(Guid.NewGuid()),
            account.AccountId,
            DateTime.Now,
            400,
            "USD"
        );

        await accountRepository
            .Add(account, credit)
            .ConfigureAwait(false);

        await _fixture
            .Context
            .SaveChangesAsync()
            .ConfigureAwait(false);

        await accountRepository
            .Delete(account.AccountId)
            .ConfigureAwait(false);

        await _fixture
            .Context
            .SaveChangesAsync()
            .ConfigureAwait(false);

        bool hasAnyAccount = _fixture
            .Context
            .Accounts
            .Any(e => e.AccountId == account.AccountId);

        bool hasAnyCredit = _fixture
            .Context
            .Credits
            .Any(e => e.CreditId == credit.CreditId);

        Assert.False(hasAnyAccount && hasAnyCredit);
    }

    [Fact]
    public async Task Update()
    {
        AccountRepository accountRepository = new(_fixture.Context);

        Account account = new(
            new AccountId(Guid.NewGuid()),
            SeedData.DefaultUserId,
            Currency.Dollar
        );

        Credit credit = new(
            new CreditId(Guid.NewGuid()),
            account.AccountId,
            DateTime.Now,
            500,
            "USD"
        );

        await accountRepository
            .Add(account, credit)
            .ConfigureAwait(false);

        await _fixture
            .Context
            .SaveChangesAsync()
            .ConfigureAwait(false);
        
        Debit debit = new(
           new DebitId(Guid.NewGuid()),
           account.AccountId,
           DateTime.Now,
           400,
           "USD"
       );

        await accountRepository
            .Update(account, debit)
            .ConfigureAwait(false);

        await _fixture
            .Context
            .SaveChangesAsync()
            .ConfigureAwait(false);

        var findAccount = await _fixture
            .Context
            .Accounts
            .FindAsync(account.AccountId);

        Money balance = findAccount.GetCurrentBalance();

        Assert.Equal(balance.Amount,100) ;
    }

}