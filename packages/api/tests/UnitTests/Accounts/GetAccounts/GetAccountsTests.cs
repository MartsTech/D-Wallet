namespace UnitTests.Accounts.GetAccounts;

using Application.UseCases.Accounts.GetAccounts;
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
    public async Task GetAccounts_Is_Successful()
    {
        GetAccountsUseCase.Query command = new();

        GetAccountsUseCase.Handler handler = new(
            _fixture.AccountRepository,
            _fixture.UserService);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }
}