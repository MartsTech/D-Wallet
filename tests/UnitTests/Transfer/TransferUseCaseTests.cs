namespace UnitTests.Transfer;

using Application.UseCases.Transfer;
using Domain.Accounts;
using Infrastructure.DataAccess;
using System.Threading.Tasks;
using Xunit;

public sealed class TransferUseCaseTests : IClassFixture<StandardFixture>
{
    private readonly StandardFixture _fixture;

    public TransferUseCaseTests(StandardFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [ClassData(typeof(ValidDataSetup))]
    public async Task TransferUseCase_Updates_Balance(
        decimal amount,
        decimal expectedOriginBalance)
    {
        TransferPresenter presenter = new();

        TransferUseCase sut = new(
            _fixture.AccountFactory,
            _fixture.AccountRepositoryFake,
            _fixture.CurrencyExchangeFake,
            _fixture.UnitOfWork);

        sut.SetOutputPort(presenter);

        await sut.Execute(
            SeedData.DefaultAccountId.Id,
            SeedData.SecondAccountId.Id,
            amount,
            "USD");

        Account? actual = presenter.OriginAccount!;

        Assert.Equal(expectedOriginBalance, actual.GetCurrentBalance().Amount);
    }
}
