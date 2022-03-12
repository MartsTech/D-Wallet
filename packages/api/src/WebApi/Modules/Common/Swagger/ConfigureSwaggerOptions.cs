namespace WebApi.Modules.Common.Swagger;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public sealed class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        options.SwaggerDoc("v1", CreateInfoForApiVersion());
    }

    private static OpenApiInfo CreateInfoForApiVersion()
    {
        OpenApiInfo info = new()
        {
            Title = "D-Wallet",
            Version = "v1",
            Description = "Digital Wallet in which customers can register an account and manage their balance with Deposit, Withdraw and Transfer operations",
            License = new OpenApiLicense { Name = "MIT" }
        };

        return info;
    }
}