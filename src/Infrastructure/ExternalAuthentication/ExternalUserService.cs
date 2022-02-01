namespace Infrastructure.ExternalAuthentication;

using System.Security.Claims;
using Application.Services;
using Microsoft.AspNetCore.Http;

public sealed class ExternalUserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ExternalUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetCurrentUserId()
    {
        ClaimsPrincipal user = _httpContextAccessor.HttpContext!.User;

        string id = user.FindFirst("sub")?.Value!;

        return id;
    }
}
