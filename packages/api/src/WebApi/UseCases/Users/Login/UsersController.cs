namespace WebApi.UseCases.Users.Login;

using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement.Mvc;
using WebApi.Modules.Common.Authentication;
using WebApi.Modules.Common.FeatureFlags;

[FeatureGate(CustomFeature.Login)]
public sealed class UsersController : BaseAuthController
{
    private readonly TokenService _tokenService;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public UsersController(
        TokenService tokenService, 
        UserManager<User> userManager,
        SignInManager<User> signInManager) : base(tokenService, userManager)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
    public async Task<IActionResult> Login(
        LoginInput input)
    {
        User? user = await _userManager
            .Users
            .FirstOrDefaultAsync(x => x.Email == input.Email)
            .ConfigureAwait(false);

        if (user == null)
        {
            return Unauthorized("Invalid email");
        }

        var result = await _signInManager
            .CheckPasswordSignInAsync(user, input.Password, false)
            .ConfigureAwait(false);

        if (!result.Succeeded)
        {
            return Unauthorized("Invalid password");
        }

        await SetRefreshToken(user)
            .ConfigureAwait(false);

        return Ok(new UserDto(user, _tokenService.CreateToken(user)));
    }
}