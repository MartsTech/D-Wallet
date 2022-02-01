namespace WebApi.Modules;

using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.FeatureManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Mime;
using WebApi.Modules.Common.FeatureFlags;

public static class HealthChecksExtensions
{
    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration config)
    {
        IHealthChecksBuilder healthChecks = services.AddHealthChecks();

        IFeatureManager featureManager = services
            .BuildServiceProvider()
            .GetRequiredService<IFeatureManager>();

        bool isEnabled = featureManager
            .IsEnabledAsync(nameof(CustomFeature.SQLServer))
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();

        if (isEnabled)
        {
            healthChecks.AddDbContextCheck<DataContext>("DataContext");
        }

        return services;
    }

    public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder app)
    {
        HealthCheckOptions options = new() 
        { 
            ResponseWriter = WriteResponse 
        };

        app.UseHealthChecks("/health", options);

        return app;
    }

    private static Task WriteResponse(HttpContext context, HealthReport result)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;

        JObject json = new(
            new JProperty("status", result.Status.ToString()),
            new JProperty("results", new JObject(result.Entries.Select(pair =>
                new JProperty(pair.Key, new JObject(
                    new JProperty("status", pair.Value.Status.ToString()),
                    new JProperty("description", pair.Value.Description),
                    new JProperty("data", new JObject(pair.Value.Data.Select(
                        p => new JProperty(p.Key, p.Value))))))))));

        return context.Response.WriteAsync(json.ToString(Formatting.Indented));
    }
}