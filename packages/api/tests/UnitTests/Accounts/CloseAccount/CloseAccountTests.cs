namespace UnitTests.Accounts.CloseAccount;

using Application.UseCases.Accounts.CloseAccount;
using Domain.Accounts;
using Domain.Credits;
using Domain.ValueObjects;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

public sealed class CloseAccountTests : IClassFixture<StandardFixture>
{
    private readonly StandardFixture _fixture;

    public CloseAccountTests(StandardFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [ClassData(typeof(ValidDataSetup))]
    public void IsClosingAllowed_Returns_False_When_Account_Has_Funds(decimal amount)
    {
        Account account = _fixture
            .EntityFactory
            .NewAccount(Guid.NewGuid().ToString(), Currency.Dollar);

        Credit credit = _fixture
            .EntityFactory
            .NewCredit(account, new Money(amount, Currency.Dollar), DateTime.Now);

        account.Deposit(credit);

        bool actual = account.IsClosingAllowed();

        Assert.False(actual);
    }

    [Fact]
    public async Task CloseAccountUseCase_Returns_Exception_When_Account_Has_Balance()
    {
        CloseAccountInput input = new()
        {
            AccountId = SeedData.DefaultAccountId.Id
        };

        CloseAccountUseCase.Command command = new(input);

        CloseAccountUseCase.Handler handler = new(
            _fixture.AccountRepository,
            _fixture.UnitOfWork,
            _fixture.UserService);

        var result = await handler.Handle(command, CancellationToken.None);

        Console.WriteLine(result.Error);
        Assert.False(result.IsSuccess);
    }
}
