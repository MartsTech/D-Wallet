namespace UnitTests.Deposit;

using Application.Services;
using Application.UseCases.Deposit;
using Domain.Credits;
using Domain.ValueObjects;
using Infrastructure.DataAccess;
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
    public async Task DepositUseCase_Returns_Transaction(decimal amount)
    {
        DepositPresenter presenter = new();

        DepositUseCase sut = new(
            _fixture.AccountFactory,
            _fixture.AccountRepositoryFake,
            _fixture.CurrencyExchangeFake,
            _fixture.UnitOfWork);

        sut.SetOutputPort(presenter);

        await sut.Execute(
            SeedData.DefaultAccountId.Id,
            amount,
            Currency.Dollar.Code);

        Credit? output = presenter.Credit!;

        Assert.Equal(amount, output.Amount.Amount);
    }

    [Theory]
    [ClassData(typeof(InvalidDataSetup))]
    public async Task DepositUseCase_Returns_Invalid_When_Negative_Amount(decimal amount)
    {
        Notification notification = new();

        DepositPresenter presenter = new();

        DepositUseCase depositUseCase = new(
            _fixture.AccountFactory,
            _fixture.AccountRepositoryFake,
            _fixture.CurrencyExchangeFake,
            _fixture.UnitOfWork);

        DepositValidationUseCase sut = new(
             notification, depositUseCase);

        sut.SetOutputPort(presenter);

        await sut.Execute(
            SeedData.DefaultAccountId.Id,
            amount,
            Currency.Dollar.Code);

        Assert.True(presenter.InvalidOutput);
    }
}
