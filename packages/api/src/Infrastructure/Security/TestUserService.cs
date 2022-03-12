namespace Infrastructure.Security;

using Application.Services;
using Persistence;

public sealed class TestUserService : IUserService
{
    public string GetCurrentUserId()
    {
        return SeedData.DefaultUserId;
    }
}