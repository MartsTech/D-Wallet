namespace WebApi.UseCases.Users.Login;

using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using WebApi.Modules.Common;
using WebApi.Modules.Common.Authentication;
using WebApi.Modules.Common.FeatureFlags;

[ApiVersion("1.0")]
[FeatureGate(CustomFeature.Login)]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public sealed class UsersController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly TokenService _tokenService;

    public UsersController(UserManager<User> userManager, SignInManager<User> signInManager, TokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(LoginResponse))]
    [ApiConventionMethod(typeof(CustomApiConventions), nameof(CustomApiConventions.Post))]
    public async Task<IActionResult> Login(
        [FromForm][Required] string email,
        [FromForm][Required] string password)
    {
        var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.Email == email);

        if (user == null)
        {
            return Unauthorized("Invalid email");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

        if (result.Succeeded)
        {
            return Unauthorized("Invalid password");
        }

        await SetRefreshToken(user);

        return Ok(new LoginResponse(_tokenService.CreateToken(user)));
    }

    private async Task SetRefreshToken(User user)
    {
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshTokens.Add(refreshToken);

        await _userManager.UpdateAsync(user);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        Response.Cookies.Append("refreshToken", refreshToken.Token!, cookieOptions);
    }
}
