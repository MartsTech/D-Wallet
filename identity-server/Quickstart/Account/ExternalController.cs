using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Quickstart.Account;

public class ExternalController : Controller
{
    private readonly IClientStore _clientStore;
    private readonly IEventService _events;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly ILogger<ExternalController> _logger;
    private readonly TestUserStore _users;

    public ExternalController(IClientStore clientStore, IEventService events, IIdentityServerInteractionService interaction, ILogger<ExternalController> logger, TestUserStore users = null)
    {
        _clientStore = clientStore;
        _events = events;
        _interaction = interaction;
        _logger = logger;
        _users = users ?? new TestUserStore(TestUsers.Users);
    }

    [HttpGet]
    public IActionResult Challenge(string scheme, string returnUrl)
    {
        if (string.IsNullOrEmpty(returnUrl))
        {
            returnUrl = "~/";
        }

        if (Url.IsLocalUrl(returnUrl) == false && _interaction.IsValidReturnUrl(returnUrl) == false)
        {
            throw new Exception("invalid return URL");
        }

        var props = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(Callback)),
            Items = { { "returnUrl", returnUrl }, { "scheme", scheme } }
        };

        return Challenge(props, scheme);
    }

    [HttpGet]
    public async Task<IActionResult> Callback()
    {
        var result = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

        if (result?.Succeeded != true)
        {
            throw new Exception("External authentication error");
        }

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            var externalClaims = result.Principal.Claims.Select(c => $"{c.Type}: {c.Value}");
            _logger.LogDebug("External claims: {@claims}", externalClaims);
        }

        var (user, provider, providerUserId, claims) = FindUserFromExternalProvider(result);

        if (user == null)
        {
            user = AutoProvisionUser(provider, providerUserId, claims);

        } 

        var additionalLocalClaims = new List<Claim>();

        var localSignInProps = new AuthenticationProperties();

        ProcessLoginCallback(result, additionalLocalClaims, localSignInProps);

        var isuser = new IdentityServerUser(user.SubjectId)
        {
            DisplayName = user.Username,
            IdentityProvider = provider,
            AdditionalClaims = additionalLocalClaims
        };

        await HttpContext.SignInAsync(isuser, localSignInProps);

        await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

        var returnUrl = result.Properties.Items["returnUrl"] ?? "~/";

        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

        await _events.RaiseAsync(new UserLoginSuccessEvent(provider, providerUserId, user.SubjectId,
            user.Username, true, context?.Client.ClientId));

        if (context != null)
        {
            if (context.IsNativeClient())
            {
                return this.LoadingPage("Redirect", returnUrl);
            }
        }

        return Redirect(returnUrl);
    }

    private (TestUser user, string provider, string providerUserId, IEnumerable<Claim> claims)
     FindUserFromExternalProvider(AuthenticateResult result)
    {
        var externalUser = result.Principal;

        var userIdClaim = externalUser.FindFirst(JwtClaimTypes.Subject) ??
                          externalUser.FindFirst(ClaimTypes.NameIdentifier) ??
                          throw new Exception("Unknown userid");

        var claims = externalUser.Claims.ToList();
        claims.Remove(userIdClaim);

        var provider = result.Properties.Items["scheme"];
        var providerUserId = userIdClaim.Value;

        var user = _users.FindByExternalProvider(provider, providerUserId);

        return (user, provider, providerUserId, claims);
    }

    private TestUser AutoProvisionUser(string provider, string providerUserId, IEnumerable<Claim> claims)
    {
        var user = _users.AutoProvisionUser(provider, providerUserId, claims.ToList());
        return user;
    }

    private void ProcessLoginCallback(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
    {
        var sid = externalResult.Principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);

        if (sid != null)
        {
            localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
        }

        var idToken = externalResult.Properties.GetTokenValue("id_token");

        if (idToken != null)
        {
            localSignInProps.StoreTokens(new[] { new AuthenticationToken { Name = "id_token", Value = idToken } });
        }
    }
}
