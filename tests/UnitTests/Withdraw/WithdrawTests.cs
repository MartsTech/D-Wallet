namespace UnitTests.Withdraw;

using Application.UseCases.Withdraw;
using Domain.Accounts;
using Infrastructure.DataAccess;
using System.Threading.Tasks;
using Xunit;

public sealed class WithdrawTests : IClassFixture<StandardFixture>
{
    private readonly StandardFixture _fixture;

    public WithdrawTests(StandardFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [ClassData(typeof(ValidDataSetup))]
    public async Task Withdraw_Returns_Account(
        decimal amount,
        decimal expectedBalance)
    {
        WithdrawPresenter presenter = new();

        WithdrawUseCase sut = new(
            _fixture.AccountFactory,
            _fixture.AccountRepositoryFake,
            _fixture.CurrencyExchangeFake,
            _fixture.UnitOfWork,
            _fixture.TestUserService);

        sut.SetOutputPort(presenter);

        await sut.Execute(SeedData.DefaultAccountId.Id, amount, "USD");

        Account? actual = presenter.Account!;

        Assert.Equal(expectedBalance, actual.GetCurrentBalance().Amount);
    }
}
