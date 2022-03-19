namespace UnitTests.Transactions.Deposit;

using Application.UseCases.Transactions.Deposit;
using FluentValidation.Results;
using Persistence;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public sealed class DepositTests : IClassFixture<StandardFixture>
{
    private readonly StandardFixture _fixture;

    public DepositTests(StandardFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [ClassData(typeof(ValidDataSetup))]
    public async Task Deposit_Is_Successful(decimal amount)
    {
        DepositInput input = new()
        {
            AccountId = SeedData.DefaultAccountId.Id,
            Amount = amount,
            Currency = "USD",
        };

        DepositUseCase.Command command = new(input);

        DepositUseCase.Handler handler = new(
             _fixture.EntityFactory,
            _fixture.AccountRepository,
            _fixture.CurrencyExchange,
            _fixture.UnitOfWork
            );

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

    }
    [Theory]
    [ClassData(typeof(InvalidDataSetup))]
    public async Task DepositUseCase_Returns_Invalid_When_Negative_Amount(decimal amount)
    {
        DepositInput input = new()
        {
            AccountId = SeedData.DefaultAccountId.Id,
            Amount = amount,
            Currency = "USD",
        };

        DepositUseCase.Command command = new(input);

        DepositUseCase.Handler handler = new(
             _fixture.EntityFactory,
            _fixture.AccountRepository,
            _fixture.CurrencyExchange,
            _fixture.UnitOfWork
            );

        DepositUseCase.CommandValidator validator = new();
        ValidationResult validationResult = validator.Validate(command);

        Assert.False(validationResult.IsValid);
        

    }
}