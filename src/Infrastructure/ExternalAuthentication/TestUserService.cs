using Application.Services;
using Infrastructure.DataAccess;

namespace Infrastructure.ExternalAuthentication;

public sealed class TestUserService : IUserService
{
    public string GetCurrentUserId()
    {
        return SeedData.DefaultExternalUserId;
    }
}
