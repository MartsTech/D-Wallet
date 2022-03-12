namespace WebApi.UseCases.Users.GetCurrentUser;

using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement.Mvc;
using System.Security.Claims;
using WebApi.Modules.Common.Authentication;
using WebApi.Modules.Common.FeatureFlags;

[FeatureGate(CustomFeature.GetCurrentUser)]
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
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetCurrentUserResponse))]
    public async Task<IActionResult> GetCurrentUser()
    {
        User? user = await _userManager
            .Users
            .FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));

        if (user is User currentUser)
        {
            await SetRefreshToken(currentUser);

            return Ok(new UserDto(currentUser, _tokenService.CreateToken(currentUser)));
        }

        return BadRequest("Failed to get current user.");
    }
}