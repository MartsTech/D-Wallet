namespace WebApi.Modules.Common;

using Microsoft.AspNetCore.DataProtection;

public static class DataProtectionExtensions
{
    public static IServiceCollection AddCustomDataProtection(this IServiceCollection services)
    {
        services
            .AddDataProtection()
            .SetApplicationName("accounts-api")
            .PersistKeysToFileSystem(new DirectoryInfo(@"./"));

        return services;
    }
}
