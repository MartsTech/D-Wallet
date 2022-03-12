namespace WebApi.Modules.Common.Authentication;

using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Persistence;

public sealed class TestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Claim[] claims =
        {
            new Claim(ClaimTypes.NameIdentifier, "test"),
            new Claim(ClaimTypes.Name, "test"),
            new Claim("id", SeedData.DefaultUserId)
        };

        ClaimsIdentity identity = new(claims, Scheme.Name);

        ClaimsPrincipal principal = new(identity);

        AuthenticationTicket ticket = new(principal, Scheme.Name);

        return await Task
            .FromResult(AuthenticateResult.Success(ticket))
            .ConfigureAwait(false);
    }
}