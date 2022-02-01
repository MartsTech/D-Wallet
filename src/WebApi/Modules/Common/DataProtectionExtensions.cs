namespace WebApi.Modules.Common;

using Microsoft.AspNetCore.DataProtection;

public static class DataProtectionExtensions
{
    public static IServiceCollection AddCustomDataProtection(this IServiceCollection services)
    {
        services
            .AddDataProtection()
            .SetApplicationName("d-wallet")
            .PersistKeysToFileSystem(new DirectoryInfo(@"./"));

        return services;
    }
}
