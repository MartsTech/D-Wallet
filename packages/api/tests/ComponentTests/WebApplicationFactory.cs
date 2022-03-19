namespace ComponentTests;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Collections.Generic;

public sealed class WebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder) => builder.ConfigureAppConfiguration(
        (context, config) =>
    {
        config.AddInMemoryCollection(
            new Dictionary<string, string>
            {
                ["FeatureManagement:SQLServer"] = "true",
                ["FeatureManagement:CurrencyExchangeModule"] = "false"
            });
    });
}