﻿namespace UnitTests.Accounts.GetAccount;

using Application.UseCases.Accounts.GetAccount;
using Persistence;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public sealed class GetAccountsTests : IClassFixture<StandardFixture>
{
    private readonly StandardFixture _fixture;

    public GetAccountsTests(StandardFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetAccount_Function_Is_Successful()
    {
        GetAccountInput inputGetAccount = new()
        {
            AccountId = SeedData.DefaultAccountId.Id,
        };

        GetAccountUseCase.Query command = new(inputGetAccount);

        GetAccountUseCase.Handler handler = new(
            _fixture.AccountRepository);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }
}