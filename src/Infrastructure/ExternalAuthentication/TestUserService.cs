namespace Infrastructure.ExternalAuthentication;

using Application.Services;
using Infrastructure.DataAccess;

public sealed class TestUserService : IUserService
{
    public string GetCurrentUserId()
    {
        return SeedData.DefaultExternalUserId;
    }
}
