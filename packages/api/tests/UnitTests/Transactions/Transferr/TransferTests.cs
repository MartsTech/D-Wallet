﻿namespace UnitTests.Transactions.Transferr;

using Application.UseCases.Transactions.Transfer;
using FluentValidation.Results;
using Persistence;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public sealed class TransferrTests : IClassFixture<StandardFixture>
{
    private readonly StandardFixture _fixture;

    public TransferrTests(StandardFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [ClassData(typeof(ValidDataSetup))]
    public async Task Transfer_Is_Successful(decimal amount)
    {
        TransferInput input = new()
        {
            Amount = amount,
            OriginAccountId = SeedData.DefaultAccountId.Id,
            DestinationAccountId = SeedData.SecondAccountId.Id,
            Currency = "USD"
        };

        TransferUseCase.Command command = new(input);

        TransferUseCase.Handler handler = new(
            _fixture.EntityFactory,
            _fixture.AccountRepository,
            _fixture.CurrencyExchange,
            _fixture.UnitOfWork);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        
    }

    [Theory]
    [ClassData(typeof(InvalidDataSetup))]
    public async Task Transfer_Is_Not_Successful_When_Transferring_Negative_Amount(decimal amount)
    {
        TransferInput input = new()
        {
            Amount = amount,
            OriginAccountId = SeedData.DefaultAccountId.Id,
            DestinationAccountId = SeedData.SecondAccountId.Id,
            Currency = "USD"
        };

        TransferUseCase.Command command = new(input);

        TransferUseCase.CommandValidator validator = new();

        ValidationResult validationResult = validator.Validate(command);

        Assert.False(validationResult.IsValid);
    }
}