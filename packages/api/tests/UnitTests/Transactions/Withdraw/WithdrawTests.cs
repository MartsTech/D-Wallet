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
using UnitTests.Transactions.Withdraw;
using Xunit;
using Xunit.Abstractions;

public sealed class WithdrawTests : IClassFixture<StandardFixture>
{
    private readonly StandardFixture _fixture;

    public WithdrawTests(StandardFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [ClassData(typeof(ValidDataSetup))]
    public async Task Withdraw_Is_Successful(decimal amount)
    {
        WithdrawInput input = new()
        {
            AccountId = SeedData.DefaultAccountId.Id,
            Amount = amount,
            Currency = "USD"
        };

        WithdrawUseCase.Command command = new(input);

        WithdrawUseCase.Handler handler = new(
            _fixture.EntityFactory,
            _fixture.AccountRepository,
            _fixture.CurrencyExchange,
            _fixture.UnitOfWork,
            _fixture.UserService);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }
    [Theory]
    [ClassData(typeof(InvalidDataSetup))]
    public async Task Error_When_Withdrawing_More_Money_Than_Money_In_Balance(decimal amount)
    {
        WithdrawInput input = new()
        {
            AccountId = SeedData.DefaultAccountId.Id,
            Amount = amount,
            Currency = "USD",
           

        };

        WithdrawUseCase.Command command = new(input);

        WithdrawUseCase.Handler handler = new(
            _fixture.EntityFactory,
            _fixture.AccountRepository,
            _fixture.CurrencyExchange,
            _fixture.UnitOfWork,
            _fixture.UserService);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
    }
}


    
