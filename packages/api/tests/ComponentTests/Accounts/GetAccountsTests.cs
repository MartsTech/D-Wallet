namespace ComponentTests.Accounts;

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

[Collection("WebApi Collection")]
public sealed class GetAccountsTests
{
    private readonly WebApplicationFactoryFixture _fixture;

    public GetAccountsTests(WebApplicationFactoryFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetAccountsReturnsList()
    {
        HttpClient client = _fixture
            .WebApplicationFactory
            .CreateClient();

        HttpResponseMessage actualResponse = await client
            .GetAsync("/api/accounts/")
            .ConfigureAwait(false);

        Assert.Equal(HttpStatusCode.OK, actualResponse.StatusCode);
    }
}