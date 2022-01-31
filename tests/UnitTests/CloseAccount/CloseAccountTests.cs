namespace UnitTests.CloseAccount;

using Application.UseCases.CloseAccount;
using Application.UseCases.GetAccount;
using Application.UseCases.Withdraw;
using Domain.Accounts;
using Domain.Credits;
using Domain.ValueObjects;
using Infrastructure.DataAccess;
using System;
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
            .AccountFactory
            .NewAccount(Guid.NewGuid().ToString(), Currency.Dollar);

        Credit credit = _fixture
            .AccountFactory
            .NewCredit(account, new Money(amount, Currency.Dollar), DateTime.Now);

        account.Deposit(credit);

        bool actual = account.IsClosingAllowed();

        Assert.False(actual);
    }

    [Fact]
    public async Task CloseAccountUseCase_Returns_Closed_Account_Id_When_Account_Has_Zero_Balance()
    {
        GetAccountPresenter getAccountPresenter = new();

        CloseAccountPresenter closeAccountPresenter = new();

        GetAccountUseCase getAccountUseCase = new(_fixture.AccountRepositoryFake);

        WithdrawUseCase withdrawUseCase = new(
            _fixture.AccountFactory,
            _fixture.AccountRepositoryFake,
            _fixture.CurrencyExchangeFake,
            _fixture.UnitOfWork,
            _fixture.TestUserService);

        CloseAccountUseCase sut = new(
            _fixture.AccountRepositoryFake,
            _fixture.UnitOfWork,
            _fixture.TestUserService);

        sut.SetOutputPort(closeAccountPresenter);

        getAccountUseCase.SetOutputPort(getAccountPresenter);

        await getAccountUseCase.Execute(SeedData.DefaultAccountId.Id);

        IAccount getAccountDetailOutput = getAccountPresenter.Account!;

        await withdrawUseCase.Execute(
            SeedData.DefaultAccountId.Id,
            getAccountDetailOutput.GetCurrentBalance().Amount,
            getAccountDetailOutput.GetCurrentBalance().Currency.Code);

        await sut.Execute(SeedData.DefaultAccountId.Id);

        Assert.Equal(SeedData.DefaultAccountId.Id, closeAccountPresenter.Account!.AccountId.Id);
    }

    [Fact]
    public void IsClosingAllowed_Returns_True_When_Account_Does_Not_Has_Funds()
    {
        IAccount account = _fixture.AccountFactory
            .NewAccount(Guid.NewGuid().ToString(), Currency.Dollar);

        bool actual = account.IsClosingAllowed();

        Assert.True(actual);
    }
}
