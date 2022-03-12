namespace WebApi.UseCases.Users;

using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Modules.Common.Authentication;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseAuthController : ControllerBase
{
    private readonly TokenService _tokenService;
    private readonly UserManager<User> _userManager;

    protected BaseAuthController(TokenService tokenService, UserManager<User> userManager)
    {
        _tokenService = tokenService;
        _userManager = userManager;
    }

    protected async Task SetRefreshToken(User user)
    {
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshTokens.Add(refreshToken);

        await _userManager.UpdateAsync(user)
            .ConfigureAwait(false);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        Response.Cookies.Append("refreshToken", refreshToken.Token!, cookieOptions);
    }
}