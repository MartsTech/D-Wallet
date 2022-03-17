namespace UnitTests.Accounts.CloseAccount;

using Application.UseCases.Accounts.CloseAccount;
using Application.UseCases.Transactions.Withdraw;
using Domain.Accounts;
using Domain.Credits;
using Domain.ValueObjects;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

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
    public async Task CloseAccountUseCase_Returns_Closed_Account_Id_When_Account_Has_Zero_Balance()
    {
        WithdrawInput inputWithdraw = new()
        {
            AccountId = SeedData.DefaultAccountId.Id,
            Amount = 500m,
            Currency = "USD"
        };

        WithdrawUseCase.Command commandWithdraw = new(inputWithdraw);

        WithdrawUseCase.Handler handlerWithdraw = new(
            _fixture.EntityFactory,
            _fixture.AccountRepository,
            _fixture.CurrencyExchange,
            _fixture.UnitOfWork,
            _fixture.UserService);

        await handlerWithdraw.Handle(commandWithdraw, CancellationToken.None);

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

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    //[Theory]
    //[ClassData(typeof(InvalidDataSetup))]
    //public async Task CloseAccountUseCase_Returns_Error_When_Given_Id_Which_Does_Not_Exist(Guid id)
    //{
        

      //  CloseAccountInput input = new()
       // {
            //AccountId = id
       // };

       // CloseAccountUseCase.Command command = new(input);

        //CloseAccountUseCase.Handler handler = new(
            //_fixture.AccountRepository,
            //_fixture.UnitOfWork,
           // _fixture.UserService);

        //var result = await handler.Handle(command, CancellationToken.None);

       // Assert.False(result.IsSuccess);
       // Assert.NotNull(result.Error);
    //}
}
