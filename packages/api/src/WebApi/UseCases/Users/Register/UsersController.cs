namespace WebApi.UseCases.Users.Register;

using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement.Mvc;
using WebApi.Modules.Common.Authentication;
using WebApi.Modules.Common.FeatureFlags;

[FeatureGate(CustomFeature.Register)]
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

    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisterResponse))]
    public async Task<IActionResult> Register(
        RegisterInput input)
    {
        Console.WriteLine(input.Password);

        bool emailExists = await _userManager
            .Users
            .AnyAsync(x => x.Email == input.Email)
            .ConfigureAwait(false);

        if (emailExists)
        {
            ModelState.AddModelError("email", "Email taken");
            return ValidationProblem();
        }

        bool usernameExists = await _userManager
            .Users
            .AnyAsync(x => x.UserName == input.Username)
            .ConfigureAwait(false);

        User user = new() 
        { 
            Email = input.Email,
            UserName =  input.Username
        };

        var result = await _userManager
            .CreateAsync(user, input.Password)
            .ConfigureAwait(false);

        if (!result.Succeeded)
        {
            return BadRequest("Problem registering user");
        }

        await SetRefreshToken(user)
            .ConfigureAwait(false);

        return Ok(new UserDto(user, _tokenService.CreateToken(user)));
    }
}