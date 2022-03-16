﻿namespace UnitTests.Accounts.OpenAccount;

using Application.UseCases.Accounts.OpenAccount;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public sealed class OpenAccountTests : IClassFixture<StandardFixture>
{
    private readonly StandardFixture _fixture;

    public OpenAccountTests(StandardFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [ClassData(typeof(ValidDataSetup))]
    public async Task OpenAccount_Returns_Ok(decimal amount, string currency)
    {
        OpenAccountInput input = new()
        {
            Amount = amount,
            Currency = currency
        };

        OpenAccountUseCase.Command command = new(input);

        OpenAccountUseCase.Handler handler = new(
            _fixture.EntityFactory,
            _fixture.AccountRepository,
            _fixture.UnitOfWork,
            _fixture.UserService);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }
}