using Microsoft.AspNetCore.DataProtection;

namespace IdentityServer.Modules.Common;

public static class DataProtectionExtensions
{
    public static IServiceCollection AddCustomDataProtection(this IServiceCollection services)
    {
        services.AddDataProtection()
            .SetApplicationName("identity-server")
            .PersistKeysToFileSystem(new DirectoryInfo(@"./"));

        return services;
    }
}
