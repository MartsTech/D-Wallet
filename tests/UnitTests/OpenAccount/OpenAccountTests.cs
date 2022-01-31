namespace UnitTests.OpenAccount;

using Application.UseCases.OpenAccount;
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
        OpenAccountPresenter presenter = new();

        OpenAccountUseCase sut = new(
            _fixture.AccountFactory,
            _fixture.AccountRepositoryFake,
            _fixture.UnitOfWork,
            _fixture.TestUserService);

        sut.SetOutputPort(presenter);

        await sut.Execute(amount, currency);

        Assert.NotNull(presenter.Account);
    }
}
