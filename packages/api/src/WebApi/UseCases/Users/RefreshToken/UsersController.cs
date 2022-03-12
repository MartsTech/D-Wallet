namespace WebApi.UseCases.Users.RefreshToken;

using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement.Mvc;
using System.Security.Claims;
using WebApi.Modules.Common.Authentication;
using WebApi.Modules.Common.FeatureFlags;

[FeatureGate(CustomFeature.RefreshToken)]
public sealed class UsersController : BaseAuthController
{
    private readonly TokenService _tokenService;
    private readonly UserManager<User> _userManager;

    public UsersController(
        TokenService tokenService,
        UserManager<User> userManager) : base(tokenService, userManager)
    {
        _tokenService = tokenService;
        _userManager = userManager;
    }

    [Authorize]
    [HttpPost("refreshToken")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RefreshTokenResponse))]
    public async Task<IActionResult> RefreshToken()
    {
        string? refreshToken = Request.Cookies["refreshToken"];

        User? user = await _userManager.Users
                .Include(r => r.RefreshTokens)
                .FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));

        if (user == null)
        {
            return Unauthorized();
        }

        RefreshToken? oldToken = user.RefreshTokens
            .SingleOrDefault(x => x.Token == refreshToken);

        if (oldToken != null && !oldToken.IsActive)
        {
            return Unauthorized();
        }

        return Ok(new UserDto(user, _tokenService.CreateToken(user)));
    }
}